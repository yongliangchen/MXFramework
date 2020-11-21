using System;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

namespace Mx.Utils
{
    /// <summary>拷贝文件</summary>
    public class CopyFiles : MonoBehaviour
    {
        /// <summary>拷贝（同步的方式拷贝文件，当拷贝大文件的时候会存在卡顿问题）</summary>
        public static void Copy(string inPath, string outPath, Action<float> progress=null, Action<UnityWebRequest> actionResult=null)
        {
            if(!File.Exists(inPath))
            {
                Debug.LogWarning("CopyFiles/Copy()/需要拷贝的文件为空！inPath：" + inPath);
                return;
            }

            outPath = outPath.Replace("file://", null);

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.IPhonePlayer:

                    inPath = @"file://" + inPath;
                    break;
            }

            copy(inPath, outPath, progress, actionResult);
        }

        private static void copy(string url, string downloadFilePathAndName, Action<float> progress, Action<UnityWebRequest> actionResult)
        {
            var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);

            uwr.downloadHandler = new DownloadHandlerFile(downloadFilePathAndName);
            uwr.SendWebRequest();

            while (!uwr.isDone)
            {
                if (progress != null) progress(uwr.downloadProgress);
            }

            if (actionResult != null) { actionResult(uwr); }
        }
    }
}