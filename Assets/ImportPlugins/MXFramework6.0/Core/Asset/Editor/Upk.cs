using System;
using System.Collections.Generic;
using System.IO;
using Mx.Utils;
using UnityEditor;
using UnityEngine;

namespace Mx.Res
{
    /// <summary>资源压缩和加密类</summary>
    public class Upk
    {
        /// <summary>压缩资源调用接口</summary>
		//[MenuItem("MXFramework/AssetBundle/Compression", false, 211)]
		public static void Compressions()
		{
			if (Directory.Exists(AssetDefine.UpkOutPant))
			{
				Directory.Delete(AssetDefine.UpkOutPant, true);
			}
			Directory.CreateDirectory(AssetDefine.UpkOutPant);

			Debug.Log("Upk/Compressions()/ upkOutPath:" + AssetDefine.UpkOutPant);

			string androidFileListPath = AssetDefine.GetBuildAssetOutPath() + "/" + GetPlatformName(BuildTarget.Android) + "/files.txt";
			string iosFileListPath = AssetDefine.GetBuildAssetOutPath() + "/" + GetPlatformName(BuildTarget.iOS) + "/files.txt";
			string windowsFileListPath = AssetDefine.GetBuildAssetOutPath() + "/" + GetPlatformName(BuildTarget.StandaloneWindows64) + "/files.txt";
			string OSXFileListPath = AssetDefine.GetBuildAssetOutPath() + "/" + GetPlatformName(BuildTarget.StandaloneOSX) + "/files.txt";

			Filter(androidFileListPath, GetPlatformName(BuildTarget.Android));
			Filter(iosFileListPath, GetPlatformName(BuildTarget.iOS));
			Filter(windowsFileListPath, GetPlatformName(BuildTarget.StandaloneWindows64));
			Filter(OSXFileListPath, GetPlatformName(BuildTarget.StandaloneOSX));

			if (Directory.Exists(AssetDefine.UpkTempCompressionPath)) Directory.Delete(AssetDefine.UpkTempCompressionPath, true);

			DelAssetsBundle();

			AssetDatabase.Refresh();//刷新
		}

        /// <summary>
		/// 对资源进行筛选（过滤掉不需要压缩的资源）
		/// </summary>
		/// <param name="fileListPath">需要压缩资源，清单路径</param>
		/// <param name="outFolder">压缩完成后，压缩包输出路径</param>
        private static void Filter(string fileListPath,string outFolder)
        {
            string[] fileListArr = ReadTextDataArray(fileListPath);
            if (fileListArr == null) return;

            for(int i =0;i< fileListArr.Length;i++)
            {
                string[] tempArr = fileListArr[i].Split('|');

                if (tempArr[0].EndsWith(AssetDefine.AB_RES_EXTENSIONS) || tempArr[0].EndsWith(AssetDefine.AB_SCENE_EXTENSIONS))
                {
                    Compression(tempArr[0], outFolder);
                }
            }
        }

		/// <summary>
		/// 压缩资源
		/// </summary>
		/// <param name="filePath">需要压缩资源路径</param>
		/// <param name="outFolder">压缩完成后，压缩包输出路径</param>
		private static void Compression(string filePath, string outFolder)
		{
			string inPath = AssetDefine.GetBuildAssetOutPath() + "/" + outFolder;
			string[] fileNameArr = filePath.Split('/');
			string fileDir = filePath.Replace('/' + fileNameArr[fileNameArr.Length - 1], null);
			fileNameArr = fileNameArr[fileNameArr.Length - 1].Split('.');
			string fileName = fileNameArr[0];

			string[] inPathArr = new string[2];
			inPathArr[0] = inPath + "/" + filePath;
			inPathArr[1] = inPath + "/" + filePath + ".manifest";

			string outDir = AssetDefine.UpkOutPant + "/" + outFolder + "/" + fileDir + '/';
			if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);
			string outPath = outDir + fileName + "." + AssetDefine.UPK_EXTENSIONS;

			PackFolder(fileDir, fileName, inPathArr, outPath);
		}

