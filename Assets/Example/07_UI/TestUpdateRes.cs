using Mx.Res;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Mx.Example
{
    public class TestUpdateRes : MonoBehaviour
    {
        public Text textState;
        public Text textProgress;
        public Slider sliderProgress;

        private void Awake()
        {
            textState.text="正在检查资源更新...";
            UpdateResourcesFileFromServer.Instance.onDownloadResLength += onDownloadResLength;
            UpdateResourcesFileFromServer.Instance.onDownloadResCount += onDownloadResCount;
            UpdateResourcesFileFromServer.Instance.CheckResourcesUpdate(checkResourcesUpdateFinish);
        }

        /// <summary>检查资源更新完成</summary>
        private void checkResourcesUpdateFinish(string error, long updateResTotalLength, AssetInfo[] updateResArr)
        {
            if (string.IsNullOrEmpty(error))
            {
                textState.text = "正在下载资源...";

                Debug.Log(GetType() + "/checkResourcesUpdateFinish()/需要更新文件数量：" + updateResArr.Length);

                UpdateResourcesFileFromServer.Instance.DownloadResources((downloadeResError) =>
                {
                    if (string.IsNullOrEmpty(downloadeResError))
                    {
                        textState.text = "更新资源完成!";
                        Debug.Log("更新资源完成！");

                        SceneManager.LoadScene("TestUI");
                    }
                    else
                    {
                        textState.text = "更新资源发生错误! Error:"+ downloadeResError;
                        Debug.Log("更新资源发生错误！ downloadeResError:"+ downloadeResError);
                    }
                });
            }
        }

        /// <summary>下载资源进度</summary>
        private void onDownloadResLength(float downloadLength, float totalLength)
        {
            sliderProgress.value = downloadLength / totalLength;
            textProgress.text = (int)((downloadLength / totalLength)*100) +"%";
        }

        /// <summary>更新资源个数</summary>
        private void onDownloadResCount(int downloadCount, int totalCount)
        {
            textState.text = "正在下载资源 " + downloadCount + " / " + totalCount;
        }
    }
}