﻿using System.Collections.Generic;
using Mx.Net;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
        public Text downloadProgress2;
        public Text downloadInfo2;
        private string outFolder;

     
        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();

            outFolder = Application.persistentDataPath + "/TestDown";
            downloadSlider2.value = 0;
            Debug.Log(GetType() + "/Awake()/outFolder:" + outFolder);
        }

        /// <summary>测试获取头文件</summary>
        public void GetHeadFile(string uri)
        {
            //headers.TryGetValue("Last-Modified", out m_ServerResVersion);//获取文件修改时间可以用来做版本号

            WebRequest.Instance.GetHeadFile(uri, (uwr) =>
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
        public void Get(string uri)
        {
            WebRequest.Instance.Get(uri, (uwr) =>
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
        public void GetTexture(string uri)
        {
            rawImage.texture = null;
            slider.value = 0;
            WebRequest.Instance.GetTexture(uri, (progress)=> { slider.value = progress; }, (error, texture2D) =>
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
        public void GetText(string uri)
        {
            WebRequest.Instance.GetText(uri,null, (error, text) =>
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
        public void GetAssetBundle(string uri)
        {
            WebRequest.Instance.GetAssetBundle(uri, null, (error, ab) =>
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
        public void GetAudioClip(string uri)
        {
            m_AudioSource.Stop();

            WebRequest.Instance.GetAudioClip(uri, AudioType.WAV, null, (error, clip) =>
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
        public void Post(string uri)
        {
            WWWForm form = new WWWForm();
            //form.AddField("请输入Key", "数输入参数");

            WebRequest.Instance.Post(uri, form,(uwr) =>
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
        public void Download1(string uri)
        {
            downloadSlider1.value = 0;
            downloadProgress1.text = "0";
            downloadInfo1.text = null;
            string savePath = outFolder;

            DownLoadFile.Instance.Download(uri, savePath, (progress, uwr) =>
            {
                if (!uwr.isDone)
                {
                    downloadSlider1.value = progress;
                    downloadProgress1.text = progress.ToString("f2");
                    return;
                }

                if (string.IsNullOrEmpty(uwr.error))
                {
                    downloadSlider1.value = progress;
                    downloadProgress1.text = progress.ToString("f2");
                    downloadInfo1.color = Color.green;
                    downloadInfo1.text = "文件下载完成：" + savePath;
                    Debug.Log("文件下载完成：" + savePath);
                }
                else
                {
                    downloadInfo1.color = Color.red;
                    downloadInfo1.text = "下载文件出错：" + uwr.error;
                    Debug.Log("下载文件出错：" + uwr.error);
                }
            });
        }

        public void Download2(string uri)
        {
            downloadSlider2.value = 0;
            downloadProgress2.text = "0";
            downloadInfo2.text = null;
            string savePath = outFolder;

            DownLoadFile.Instance.GetLastModified(uri, (version) =>
            {
                if (!string.IsNullOrEmpty(version))
                {
                    DownLoadFile.Instance.Download(uri, savePath, version, (progress, uwr) => {

                        if (!uwr.isDone)
                        {
                            downloadSlider2.value = progress;
                            downloadProgress2.text = progress.ToString("f2");
                            return;
                        }

                        if (string.IsNullOrEmpty(uwr.error))
                        {
                            downloadSlider2.value = progress;
                            downloadProgress2.text = progress.ToString("f2");
                            downloadInfo2.color = Color.green;
                            downloadInfo2.text = "文件下载完成：" + savePath;
                            Debug.Log(GetType() + "/Download()/文件下载完成！savePath:" + savePath);
                           
                            Debug.Log(File.Exists(savePath + "/pg3.zip"));
                        }
                        else
                        {
                            downloadInfo2.color = Color.red;
                            downloadInfo2.text = "下载文件出错：" + uwr.error;
                            Debug.LogWarning(GetType() + "/Download()/文件下载出错！error:" + uwr.error);
                        }
                    });
                }

                else
                {
                    downloadInfo2.color = Color.red;
                    downloadInfo2.text = "获取文件版本号错误!";
                    Debug.LogWarning(GetType() + "/Download()/获取文件版本号错误！");
                }
            });
        }

    }
}