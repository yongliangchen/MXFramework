/***
 * 
 *    Title: MXFramework
 *           主题: UnityWebRequest的基类
 *    Description: 
 *           功能：1.封装了Get请求
 *                2.封装了Pos请求
 *                3.封装了下载资源(重新下载和断点续传2种)
 *                4.封装了上传资源
 *                
 *    Date: 2020
 *    Version: v5.0版本
 *    Modify Recoder:     
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Mx.Net
{
    public abstract class BaseUnityWebRequest : MonoBehaviour
    {
        protected Dictionary<string, UnityWebRequest> downReqMap = new Dictionary<string, UnityWebRequest>();
        protected Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();

        protected IEnumerator Get(string uri, Action<UnityWebRequest> callback, int timeout)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(uri))
            {
                uwr.timeout = timeout;
                yield return uwr.SendWebRequest();
                if (callback != null) { callback(uwr); }
            }
        }

        protected IEnumerator GetHeadFile(string uri, Action<UnityWebRequest> callback, int timeout)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Head(uri))
            {
                uwr.timeout = timeout;
                yield return uwr.SendWebRequest();
                if (callback != null) { callback(uwr); }
            }
        }

        protected IEnumerator GetTexture(string uri, Action<float> progress, DelGetTextureCallback callback, int timeout)
        {
            UnityWebRequest uwr = new UnityWebRequest(uri);
            uwr.timeout = timeout;
            DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
            uwr.downloadHandler = downloadTexture;
            uwr.SendWebRequest();
            downReqMap.Add(uri, uwr);

            yield return getProgress(uwr, progress);

            Texture2D texture2D = (string.IsNullOrEmpty(uwr.error)) ? downloadTexture.texture : null;
            if (callback != null) { callback(uwr.error, texture2D); }
            Dispose(uri);
        }

        protected IEnumerator GetText(string uri, Action<float> progress, DelGetTextCallback callback, int timeout)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(uri);
            uwr.timeout = timeout;
            uwr.SendWebRequest();
            downReqMap.Add(uri, uwr);

            yield return getProgress(uwr, progress);

            string text = (string.IsNullOrEmpty(uwr.error)) ? uwr.downloadHandler.text : string.Empty;
            if (callback != null) { callback(uwr.error, text); }
            Dispose(uri);
        }

        protected IEnumerator GetAssetBundle(string uri, Action<float> progress, DelGetAbCallback callback, int timeout)
        {
            UnityWebRequest uwr = new UnityWebRequest(uri);
            uwr.timeout = timeout;
            DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(uwr.url, uint.MaxValue);
            uwr.downloadHandler = handler;
            uwr.SendWebRequest();
            downReqMap.Add(uri, uwr);

            yield return getProgress(uwr, progress);

            AssetBundle ab = (string.IsNullOrEmpty(uwr.error)) ? handler.assetBundle : null;
            if (callback != null) { callback(uwr.error,ab); }
            Dispose(uri);
        }

        protected IEnumerator GetAudioClip(string uri, AudioType audioType, Action<float> progress, DelGetAudioClipCallback callback, int timeout)
        {
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(uri, audioType))
            {
                uwr.timeout = timeout;
                uwr.SendWebRequest();

                downReqMap.Add(uri, uwr);

                yield return getProgress(uwr, progress);

                AudioClip clip = (string.IsNullOrEmpty(uwr.error)) ? DownloadHandlerAudioClip.GetContent(uwr) : null;
                if (callback != null) callback(uwr.error, clip);
                Dispose(uri);
            }
        }

        protected IEnumerator Post(UnityWebRequest uwr, Action<UnityWebRequest> callback = null)
        {
            yield return uwr.SendWebRequest();
            if (callback != null) { callback(uwr); }
        }

        protected IEnumerator Upload(string uri, byte[] bytes, DelWebRequestCallback callback, string contentType = "application/octet-stream")
        {
            UnityWebRequest uwr = new UnityWebRequest();
            UploadHandler uploader = new UploadHandlerRaw(bytes);
            uploader.contentType = contentType;
            uwr.uploadHandler = uploader;
            uwr.SendWebRequest();

            downReqMap.Add(uri, uwr);

            while (!uwr.isDone)
            {
                if (callback != null && uwr.uploadProgress < 1) callback(uwr.uploadProgress, uwr);
                yield return null;
            }
            if (callback != null) callback(uwr.uploadProgress, uwr);
            Dispose(uri);
        }

        protected IEnumerator Download(string uri, string savePath, DelWebRequestCallback callback,int timeout)
        {
            var uwr = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbGET);
            uwr.timeout = timeout;
            uwr.downloadHandler = new DownloadHandlerFile(savePath);
            uwr.SendWebRequest();

            downReqMap.Add(uri, uwr);

            while (!uwr.isDone)
            {
                if (callback != null && uwr.downloadProgress < 1) callback(uwr.downloadProgress, uwr);
                yield return null;
            }

            if (callback != null) { callback(uwr.downloadProgress, uwr); }
            Dispose(uri);
        }

        protected IEnumerator Download(string uri, string savePath, string version, DelWebRequestCallback callback, int timeout)
        {
            string fileName = uri.Split('/')[uri.Split('/').Length - 1];

            UnityWebRequest uwr = UnityWebRequest.Get(uri);
            uwr.timeout = timeout;
            DownloadFileHandler downloadFile = new DownloadFileHandler(savePath, fileName, version);
            uwr.downloadHandler = downloadFile;
            long length = downloadFile.NowLength;
            uwr.SetRequestHeader("Range", "bytes=" + length + "-");
            uwr.SendWebRequest();

            downReqMap.Add(uri, uwr);

            while (!uwr.isDone)
            {
                yield return new WaitForEndOfFrame();
                if (callback != null && downloadFile.DownloadProgress < 1) callback(downloadFile.DownloadProgress, uwr);
                yield return null;
            }

            if (callback != null) {callback(downloadFile.DownloadProgress, uwr); }
            Dispose(uri);
        }

        private IEnumerator getProgress(UnityWebRequest uwr, Action<float> progress)
        {
            while (!uwr.isDone)
            {
                yield return new WaitForEndOfFrame();
                if (progress != null && uwr.downloadProgress < 1) progress(uwr.downloadProgress);
                yield return null;
            }
        }

        public virtual void Dispose(string uri)
        {
            if (coroutines.ContainsKey(uri))
            {
                if (coroutines[uri] != null) StopCoroutine(coroutines[uri]);
                coroutines.Remove(uri);
            }

            if (downReqMap.ContainsKey(uri))
            {
                if (downReqMap[uri] != null) downReqMap[uri].Abort();
                if (downReqMap[uri] != null) downReqMap[uri].Dispose();
                downReqMap.Remove(uri);
            }
        }

        public virtual void DisposeAll()
        {
            foreach (Coroutine ct in coroutines.Values) { if (ct != null) StopCoroutine(ct); }
            coroutines.Clear();

            foreach (var item in downReqMap.Values)
            {
                if (item != null) item.Abort();//中止下载  
                if (item != null) item.Dispose();  //释放
            }
            downReqMap.Clear();
        }

        private void OnDestroy()
        {
            DisposeAll();
        }
    }
}