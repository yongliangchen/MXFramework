/***
 * 
 *    Title: MXFramework
 *           主题: UnityWebRequest工具
 *    Description: 
 *           功能：1.UnityWebRequest常用的API进行封装
 * 
 *    Date: 2020
 *    Version: v5.0版本
 *    Modify Recoder: 
 *      
 */

using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Mx.Net
{
    public class WebRequest :BaseUnityWebRequest
    {
        private static WebRequest instance;
        public static WebRequest Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject mounter = new GameObject("_WebRequest");
                    instance = mounter.AddComponent<WebRequest>();
                }
                return instance;
            }
        }

        public void Get(string url, Action<UnityWebRequest> callback = null, int timeout = 0)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(url);
            uwr.timeout = timeout;
            StartCoroutine(base.Get(uwr, callback));
        }

        public new void GetHeadFile(string url, Action<UnityWebRequest> callback, int timeout = 0)
        {
           StartCoroutine(base.GetHeadFile(url, callback, timeout));
        }

        public new void GetTexture(string url, Action<float> progress, DelGetTextureCallback callback, int timeout = 0)
        {
            coroutines.Add(url,StartCoroutine(base.GetTexture(url, progress, callback, timeout)));
        }

        public new void GetText(string url, Action<float> progress, DelGetTextCallback callback, int timeout = 0)
        {
            coroutines.Add(url,StartCoroutine(base.GetText(url, progress, callback, timeout)));
        }

        public new void GetAssetBundle(string url, Action<float> progress, DelGetAbCallback callback, int timeout = 0)
        {
            coroutines.Add(url,StartCoroutine(base.GetAssetBundle(url, progress, callback, timeout)));
        }

        public new void GetAudioClip(string url, AudioType audioType, Action<float> progress, DelGetAudioClipCallback callback, int timeout = 0)
        {
            coroutines.Add(url,StartCoroutine(base.GetAudioClip(url, audioType, progress, callback, timeout)));
        }

        public void Post(string url, WWWForm form, Action<UnityWebRequest> callback = null, int timeout = 0)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(url, form);
            uwr.timeout = timeout;
            StartCoroutine(base.Post(uwr, callback));
        }

    }
}