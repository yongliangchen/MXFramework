using Mx.Res;
using UnityEngine;

namespace Mx.Example
{
    public class TsetAB : MonoBehaviour
    {
        /// <summary>同步加载资源</summary>
        public void Load()
        {
            string sceneName = "TsetAB";
            string abName = "ui/atlas.data";
            string assetName = "fuben1.png";

            AssetBundleMgr.Instance.LoadAssetBunlde(sceneName, abName);
            Object asset = AssetBundleMgr.Instance.LoadAsset(sceneName, abName, assetName);
            Debug.Log("asset:" + asset);
        }

        /// <summary>异步加载资源</summary>
        public void LoadAsyn()
        {
            string sceneName = "TsetAB";
            string abName = "ui/uiprefabs.data";
            string assetName = "HeroInfoUIForm.prefab";

            AssetBundleMgr.Instance.LoadAssetBunldeAsyn(sceneName, abName, () =>
            {
                Object asset = AssetBundleMgr.Instance.LoadAsset(sceneName, abName, assetName);
                Debug.Log("asset:" + asset);
            });
        }

        /// <summary>查看AB包中的所有资源</summary>
        public void RetriveAllAssetName()
        {
            string sceneName = "TsetAB";
            string abName = "ui/uiprefabs.data";
            string[] assetArr = AssetBundleMgr.Instance.RetriveAllAssetName(sceneName, abName);

            foreach (string asset in assetArr)
            {
                Debug.Log(asset);
            }
        }
    }
}