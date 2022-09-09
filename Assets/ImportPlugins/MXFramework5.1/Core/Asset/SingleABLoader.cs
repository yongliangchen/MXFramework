using System;
using System.Collections;
using UnityEngine;

namespace Mx.Res
{
    /// <summary>加载单个Ab资源包</summary>
    public class SingleABLoader : System.IDisposable
    {
        private AssetLoader m_AssetLoader;
        private string m_AssetBundlePath;
        private string m_AbName;

        public SingleABLoader(string abName)
        {
            m_AssetLoader = null;
            m_AbName = abName;
            m_AssetBundlePath = AssetDefine.GetABLoadPath() + "/" + abName;
        }

        /// <summary>同步加载Ab包</summary>
        public void LoadAssetBunlde()
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(m_AssetBundlePath);
            if (bundle != null) m_AssetLoader = new AssetLoader(bundle);
            else Debug.LogError(GetType() + "/LoadAssetBunlde()/ load asset bunlde error! path:" + m_AssetBundlePath);
        }

        /// <summary>
        /// 异步加载Ab包
        /// </summary>
        /// <param name="finish">加载完成回调</param>
        /// <returns></returns>
        public IEnumerator LoadAssetBunldeAsyn(Action finish= null)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(m_AssetBundlePath);
            while (!request.isDone){ yield return null; }
            yield return request;

            AssetBundle bundle = request.assetBundle;
            if (bundle != null) m_AssetLoader = new AssetLoader(bundle);
            else Debug.LogError(GetType() + "/LoadAssetBunldeAsyn()/ load asset bunlde error! path:" + m_AssetBundlePath);
            if (finish != null) finish();
        }

        /// <summary>加载资源</summary>
        public UnityEngine.Object LoadAsset(string assetName)
        {
            if (m_AssetLoader != null)
            {
                return m_AssetLoader.LoadAsset(assetName);
            }

            Debug.LogWarning(GetType() + "LoadAsset()/ load asset is null! assetName:" + assetName);
            return null;
        }

        /// <summary>卸载资源</summary>
        public void UnLoadAsset(UnityEngine.Object asset)
        {
            if (m_AssetLoader != null)
            {
                m_AssetLoader.UnLoadAsset(asset);
            }
            else
            {
                Debug.LogError(GetType() + "/UnLoadAsset()/m_AssetLoader is null!");
            }
        }

        /// <summary>释放资源</summary>
        public void Dispose()
        {

            if (m_AssetLoader != null)
            {
                m_AssetLoader.Dispose();
                m_AssetLoader = null;
            }
            else
            {
                Debug.LogError(GetType() + "/Dispose()/m_AssetLoader is null!");
            }
        }

        /// <summary>释放当前 AssetBundle 资源包，且卸载所有资源</summary>
        public void DisposeALL()
        {
            if (m_AssetLoader != null)
            {
                m_AssetLoader.DisposeALL();
                m_AssetLoader = null;
            }
            else
            {
                Debug.LogError(GetType() + "/DisposeALL()/m_AssetLoader is null!");
            }
        }

        /// <summary>查询当前AssetBundle中所有资源名称</summary>
        public string[] RetriveAllAssetName()
        {
            if (m_AssetLoader != null)
            {
                return m_AssetLoader.RetriveAllAssetName();
            }

            Debug.LogError(GetType() + "/RetriveAllAssetName()/m_AssetLoader is null!");
            return null;
        }
    }
}