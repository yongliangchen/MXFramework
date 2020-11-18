/***
 * 
 *    Title: MXFramework
 *           主题: UnityWebRequest的基类
 *    Description: 
 *           功能：1.封装了Get请求
 *                2.封装了Pos请求
 *                3.封装了下载资源
 *                4.封装了上传资源
 *                
 *    Date: 2020
 *    Version: v5.0版本
 *    Modify Recoder:     
 */

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Mx.Net
{
    public class BaseUnityWebRequest : MonoBehaviour
    {
        public void GetHeadFile(string url, Action<UnityWebRequest> callback, int timeout = 0)
        {
            StartCoroutine(getHeadFile(url, callback, timeout));
        }

        public void Get(string url, Action<UnityWebRequest> callback = null, int timeout = 0)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(url);
            uwr.timeout = timeout;

            StartCoroutine(Get(uwr, callback));
        }

        public virtual IEnumerator Get(UnityWebRequest uwr, Action<UnityWebRequest> callback = null)
        {
            using (uwr)
            {
                yield return uwr.SendWebRequest();
                if (callback != null) { callback(uwr); }
            }
        }

        public void GetTexture(string url, Action<float> progress, DelGetTextureCallback callback, int timeout=0)
        {
            StartCoroutine(getTexture(url, progress, callback, timeout));
        }

        public void GetText(string url, Action<float> progress, DelGetTextCallback callback, int timeout=0)
        {
            StartCoroutine(getText(url, progress, callback, timeout));
        }

        public void GetAssetBundle(string url, Action<float> progress, DelGetAbCallback callback, int timeout=0)
        {
            StartCoroutine(getAssetBundle(url, progress, callback, timeout));
        }

        public void GetAudioClip(string url, AudioType audioType, Action<float> progress, DelGetAudioClipCallback callback, int timeout=0)
        {
            StartCoroutine(getAudioClip(url, audioType, progress, callback, timeout));
        }

        public void Post(string url, WWWForm form, Action<UnityWebRequest> callback = null, int timeout = 0)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(url, form);
            uwr.timeout = timeout;

            StartCoroutine(Post(uwr, callback));
        }

        public virtual IEnumerator Post(UnityWebRequest uwr, Action<UnityWebRequest> callback = null)
        {
            yield return uwr.SendWebRequest();
            if (callback != null) { callback(uwr); }
        }

        public void Download(string url, string savePath, DelWebRequestCallback callback = null, int timeout = 0)
        {
            var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            uwr.timeout = timeout;

            StartCoroutine(Download(uwr, savePath, callback));
        }

        public virtual IEnumerator Download(UnityWebRequest uwr, string savePath, DelWebRequestCallback callback = null)
        {
            uwr.downloadHandler = new DownloadHandlerFile(savePath);
            uwr.SendWebRequest();

            while (!uwr.isDone)
            {
                if (callback != null) callback(uwr.downloadProgress, uwr);
                yield return null;
            }
        
            if (callback != null) callback(uwr.downloadProgress, uwr);
        }

        public void Upload(byte[] bytes, DelWebRequestCallback callback = null, int timeout = 0)
        {
            UnityWebRequest uwr = new UnityWebRequest();
            UploadHandler uploader = new UploadHandlerRaw(bytes);
            uploader.contentType = "application/octet-stream";

            StartCoroutine(Upload(uwr, uploader, callback));
        }

        public virtual IEnumerator Upload(UnityWebRequest uwr, UploadHandler uploader, DelWebRequestCallback callback = null)
        {
            uwr.uploadHandler = uploader;
            yield return uwr.SendWebRequest();

            while (!uwr.isDone)
            {
                if (callback != null) callback(uwr.uploadProgress, uwr);
                yield return null;
            }

            if (callback != null) callback(uwr.uploadProgress, uwr);
        }

        private IEnumerator getHeadFile(string url, Action<UnityWebRequest> callback, int timeout)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Head(url))
            {
                uwr.timeout = timeout;
                yield return uwr.SendWebRequest();
                if (callback != null) { callback(uwr); }
            }
        }

        private IEnumerator getTexture(string url, Action<float> progress, DelGetTextureCallback callback, int timeout)
        {
            UnityWebRequest uwr = new UnityWebRequest(url);
            uwr.timeout = timeout;
            DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
            uwr.downloadHandler = downloadTexture;

            uwr.SendWebRequest();

            while (!uwr.isDone)
            {
                if (progress != null) progress(uwr.downloadProgress);
                yield return null;
            }

            Texture2D texture2D = (string.IsNullOrEmpty(uwr.error)) ? downloadTexture.texture : null;
            if (callback != null) { callback(uwr.error, texture2D); }
        }

        private IEnumerator getText(string url, Action<float> progress, DelGetTextCallback callback, int timeout)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(url);
            uwr.timeout = timeout;

            uwr.SendWebRequest();
            while (!uwr.isDone)
            {
                if (progress != null) progress(uwr.downloadProgress);
                yield return null;
            }

            string text = (string.IsNullOrEmpty(uwr.error)) ? uwr.downloadHandler.text : string.Empty;
            if (callback != null) { callback(uwr.error, text); }
        }

        private IEnumerator getAssetBundle(string url, Action<float> progress, DelGetAbCallback callback, int timeout)
        {
            UnityWebRequest uwr = new UnityWebRequest(url);
            uwr.timeout = timeout;
            DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(uwr.url, uint.MaxValue);
            uwr.downloadHandler = handler;

            uwr.SendWebRequest();
            while (!uwr.isDone)
            {
                if (progress != null) progress(uwr.downloadProgress);
                yield return null;
            }

            AssetBundle ab = (string.IsNullOrEmpty(uwr.error)) ? handler.assetBundle : null;
            if (callback != null) { callback(uwr.error,ab); }
        }

        private IEnumerator getAudioClip(string url, AudioType audioType, Action<float> progress, DelGetAudioClipCallback callback, int timeout)
        {
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
            {
                uwr.timeout = timeout;

                yield return uwr.SendWebRequest();
                while (!uwr.isDone)
                {
                    if (progress != null) progress(uwr.downloadProgress);
                    yield return null;
                }

                AudioClip clip = (string.IsNullOrEmpty(uwr.error)) ? DownloadHandlerAudioClip.GetContent(uwr) : null;
                if (callback != null) callback(uwr.error, clip);
            }
        }
    }

    public delegate void DelGetTextureCallback(string error, Texture2D texture2D);
    public delegate void DelGetTextCallback(string error, string text);
    public delegate void DelGetAbCallback(string error, AssetBundle assetBundle);
    public delegate void DelGetAudioClipCallback(string error, AudioClip audioClip);
    public delegate void DelWebRequestCallback(float progress, UnityWebRequest unityWeb);

}