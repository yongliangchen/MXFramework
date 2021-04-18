using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mx.Net;
using Mx.Utils;
using UnityEngine;

namespace Mx.Res
{

    /// <summary>从服务器中下载更新资源</summary>
    public class UpdateResourcesFileFromServer : MonoSingleton<UpdateResourcesFileFromServer>
    {
        private Queue updateRes = new Queue();

        /// <summary>需要更新资源的总长度（单位字节）</summary>
        public long updateResTotalLength { get; private set; }
        /// <summary>已经下载的长度</summary>
        public float downloadLength { get; private set; }

        /// <summary>需要下载资源的总数量</summary>
        public int updateResTotalCount  { get; private set; }
        /// <summary>已经下载的资源个数</summary>
        public int downloadCount { get; private set; }

        /// <summary>下载资源长度事件（单位字节）</summary>
        public event DelDownloadResLength onDownloadResLength = null;
        /// <summary>下载资源数量回调</summary>
        public event DelDownloadResCount onDownloadResCount = null;

        /// <summary>
        /// 检查资源更新
        /// </summary>
        /// <param name="callback">检查资源更新回调</param>
        public void CheckResourcesUpdate(DelCheckResCallback callback)
        {
            downloadLength = 0;
            updateResTotalLength = 0;
            updateResTotalCount = 0;
            downloadCount = 0;
            updateRes.Clear();
            getServerResourcesList(PathTools.ServerURL + PathTools.GetAssetFilesListPath(), callback);
        }

        /// <summary>获取服务器资源清单</summary>
        private void getServerResourcesList(string assetFilesListURL, DelCheckResCallback callback)
        {
            WebRequest.Instance.GetText(assetFilesListURL, null, (error, text) =>
            {
                if (!string.IsNullOrEmpty(error)) { if (callback != null) callback(error, 0, null); }
                else { calculationUpdateData(text, callback); }
            }, 20);
        }

        /// <summary>计算需要更新资源数量</summary>
        private void calculationUpdateData(string text, DelCheckResCallback callback)
        {
            List<AssetInfo> tempList = new List<AssetInfo>();
            AssetList assetList = JsonUtility.FromJson<AssetList>(text);

            for (int i = 0; i < assetList.filesList.Length; i++)
            {
                AssetInfo assetInfo = assetList.filesList[i];
                string savePath = PathTools.AssetDirectory + "/" + assetInfo.directory + assetInfo.name;
                if (File.Exists(savePath))
                {
                    string localFileMd5 = StringEncrypt.GetFileMd5(savePath);
                    if (assetInfo.md5.Equals(localFileMd5)) continue;
                }

                tempList.Add(assetInfo);
                updateRes.Enqueue(assetInfo);
                updateResTotalLength += assetInfo.length;
            }

            updateResTotalCount = tempList.Count;

            if (callback != null) callback(null, updateResTotalLength, tempList.ToArray());
        }

        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="complete">下载资源完成回调</param>
        public void DownloadResources(DelDownloadResComplete complete)
        {
            float oldLength = 0;
            float nowLength = 0;

            if (updateRes == null || updateRes.Count == 0)
            {
                if (complete != null) complete(null);
                return;
            }

            AssetInfo assetInfo = (AssetInfo)updateRes.Dequeue();
            string serverResURL = PathTools.ServerURL + "Data/" + assetInfo.directory + assetInfo.name;
            string savePath = PathTools.AssetDirectory + "/" + assetInfo.directory;

            DownLoadFile.Instance.Download(serverResURL, savePath, assetInfo.md5, (progress, uwr) =>
              {
                  nowLength = assetInfo.length * progress;
                  downloadLength += nowLength - oldLength;
                  oldLength = nowLength;

                  if (onDownloadResLength != null) onDownloadResLength(downloadLength, updateResTotalLength);

                  if (!uwr.isDone) { return; }

                  if (string.IsNullOrEmpty(uwr.error))
                  {
                      downloadCount++;
                      if (onDownloadResCount != null) onDownloadResCount(downloadCount, updateResTotalCount);

                      if (updateRes.Count > 0) DownloadResources(complete);
                      else { if (complete != null) complete(null); }
                  }
                  else
                  {
                      if (complete != null) complete(uwr.error);
                  }
              });
        }
    }


    /// <summary>
    /// 检查资源更新回调
    /// </summary>
    /// <param name="error">是否发生错误</param>
    /// <param name="updateResTotalLength">需要更新资源的总大小（单位:字节）</param>
    /// <param name="updateResArr">需要更新资源的集合</param>
    public delegate void DelCheckResCallback(string error, long updateResTotalLength, AssetInfo[] updateResArr);

    /// <summary>
    /// 下载资源长度回调
    /// </summary>
    /// <param name="downloadLength">下载资源长度</param>
    /// <param name="totalLength">资源总长度</param>
    public delegate void DelDownloadResLength(float downloadLength, float totalLength);

    /// <summary>
    /// 下载资源个数回调
    /// </summary>
    /// <param name="downloadCount">下载资源个数</param>
    /// <param name="totalCount">需要下载资源个数</param>
    public delegate void DelDownloadResCount(int downloadCount, int totalCount);

    /// <summary>
    /// 下载资源结束回调
    /// </summary>
    /// <param name="error">是否发生错误</param>
    public delegate void DelDownloadResComplete(string error);

}