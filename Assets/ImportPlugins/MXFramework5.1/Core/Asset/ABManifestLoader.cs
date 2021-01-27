using UnityEngine;

namespace Mx.Res
{
    /// <summary>管理Manifest文件加载</summary>
    public class ABManifestLoader : System.IDisposable
    {
        private static ABManifestLoader m_Instance;
        private AssetBundleManifest m_ManifestObj;
        private string m_ManifestPath;
        private AssetBundle m_ABReadManifest;

        private ABManifestLoader()
        {
            m_ManifestPath = AssetDefine.GetManifestPath();
            m_ManifestObj = null;
            m_ABReadManifest = null;
        }

        /// <summary>单例类</summary>
        public static ABManifestLoader Instance
        {
            get
            {
                if (m_Instance == null){m_Instance = new ABManifestLoader();}
                return m_Instance;
            }
        }

        /// <summary>
        /// 加载 Manifest 文件
        /// </summary>
        public void LoadManifestFile()
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(m_ManifestPath);
            if (bundle != null)
            {
                m_ABReadManifest = bundle;
                m_ManifestObj = m_ABReadManifest.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            }
            else Debug.LogError(GetType() + "/LoadManifestFile()/ load manifest error! path:" + m_ManifestPath);
        }

        /// <summary>
        /// 获取Manifest文件
        /// </summary>
        /// <returns>The ABM anifest.</returns>
        public AssetBundleManifest GetABManifest()
        {
            if (m_ManifestObj != null)
            {
                return m_ManifestObj;
            }
            else
            {
                Debug.Log(GetType() + "/GetABManifest()/ m_ManifestObj==Null");
            }

            return null;
        }

        /// <summary>
        /// 获取AssetBundleManifest(系统类)所有依赖项
        /// </summary>
        /// <returns>The dependce.</returns>
        /// <param name="abName">Ab name.</param>
        public string[] RetrivalDependce(string abName)
        {

            if (m_ManifestObj != null && !string.IsNullOrEmpty(abName))
            {
                return m_ManifestObj.GetAllDependencies(abName);
            }

            return null;
        }

        /// <summary>
        /// 资源卸载
        /// </summary>
        public void Dispose()
        {
            if (m_ABReadManifest != null)
            {
                m_ABReadManifest.Unload(true);
            }
        }

    }
}

