using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Mx.Res
{

    /// <summary>拷贝文件</summary>
    public class CopyAsset : MonoBehaviour
    {
        [MenuItem("MXFramework/Output Data/Copy Files", false, 502)]
        public static void CopyFiles()
        {
            string outDirectory = AssetDefine.CopyAssetOutPant;
            if (Directory.Exists(outDirectory)) Directory.Delete(outDirectory, true);
            Directory.CreateDirectory(outDirectory);

            string resPath = PathTools.InitialResPath;
            string[] files = System.IO.Directory.GetFiles(resPath, "*.*", SearchOption.AllDirectories);
            if (files == null || files.Length == 0) return;

            for (int i = 0; i < files.Length; ++i)
            {
                FileInfo fileInfo = new FileInfo(files[i]);
                if (!AssetDefine.FilterFormat(fileInfo)) continue;

                string localPath = fileInfo.FullName.Replace(Application.streamingAssetsPath + "/", null);
                string directory = outDirectory + "/" + localPath.Substring(0, localPath.Length - fileInfo.Name.Length);

                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                string inPath = fileInfo.FullName;
                string outPath = outDirectory + "/" + localPath;

                copyFile(inPath, outPath, null);
            }

            Debug.Log("拷贝文件完成！" + AssetDefine.CopyAssetOutPant);
        }

        /// <summary>拷贝单个文件</summary>
        private static void copyFile(string inPath, string outPath, Action finish)
        {
            if (!File.Exists(outPath.Replace("file://", null)))
            {
                Mx.Utils.CopyFiles.Copy(inPath, outPath, null, (uwr) =>
                {
                    if (!string.IsNullOrEmpty(uwr.error))
                    {
                        Debug.LogWarning("CopyAsset/CopyFile()/ copy error! " + uwr.error);
                    }
                    else { if (finish != null) finish(); }
                });
            }
            else { if (finish != null) finish(); }
        }
    }
}