using System.Collections;
using UnityEngine;

namespace Mx.Res
{
    /// <summary>加载资源包中的指定资源</summary>
    public class AssetLoader : System.IDisposable
    {
        private AssetBundle m_CurrentAssetBundle;
        private Hashtable m_Cache;

        public AssetLoader(AssetBundle abObj)
        {
            if (abObj != null)
            {
                m_CurrentAssetBundle = abObj;
                m_Cache = new Hashtable();
            }
            else { Debug.LogWarning(GetType() + "/AssetLoader()/ asset bundle is null!"); }
        }

        /// <summary>
        /// 加载当前包中指定资源
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="assetName">加载资源名称</param>
        /// <param name="isCache">是否需要缓存</param>
        public UnityEngine.Object LoadAsset(string assetName, bool isCache = false)
        {
            if (!m_CurrentAssetBundle.Contains(assetName))
            {
                Debug.LogWarning(GetType() + "/LoadAsset()/ load asset error! assetName:" + assetName);
                return null;
            }
            return LoadResource<UnityEngine.Object>(assetName, isCache);
        }

        /// <summary>
        /// 加载当前包中指定资源
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="assetName">加载资源名称</param>
        /// <param name="isCache">是否需要缓存</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private T LoadResource<T>(string assetName, bool isCache) where T : UnityEngine.Object
        {
            if (m_Cache.Contains(assetName))
            {
                return m_Cache[assetName] as T;
            }

            T tmpTResource = m_CurrentAssetBundle.LoadAsset<T>(assetName);

            if (tmpTResource != null && isCache)
            {
                m_Cache.Add(assetName, tmpTResource);
            }
            else if (tmpTResource == null)
            {
                Debug.LogError(GetType() + "/LoadResource<T>() tmpTResource 为空！ assetName=" + assetName);
            }

            return tmpTResource;
        }

        /// <summary>卸载指定资源</summary>
        public bool UnLoadAsset(UnityEngine.Object asset)
        {
            if (asset != null)
            {
                Resources.UnloadAsset(asset);
                return true;
            }

            Debug.LogError(GetType() + "/UnLoadAsset()/ 参数asse为空！");

            return false;
        }

        /// <summary>释放资源</summary>
        public void Dispose()
        {
            m_CurrentAssetBundle.Unload(false);
            m_Cache.Clear();
        }

        /// <summary>释放当前 AssetBundle 内存镜像资源,且释放内存资源</summary>
        public void DisposeALL()
        {
            m_CurrentAssetBundle.Unload(true);
            m_Cache.Clear();
        }

        /// <summary>查询当前 AssetBundle 包含的所有资源</summary>
        public string[] RetriveAllAssetName()
        {
            return m_CurrentAssetBundle.GetAllAssetNames();
        }

    }
}