using System.Collections.Generic;
using Mx.Net;
using Mx.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Mx.Example
{
    public class TestUnityWebRequest : MonoBehaviour
    {
        public RawImage rawImage;
        public Slider slider;

        private AudioSource m_AudioSource;

        public Slider downloadSlider1;
        public Text downloadProgress1;
        public Text downloadInfo1;

        public Slider downloadSlider2;
        private string outFolder;

     
        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();

            outFolder = Application.persistentDataPath + "/TestDown";
            downloadSlider2.value = 0;
            Debug.Log(GetType() + "/Awake()/outFolder:" + outFolder);
        }

        /// <summary>测试获取头文件</summary>
        public void GetHeadFile(string url)
        {
            //headers.TryGetValue("Last-Modified", out m_ServerResVersion);//获取文件修改时间可以用来做版本号

            WebRequest.Instance.GetHeadFile(url, (uwr) =>
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
                    Debug.LogWarning(GetType() + "/GetHeadFile()/error!" + uwr.error);
                }

            }, 10);
        }

        /// <summary>测试Get方法</summary>
        public void Get(string url)
        {
            WebRequest.Instance.Get(url, (uwr) =>
            {
                if (string.IsNullOrEmpty(uwr.error))
                {
                    Debug.Log(uwr.downloadHandler.text);
                }
                else
                {
                    Debug.LogWarning(GetType() + "/Get()/error!" + uwr.error);
                }

            }, 20);
        }

        /// <summary>测试下载图片</summary>
        public void GetTexture(string url)
        {
            rawImage.texture = null;
            slider.value = 0;
            WebRequest.Instance.GetTexture(url, (progress)=> { slider.value = progress; }, (error, texture2D) =>
             {
                 if (string.IsNullOrEmpty(error))
                 {
                     Debug.Log("width:"+texture2D.width + "  height:" + texture2D.height);
                     slider.value = 1;
                     rawImage.texture = texture2D;
                 }
                 else
                 {
                     Debug.LogWarning(GetType() + "/GetTexture()/error!" + error);
                 }

             }, 20);
        }
        
        /// <summary>测试获取Text</summary>
        public void GetText(string url)
        {
            WebRequest.Instance.GetText(url,null, (error, text) =>
            {
                if (string.IsNullOrEmpty(error))
                {
                    Debug.Log(text);
                }
                else
                {
                    Debug.LogWarning(GetType() + "/GetText()/error!" + error);
                }

            }, 20);
        }
        
        /// <summary>测试获取AB</summary>
        public void GetAssetBundle(string url)
        {
            WebRequest.Instance.GetAssetBundle(url, null, (error, ab) =>
            {
                if (string.IsNullOrEmpty(error))
                {
                    Debug.Log("获取Ab成功！");
                    //Debug.Log(text);
                }
                else
                {
                    Debug.LogWarning(GetType() + "/GetAssetBundle()/error!" + error);
                }

            }, 20);
        }

        /// <summary>测试获取Clip(</summary>
        public void GetAudioClip(string url)
        {
            m_AudioSource.Stop();

            WebRequest.Instance.GetAudioClip(url, AudioType.WAV, null, (error, clip) =>
            {
                if (string.IsNullOrEmpty(error))
                {
                    Debug.Log("获取Clip成功！");
                    m_AudioSource.clip = clip;
                    m_AudioSource.Play();
                }
                else
                {
                    Debug.LogWarning(GetType() + "/GetAssetBundle()/error!" + error);
                }

            }, 20);
        }

        /// <summary>测试Post</summary>
        public void Post(string url)
        {
            WWWForm form = new WWWForm();
            //form.AddField("请输入Key", "数输入参数");

            WebRequest.Instance.Post(url, form,(uwr) =>
            {
                if (string.IsNullOrEmpty(uwr.error))
                {
                    Debug.Log("Post成功！");
                }
                else
                {
                    Debug.LogWarning(GetType() + "/Post()/error!" + uwr.error);
                }

            }, 20);
        }

        /// <summary>测试下载文件(没有断点续传)</summary>
        public void Download1(string url)
        {
            downloadSlider1.value = 0;
            downloadProgress1.text = "0";
            downloadInfo1.text = null;
            string[] tempArr = url.Split('/');
            string saveName = tempArr[tempArr.Length - 1];
            string savePath = Application.persistentDataPath + "/" + saveName;

            DownLoadFile.Instance.Download(url, savePath, (progress, uwr) =>
            {
                if (!uwr.isDone)
                {
                    downloadSlider1.value = progress;
                    downloadProgress1.text = progress.ToString("f2");
                    return;
                }

                if (string.IsNullOrEmpty(uwr.error))
                {
                    downloadInfo1.color = Color.green;
                    downloadInfo1.text = "文件下载完成：" + savePath;
                    Debug.Log("文件下载完成：" + savePath);
                    downloadSlider1.value = uwr.downloadProgress;
                    downloadProgress1.text = uwr.downloadProgress.ToString();

                }
                else
                {
                    downloadInfo1.color = Color.red;
                    downloadInfo1.text = "下载文件出错：" + uwr.error;
                    Debug.Log("下载文件出错：" + uwr.error);
                }
            }, 20);
        }

        public void Download2(string url)
        {
            downloadSlider2.value = 0;
            string savePath = outFolder;

            DownLoadFile.Instance.GetLastModified(url, (version) =>
            {
                if (!string.IsNullOrEmpty(version))
                {
                    DownLoadFile.Instance.Download(url, savePath, version, (progress, uwr) => {
                        downloadSlider2.value = progress;
                        if (!uwr.isDone) return;

                        if (string.IsNullOrEmpty(uwr.error))
                        {
                            Debug.Log(GetType() + "/Download()/文件下载完成！savePath:" + savePath);
                        }
                        else
                        {
                            Debug.LogWarning(GetType() + "/Download()/文件下载出错！error:" + uwr.error);
                        }
                    });
                }

                else Debug.LogWarning(GetType() + "/Download()/获取文件版本号错误！");
            });
        }

    }
}