using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Mx.Utils
{
    /// <summary>拷贝文件</summary>
    public class CopyFiles : MonoBehaviour
    {
        private Dictionary<string, UnityWebRequest> downReqMap = new Dictionary<string, UnityWebRequest>();
        private Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();

        /// <summary>拷贝（同步的方式拷贝文件，当拷贝大文件的时候会存在卡顿问题）</summary>
        public static void Copy(string inPath, string outPath, Action<float> progress = null, Action<UnityWebRequest> callback = null)
        {
            outPath = outPath.Replace("file://", null);
            if (Application.platform != RuntimePlatform.Android) inPath = @"file://" + inPath;

            copy(inPath, outPath, progress, callback);
        }

        /// <summary>异步拷贝(适合拷贝大文件)</summary>
        public void CopyAsyn(string inPath, string savePath, Action<float> progress = null, Action<UnityWebRequest> callback = null)
        {
            savePath = savePath.Replace("file://", null);
            if (Application.platform != RuntimePlatform.Android) inPath = @"file://" + inPath;

            if (!downReqMap.ContainsKey(inPath))
            {
                coroutines.Add(inPath, StartCoroutine(copyAsyn(inPath, savePath, progress, callback)));
            }
        }

        private static void copy(string url, string outPath, Action<float> progress, Action<UnityWebRequest> callback)
        {
            var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            uwr.downloadHandler = new DownloadHandlerFile(outPath);
            uwr.SendWebRequest();

            while (!uwr.isDone)
            {
                if (progress != null) progress(uwr.downloadProgress);
            }

            if (callback != null) { callback(uwr); }
        }

        private IEnumerator copyAsyn(string url, string savePath, Action<float> progress, Action<UnityWebRequest> callback)
        {
            var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);

            uwr.downloadHandler = new DownloadHandlerFile(savePath);
            uwr.SendWebRequest();

            downReqMap.Add(url, uwr);

            while (!uwr.isDone)
            {
                yield return new WaitForEndOfFrame();
                if (progress != null) progress(uwr.downloadProgress);
                yield return null;
            }

            if (callback != null) { callback(uwr); }
            Dispose(url);
        }

        public void Dispose(string url)
        {
            if (coroutines.ContainsKey(url))
            {
                if (coroutines[url] != null) StopCoroutine(coroutines[url]);
                coroutines.Remove(url);
            }

            if (downReqMap.ContainsKey(url))
            {
                if (downReqMap[url] != null) downReqMap[url].Abort();
                if (downReqMap[url] != null) downReqMap[url].Dispose();
                downReqMap.Remove(url);
            }
        }

        public void DisposeAll()
        {
            foreach (Coroutine ct in coroutines.Values) { if (ct != null) StopCoroutine(ct); }
            coroutines.Clear();

            foreach (var item in downReqMap.Values)
            {
                if (item != null) item.Abort();
                if (item != null) item.Dispose();//释放
            }
            downReqMap.Clear();
        }

        private void OnDestroy()
        {
            DisposeAll();
        }
    }
}