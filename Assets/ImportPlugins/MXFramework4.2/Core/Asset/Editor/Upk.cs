using System;
using System.Collections.Generic;
using System.IO;
using Mx.Util;
using UnityEditor;
using UnityEngine;

namespace Mx.Res
{
    public class UPK
    {
        /// <summary>压缩和加密</summary>
		[MenuItem("MXFramework/AssetBundle/Compression", false, 211)]
		public static void CompressionsAndEncrypt()
		{
			if (Directory.Exists(AssetDefine.UpkOutPant))
			{
				Directory.Delete(AssetDefine.UpkOutPant, true);
			}
			Directory.CreateDirectory(AssetDefine.UpkOutPant);

			Debug.Log("Upk/Compressions()/ upkOutPath:" + AssetDefine.UpkOutPant);

            Compressions();
            CopyManifest();

            if (Directory.Exists(AssetDefine.UpkTempCompressionPath)) Directory.Delete(AssetDefine.UpkTempCompressionPath, true);
			AssetDatabase.Refresh();//刷新
		}

        /// <summary>压缩全部文件</summary>
        private static void Compressions()
        {
            string androidFileListPath = AssetDefine.GetBuildAssetOutPath() + "/Android/files.txt";
            string iosFileListPath = AssetDefine.GetBuildAssetOutPath() + "/iOS/files.txt";
            string windowsFileListPath = AssetDefine.GetBuildAssetOutPath() + "/Windows/files.txt";
            string OSXFileListPath = AssetDefine.GetBuildAssetOutPath() + "/OSX/files.txt";

            Filter(androidFileListPath, "Android");
            Filter(iosFileListPath, "iOS");
            Filter(windowsFileListPath, "Windows");
            Filter(OSXFileListPath, "OSX");
        }

        /// <summary>过滤</summary>
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

        /// <summary>压缩算法</summary>
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

        /// <summary>读取文本数据数组</summary>
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

        /// <summary>打包</summary>
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

        /// <summary>拷贝Manifest文件</summary>
        private static void CopyManifest()
        {
            string androidManifestInPath = AssetDefine.GetBuildAssetOutPath() + "/Android/Android";
            string iosManifestInPath = AssetDefine.GetBuildAssetOutPath() + "/iOS/iOS";
            string windowsManifestInPath = AssetDefine.GetBuildAssetOutPath() + "/Windows/Windows";
            string OSXManifestInPath = AssetDefine.GetBuildAssetOutPath() + "/OSX/OSX";

            string androidManifestOutPath = AssetDefine.UpkOutPant + "/Android/Android";
            string iosManifestOutPath = AssetDefine.UpkOutPant + "/iOS/iOS";
            string windowsManifestOutPath = AssetDefine.UpkOutPant + "/Windows/Windows";
            string OSXManifestOutPath = AssetDefine.UpkOutPant + "/OSX/OSX";

            if (File.Exists(androidManifestInPath)) CopyFiles.Copy(androidManifestInPath, androidManifestOutPath);
            if (File.Exists(iosManifestInPath)) CopyFiles.Copy(iosManifestInPath, iosManifestOutPath);
            if (File.Exists(windowsManifestInPath)) CopyFiles.Copy(windowsManifestInPath, windowsManifestOutPath);
            if (File.Exists(OSXManifestInPath)) CopyFiles.Copy(OSXManifestInPath, OSXManifestOutPath);
        }
    }
}