using System.IO;
using UnityEditor;
using UnityEngine;

namespace Mx.Res
{
    /// <summary>自动设置资源清单标签</summary>
    public class AutoSetLabels
    {
        //[MenuItem("MXFramework/AssetBundle/Set AssetBundle Label", false, 201)]
        public static void SetAbLabel()
        {
            string abFolder = AssetDefine.GetABResourcePath();
            if (!Directory.Exists(abFolder)) { Directory.CreateDirectory(abFolder); }

            FileInfo[] files = DirectoryEx.GetFiles(abFolder, Filter);
            foreach (FileInfo fileInfo in files)
            {
                SetFileABLabel(fileInfo);
            }

            //清空无用AB标记
            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetDatabase.Refresh();//刷新
            Debug.Log("自动设置标志完成！");
        }

        [MenuItem("MXFramework/AssetBundle/Clear AssetBundle Label", false, 202)]
        public static void ClearAbLabels()
        {
            string path = Application.dataPath;
            string[] files = System.IO.Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            if ((files != null) && (files.Length > 0))
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    string fileName = files[i];

                    FileInfo fileInfo = new FileInfo(fileName);

                    if (Filter(fileInfo))
                    {
                        int index = fileName.IndexOf("Assets");
                        fileName = fileName.Substring(index);
                        AssetImporter importer = AssetImporter.GetAtPath(fileName);

                        if (importer != null) importer.assetBundleName = string.Empty;
                    }
                }
            }

            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetDatabase.Refresh();
            Debug.Log("清空 AssetBundle Labels 完成！");
        }

        /// <summary>对指定文件设置AB包名</summary>
        private static void SetFileABLabel(FileInfo fileInfoObj)
        {
            string strABName = string.Empty;
            string strAssetFilePath = string.Empty;

            strABName = GetABName(fileInfoObj);

            int tmpIndex = fileInfoObj.FullName.IndexOf("Assets");
            strAssetFilePath = fileInfoObj.FullName.Substring(tmpIndex);
            AssetImporter tmpInmporterObj = AssetImporter.GetAtPath(strAssetFilePath);
            tmpInmporterObj.assetBundleName = strABName;

            if (fileInfoObj.Extension == ".unity")
            {
                tmpInmporterObj.assetBundleVariant = "u3d";
            }
            else
            {
                tmpInmporterObj.assetBundleVariant = "data";
            }
        }

        /// <summary>获取Ab包名</summary>
        private static string GetABName(FileInfo fileInfoObj)
        {
            string strABName = string.Empty;
            string strABPath = fileInfoObj.FullName.Replace(fileInfoObj.Extension, null).Replace(AssetDefine.GetABResourcePath(), null);
            string tmpUnityPath = strABPath.Replace("\\", "/");
            string[] strAbPathArr = tmpUnityPath.Split('/');

            if (strAbPathArr.Length < 3)
            {
                strABName = strAbPathArr[1] + "/" + strAbPathArr[1];
            }
            else
            {
                strABName = strAbPathArr[1] + "/" + strAbPathArr[2];
            }

            return strABName;
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
}