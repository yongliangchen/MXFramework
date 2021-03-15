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

        public new void Get(string uri, Action<UnityWebRequest> callback = null, int timeout = 0)
        {
            StartCoroutine(base.Get(uri, callback,timeout));
        }

        public new void GetHeadFile(string uri, Action<UnityWebRequest> callback, int timeout = 0)
        {
           StartCoroutine(base.GetHeadFile(uri, callback, timeout));
        }

        public new void GetTexture(string uri, Action<float> progress, DelGetTextureCallback callback, int timeout = 0)
        {
            if (!downReqMap.ContainsKey(uri))
            {
                coroutines.Add(uri, StartCoroutine(base.GetTexture(uri, progress, callback, timeout)));
            }
        }

        public new void GetText(string uri, Action<float> progress, DelGetTextCallback callback, int timeout = 0)
        {
            if (!downReqMap.ContainsKey(uri))
            {
                coroutines.Add(uri, StartCoroutine(base.GetText(uri, progress, callback, timeout)));
            }
        }

        public new void GetAssetBundle(string uri, Action<float> progress, DelGetAbCallback callback, int timeout = 0)
        {
            if (!downReqMap.ContainsKey(uri))
            {
                coroutines.Add(uri, StartCoroutine(base.GetAssetBundle(uri, progress, callback, timeout)));
            }
        }

        public new void GetAudioClip(string uri, AudioType audioType, Action<float> progress, DelGetAudioClipCallback callback, int timeout = 0)
        {
            if (!downReqMap.ContainsKey(uri))
            {
                coroutines.Add(uri, StartCoroutine(base.GetAudioClip(uri, audioType, progress, callback, timeout)));
            }
        }

        public void Post(string uri, WWWForm form, Action<UnityWebRequest> callback = null, int timeout = 0)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(uri, form);
            uwr.timeout = timeout;
            StartCoroutine(base.Post(uwr, callback));
        }

    }
}