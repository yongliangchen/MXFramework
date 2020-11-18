using UnityEngine;
using Mx.Net;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Mx.Example
{
    public class TestUnityWebRequest : MonoBehaviour
    {
        //byte[] bytes = o.GetByteArray("b_1").Bytes;//资源
        //Texture2D texture = new Texture2D(width, height);
        //texture.LoadImage(bytes);

        public RawImage rawImage;
        public Slider slider;

        /// <summary>测试获取头文件</summary>
        public void GetHeadFile(string url)
        {
            //headers.TryGetValue("Last-Modified", out m_ServerResVersion);//获取文件修改时间可以用来做版本号

            UnityWebRequestMgr.Instance.GetHeadFile(url, (uwr) =>
            {
                if(string.IsNullOrEmpty(uwr.error))
                {
                    Dictionary<string, string> headers = uwr.GetResponseHeaders();
                    foreach(string key in headers.Keys)
                    {
                        Debug.Log(key+" : "+ headers[key]);
                    }
                }
                else
                {
                    Debug.LogWarning(GetType() + "/GetHeadFile()/ getHeadFile error!" + uwr.error);
                }

            }, 10);
        }
   
        /// <summary>测试Get方法</summary>
        public void Get(string url)
        {
            UnityWebRequestMgr.Instance.Get(url, (uwr) =>
            {
                if (string.IsNullOrEmpty(uwr.error))
                {
                    Debug.Log(uwr.responseCode);
                }
                else
                {
                    Debug.LogWarning(GetType() + "/GetHeadFile()/ getHeadFile error!" + uwr.error);
                }

            }, 10);
        }

        /// <summary>测试下载图片</summary>
        public void GetTexture(string url)
        {
            rawImage.texture = null;
            slider.value = 0;
            UnityWebRequestMgr.Instance.GetTexture(url, (progress)=> { slider.value = progress; }, (error, texture2D) =>
             {
                 if (string.IsNullOrEmpty(error))
                 {
                     slider.value = 1;
                     rawImage.texture = texture2D;
                 }
                 else
                 {
                     Debug.LogWarning(GetType() + "/GetHeadFile()/ getHeadFile error!" + error);
                 }

             }, 20);
        }

    }
}