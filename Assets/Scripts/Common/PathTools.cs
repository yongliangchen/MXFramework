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
    /// <summary>获取数据存放路径</summary>
    public static string DataPath
    {
        get
        {
            string path = string.Empty;
            if (Application.isEditor) path = Application.streamingAssetsPath + "/Data";
            else path = Application.persistentDataPath + "/Data";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path;
        }
    }

    /// <summary>存放程序初始化时候的资源</summary>
    public static string InitialResPath
    {
        get
        {
            string path = DataPath + "/Initial";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path;
        }
    }

    /// <summary>存放下载的资源</summary>
    public static string Download
    {
        get
        {
            string path = DataPath + "/Download";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path;
        }
    }

}
