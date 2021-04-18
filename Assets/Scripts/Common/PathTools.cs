/***
 * 
 *    Title: MXFramework
 *           主题: 路径工具
 *    Description: 
 *           功能：1.管理一些公共的路径定义
 *                                  
 *    Date: 2021
 *    Version: v6.0版本
 *    Modify Recoder:      
 *
 */

using System.IO;
using UnityEngine;

/// <summary>路径</summary>
public class PathTools 
{
    /// <summary>获取资源存放目录</summary>
    public static string AssetDirectory
    {
        get
        {
            string path = string.Empty;

            if (Application.isEditor)
            {
                path = Application.streamingAssetsPath + "/Data";
            }
            else
            {
                path = Application.persistentDataPath + "/Data";
            }

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path;
        }
    }

    /// <summary>存放程序初始化时候的资源</summary>
    public static string InitialResPath
    {
        get
        {
            string path = AssetDirectory + "/Initial";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path;
        }
    }

    /// <summary>存放下载的资源</summary>
    public static string Download
    {
        get
        {
            string path = AssetDirectory + "/Download";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path;
        }
    }

    /// <summary>服务器地址</summary>
    public static string ServerURL = "https://mxframework.oss-cn-shenzhen.aliyuncs.com/";

    /// <summary>获取资源清单路径</summary>
    public static string GetAssetFilesListPath()
    {
        string assetFilesListURL = string.Empty;

        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.IPhonePlayer:

                assetFilesListURL = "Data/Initial/iOSFiles.txt";
                break;

            case RuntimePlatform.Android:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:

                assetFilesListURL = "Data/Initial/AndroidFiles.txt";
                break;
        }

        return assetFilesListURL;
    }


}
