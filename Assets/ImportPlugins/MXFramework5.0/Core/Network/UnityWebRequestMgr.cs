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

using UnityEngine;

namespace Mx.Net
{
    public class UnityWebRequestMgr :BaseUnityWebRequest
    {
        private static UnityWebRequestMgr instance;
        public static UnityWebRequestMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject mounter = new GameObject("UnityWebRequestMgr");
                    instance = mounter.AddComponent<UnityWebRequestMgr>();
                }
                return instance;
            }
        }

        public void CopyFile(string inPath, string outPath, DelWebRequestCallback callback = null)
        {
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

            Download(inPath, outPath, callback);
        }
    }
}