        /// <summary>
		/// 获取当前设备平台名称
		/// </summary>
		/// <param name="buildTarget">发布平台</param>
		/// <returns>返回sting类型</returns>
        private static string GetPlatformName(BuildTarget buildTarget)
        {
            string targetName = string.Empty;
            switch (buildTarget)
            {
                case BuildTarget.Android: targetName = "Android"; break;
                case BuildTarget.iOS: targetName = "iOS"; break;
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneWindows: targetName = "Windows"; break;
                case BuildTarget.StandaloneOSX: targetName = "OSX"; break;
                default: Debug.LogError("打包的平台不存在! buildTarget=" + buildTarget); break;
            }

            return targetName;
        }

        /// <summary>
		/// 读取Text文件数据
		/// </summary>
		/// <param name="textPath">需要读取的文本</param>
		/// <returns></returns>
        private static string[] ReadTextDataArray(string textPath)
        {
            List<string> temp = new List<string>();
            StreamReader streamReader = null;

            if (File.Exists(textPath)) streamReader = File.OpenText(textPath);
            else { return null; }

            string str;
            while ((str = streamReader.ReadLine()) != null) { if (!string.IsNullOrEmpty(str)) { temp.Add(str); } }
            return temp.ToArray();
        }

        /// <summary>
		/// 打包文件夹
		/// </summary>
		/// <param name="fileDir">文件目录</param>
		/// <param name="folderName">文件名称</param>
		/// <param name="inPathArr">输入文件路径集合</param>
		/// <param name="outPath">输出文件路径集合</param>
		/// <param name="progress">打包进度</param>
		private static void PackFolder(string fileDir, string folderName, string[] inPathArr, string outPath, Action<float> progress = null)
		{
			string temp = AssetDefine.UpkTempCompressionPath + "/" + folderName + "/" + fileDir;
			if (Directory.Exists(temp)) Directory.Delete(temp, true);
			Directory.CreateDirectory(temp);

			for (int i = 0; i < inPathArr.Length; i++)
			{
				string pStrFilePath = inPathArr[i];
				string[] inpathArr = pStrFilePath.Split('/');
				string pPerFilePath = temp + "/" + inpathArr[inpathArr.Length - 1];
				CopyFiles.Copy(pStrFilePath, pPerFilePath);
			}

			UpkUtil.PackFolder(temp, outPath, progress);
		}

        /// <summary>清理本地的资源</summary>
        private static void DelAssetsBundle()
		{
			string strNeedDeleteDIR = string.Empty;
			strNeedDeleteDIR = AssetDefine.GetBuildAssetOutPath();

			//目录信息（场景中的目录信息数组）
			DirectoryInfo[] dirScenesDIRArr = null;
			DirectoryInfo dirTempInfo = new DirectoryInfo(strNeedDeleteDIR);
			dirScenesDIRArr = dirTempInfo.GetDirectories();

			foreach (DirectoryInfo currentDIR in dirScenesDIRArr)
			{
				JudgeDIRorFileBuyRecursive(currentDIR);
			}
		}

		/// <summary>
		/// 递归判断是目录还是文件，判断是文件类型还是文件夹类型
		/// </summary>
		/// <param name="fileSystemInfo"> fileSystemInfo </param>
		/// <param name="scenesName">Scenes name.</param>
		private static void JudgeDIRorFileBuyRecursive(FileSystemInfo fileSystemInfo)
		{
			if (!fileSystemInfo.Exists){return;}

			DirectoryInfo dirInfoObj = fileSystemInfo as DirectoryInfo;
			FileSystemInfo[] fileSysArray = dirInfoObj.GetFileSystemInfos();

			foreach (FileSystemInfo fileInfo in fileSysArray)
			{
				FileInfo fileInfoObj = fileInfo as FileInfo;

				//文件类型
				if (fileInfoObj != null)
				{
					//Debug.Log(fileInfoObj.FullName);
					if (!fileInfoObj.Name.Equals(fileSystemInfo.Name)) File.Delete(fileInfoObj.FullName);
				}
				//目录类型
				else
				{
					//Debug.Log(fileInfo.FullName);
					Directory.Delete(fileInfo.FullName,true);
				}
			}
		}

	}
}