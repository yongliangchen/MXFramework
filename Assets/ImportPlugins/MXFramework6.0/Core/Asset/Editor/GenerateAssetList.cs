/***
 * 
 *    Title: MXFramework
 *           主题: 生成资源清单
 *    Description: 
 *           功能：自动生成资源清单
 * 
 *    Date: 2021
 *    Version: v6.0版本
 *    Modify Recoder: 
 *      
 */

using System;
using System.Collections.Generic;
using System.IO;
using Mx.Utils;
using UnityEditor;
using UnityEngine;

namespace Mx.Res
{
    /// <summary>
    /// 生成资源清单
    /// </summary>
    public class GenerateAssetList
    {
        [MenuItem("MXFramework/Generate Asset List", false, 501)]
        public static void GenerateFiles()
        {
            string resPath = PathTools.InitialResPath;

            string iOSFilePath = PathTools.InitialResPath + "/iOSFiles.txt";
            string iOSExcludePath = resPath+"/AssetsBundles/Android";
            if (File.Exists(iOSFilePath)) File.Delete(iOSFilePath);

            string androidFilePath = PathTools.InitialResPath + "/AndroidFiles.txt";
            string androidExcludePath = resPath+ "/AssetsBundles/iOS";
            if (File.Exists(androidFilePath)) File.Delete(androidFilePath);

            string[] files = System.IO.Directory.GetFiles(resPath, "*.*", SearchOption.AllDirectories);
            if (files == null || files.Length == 0) return;

            createFiles(iOSFilePath, files, iOSExcludePath);
            createFiles(androidFilePath, files, androidExcludePath);

            Debug.Log("生成资源清单完成!");
            AssetDatabase.Refresh();//刷新
        }

        /// <summary>
        /// 创建资源清单
        /// </summary>
        /// <param name="outPath">资源清单输出路径</param>
        /// <param name="files">全部资源集合</param>
        /// <param name="excludePath">需要排除的资源路径</param>
        private static void createFiles(string outPath, string[] files, string excludePath)
        {
            if (File.Exists(outPath)) File.Delete(outPath);

            List<AssetInfo> listAsset = new List<AssetInfo>();
            AssetInfo[] asetArr = null;
            long totalLength = 0;

            for (int i = 0; i < files.Length; ++i)
            {
                FileInfo fileInfo = new FileInfo(files[i]);
                if (!Filter(fileInfo)|| fileInfo.FullName.StartsWith(excludePath)) continue;

                string localPath = fileInfo.FullName.Replace(PathTools.InitialResPath + "/", null);
                string directory = localPath.Substring(0, localPath.Length - fileInfo.Name.Length);
                AssetInfo assetInfo = new AssetInfo();
                assetInfo.name = fileInfo.Name;
                assetInfo.directory = directory;
                assetInfo.md5 = StringEncrypt.GetFileMd5(fileInfo.FullName);
                assetInfo.length = fileInfo.Length;

                listAsset.Add(assetInfo);
                totalLength += assetInfo.length;
            }

            if (listAsset.Count > 0) asetArr = listAsset.ToArray();
            if (asetArr != null && asetArr.Length > 0)
            {
                AssetList assetList = new AssetList();
                assetList.count = asetArr.Length;
                assetList.length = totalLength;
                assetList.filesList = asetArr;
                string jsonData = JsonUtility.ToJson(assetList);
                writer(outPath, jsonData);
            }
        }

        /// <summary>写入文本</summary>
        private static void writer(string filePath,string data)
        {
            FileStream fs = new FileStream(filePath, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine(data);

            sw.Close();
            fs.Close();
        }

        /// <summary>筛选</summary>
        private static bool Filter(FileInfo fileInfo)
        {
            if (fileInfo.Extension == ".meta" || fileInfo.Extension == ".DS_Store" || fileInfo.Extension == ".cs" ||
                fileInfo.Extension == ".dll" || fileInfo.Extension == ".cpp" || fileInfo.Extension == ".a"
                || fileInfo.Extension == ".so"

               ) return false;

            else return true;
        }
    }

    [Serializable]
    public class AssetInfo
    {
        /// <summary>资源名称</summary>
        public string name;
        /// <summary>资源目录</summary>
        public string directory;
        /// <summary>Md5值（版本号）</summary>
        public string md5;
        /// <summary>资源大小（字节）</summary>
        public long length;
    }

    [Serializable]
    public class AssetList
    {
        /// <summary>资源数量</summary>
        public int count;
        /// <summary>所有资源总的大小</summary>
        public long length;
        /// <summary>资源清单</summary>
        public AssetInfo[] filesList;
    }

}

