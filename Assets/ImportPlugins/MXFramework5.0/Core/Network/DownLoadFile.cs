/***
 * 
 *    Title: MXFramework
 *           主题: 文件下载
 *    Description: 
 *           功能：1.封装了拷贝文件
 *                2.封装了获取头文件信息
 *                2.封装了下载资源(重新下载和断点续传2种)
 *                
 *    Date: 2020
 *    Version: v5.0版本
 *    Modify Recoder:     
 */

using System;
using System.Collections.Generic;
using System.IO;
using Mx.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace Mx.Net
{
    /// <summary>下载文件</summary>
    public class DownLoadFile : BaseUnityWebRequest
    {
        private static DownLoadFile instance;
        public static DownLoadFile Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject mounter = new GameObject("DownLoadFile");
                    instance = mounter.AddComponent<DownLoadFile>();
                }
                return instance;
            }
        }

        /// <summary>获取头文件信息</summary>
        public new void GetHeadFile(string url, Action<UnityWebRequest> callback, int timeout = 0)
        {
            StartCoroutine(base.GetHeadFile(url, callback, timeout));
        }

        /// <summary>获取文件最后修改时间用来做版本号（也可以获取MD5）</summary>
        public void GetLastModified(string url,Action<string> callback)
        {
            string version = string.Empty;

            GetHeadFile(url, (uwr) =>
            {
                if (string.IsNullOrEmpty(uwr.error))
                {
                    Dictionary<string, string> headers = uwr.GetResponseHeaders();
                    headers.TryGetValue("Last-Modified", out version);
                    version = StringEncrypt.GetStringMd5(version);
                }
                else
                {
                    Debug.LogWarning(GetType() + "/GetHeadFile()/error!" + uwr.error);
                }

                if(callback!=null) callback(version);

            }, 10);
        }

        /// <summary>下载文件(没有断点续传)</summary>
        public new void Download(string url, string savePath, DelWebRequestCallback callback = null, int timeout = 0)
        {
            coroutines.Add(url,StartCoroutine(base.Download(url, savePath, callback, timeout)));
        }

        /// <summary>下载文件(断点续传)</summary>
        public new void Download(string url, string savePath, string version, DelWebRequestCallback callback = null, int timeout = 0)
        {
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            if (!downReqMap.ContainsKey(url))
            {
                coroutines.Add(url, StartCoroutine(base.Download(url, savePath, version, callback, timeout)));
            }
        }
    }
}