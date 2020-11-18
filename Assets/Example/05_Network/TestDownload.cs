using Mx.Net;
using UnityEngine;
using UnityEngine.UI;

namespace Mx.Example
{

    public class TestDownload : MonoBehaviour
    {
        private static string url1 = "https://hisceneapp.oss-cn-shenzhen.aliyuncs.com/pg3.zip";
        private static string url2 = "https://hisceneapp.oss-cn-shenzhen.aliyuncs.com/pg4.zip";
        private static string url3 = "https://hisceneapp.oss-cn-shenzhen.aliyuncs.com/%E6%89%93%E5%8D%A1.zip";
        private static string url4 = "https://hisceneapp.oss-cn-shenzhen.aliyuncs.com/%E9%A3%9E%E6%9C%BA%E6%A3%80%E4%BF%AE.apk";
        private static string url5 = "https://professions.oss-cn-beijing.aliyuncs.com/10003.jpg";

        private string[] urlArr = { url1, url2, url3, url4, url5 };

        private void Awake()
        {
            int i = 0;
            foreach (Transform tf in transform)
            {
                OnClick(i, tf);
                i++;
            }
        }

        private void OnClick(int i, Transform tf)
        {
            Button bt = tf.transform.Find("Button").GetComponent<Button>();
            string[] tempArr = urlArr[i].Split('/');
            string saveName = tempArr[tempArr.Length - 1];

            bt.onClick.AddListener(() =>
            {
                Download(urlArr[i], saveName, tf);
            });
        }

        private void Download(string url, string saveName, Transform tf)
        {
            Slider slider = tf.Find("Slider").GetComponent<Slider>();
            Text Info = tf.Find("Info").GetComponent<Text>();
            Info.text = null;
            Text progressText = slider.transform.Find("Handle Slide Area/Handle/Progress").GetComponent<Text>();

            string savePath = Application.persistentDataPath + "/" + saveName;

            UnityWebRequestMgr.Instance.Download(url, savePath, (progress,unityWeb) =>
             {
                 if (!unityWeb.isDone)
                 {
                     slider.value = progress;
                     progressText.text = progress.ToString("f2");
                     return;
                 }

                 if (string.IsNullOrEmpty(unityWeb.error))
                 {
                     Info.color = Color.green;
                     Info.text = "文件下载完成：" + savePath;
                     Debug.Log("文件下载完成：" + savePath);
                     slider.value = unityWeb.downloadProgress;
                     progressText.text = unityWeb.downloadProgress.ToString();

                 }
                 else
                 {
                     Info.color = Color.red;
                     Info.text = "下载文件出错：" + unityWeb.error;
                     Debug.Log("下载文件出错：" + unityWeb.error);
                 }
             }, 5);
        }

    }
}
