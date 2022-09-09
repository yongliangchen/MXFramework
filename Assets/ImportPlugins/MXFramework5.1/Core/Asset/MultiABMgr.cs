using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mx.Res
{
    /// <summary>多个AssetBundle管理</summary>
    public class MultiABMgr
    {
        private SingleABLoader m_CurrentSingleABLoader;
        private Dictionary<string, SingleABLoader> m_DicSingleABLoaderCache;
        private string m_CurrentABName;
        private Dictionary<string, ABRelating> m_DicABRelating;

        public MultiABMgr(string abName)
        {
            m_CurrentABName = abName;
            m_DicSingleABLoaderCache = new Dictionary<string, SingleABLoader>();
            m_DicABRelating = new Dictionary<string, ABRelating>();
        }

        public IEnumerator LoadAssetBunldeAsyn(string abName)
        {
            if (!m_DicABRelating.ContainsKey(abName))
            {
                ABRelating aBRelatingObj = new ABRelating(abName);
                m_DicABRelating.Add(abName, aBRelatingObj);
            }

            ABRelating tmpABRelatingObj = m_DicABRelating[abName];

            string[] strDependeceArrar = ABManifestLoader.Instance.RetrivalDependce(abName);
            foreach (string item_Dependece in strDependeceArrar)
            {
                tmpABRelatingObj.AddDependence(item_Dependece);
                yield return LoadReferenceAsyn(item_Dependece, abName);
            }

            if (!m_DicSingleABLoaderCache.ContainsKey(abName))
            {
                m_CurrentSingleABLoader = new SingleABLoader(abName);
                m_DicSingleABLoaderCache.Add(abName, m_CurrentSingleABLoader);
                yield return m_CurrentSingleABLoader.LoadAssetBunldeAsyn();
            }
        }

        private IEnumerator LoadReferenceAsyn(string abName, string refABName)
        {
            if (m_DicABRelating.ContainsKey(abName))
            {
                ABRelating tmpABRelatingObj = m_DicABRelating[abName];
                tmpABRelatingObj.AddReference(refABName);
            }
            else
            {
                ABRelating tmpABRelatingObj = new ABRelating(abName);
                tmpABRelatingObj.AddReference(refABName);
                m_DicABRelating.Add(abName, tmpABRelatingObj);
                yield return LoadAssetBunldeAsyn(abName);
            }
        }

        public void LoadAssetBunlde(string abName)
        {
            if (!m_DicABRelating.ContainsKey(abName))
            {
                ABRelating aBRelatingObj = new ABRelating(abName);
                m_DicABRelating.Add(abName, aBRelatingObj);
            }

            ABRelating tmpABRelatingObj = m_DicABRelating[abName];

            string[] strDependeceArrar = ABManifestLoader.Instance.RetrivalDependce(abName);
            foreach (string item_Dependece in strDependeceArrar)
            {
                tmpABRelatingObj.AddDependence(item_Dependece);
                LoadReference(item_Dependece, abName);
            }

            if (!m_DicSingleABLoaderCache.ContainsKey(abName))
            {
                m_CurrentSingleABLoader = new SingleABLoader(abName);
                m_DicSingleABLoaderCache.Add(abName, m_CurrentSingleABLoader);
                m_CurrentSingleABLoader.LoadAssetBunlde();
            }
        }

        private void LoadReference(string abName, string refABName)
        {
            if (m_DicABRelating.ContainsKey(abName))
            {
                ABRelating tmpABRelatingObj = m_DicABRelating[abName];
                tmpABRelatingObj.AddReference(refABName);
            }
            else
            {
                ABRelating tmpABRelatingObj = new ABRelating(abName);
                tmpABRelatingObj.AddReference(refABName);
                m_DicABRelating.Add(abName, tmpABRelatingObj);
                LoadAssetBunlde(abName);
            }
        }

        public UnityEngine.Object LoadAsset(string abName, string assetName)
        {
            foreach (string item_abName in m_DicSingleABLoaderCache.Keys)
            {
                if (abName == item_abName)
                {
                    return m_DicSingleABLoaderCache[item_abName].LoadAsset(assetName);
                }
            }

            Debug.LogError(GetType() + "/LoadAsset()/找不到AssetBundle包，abName=" + abName + "  assetName=" + assetName);
            return null;
        }

        public string[] RetriveAllAssetName(string abName)
        {
            SingleABLoader singleABLoader;
            m_DicSingleABLoaderCache.TryGetValue(abName, out singleABLoader);
            if (singleABLoader == null) return null;
            return singleABLoader.RetriveAllAssetName();
        }

        public void DisposeAllAsset()
        {
            try
            {
                foreach (SingleABLoader item_ABLoader in m_DicSingleABLoaderCache.Values)
                {
                    item_ABLoader.DisposeALL();
                }
            }

            finally
            {
                m_DicSingleABLoaderCache.Clear();
                m_DicSingleABLoaderCache = null;

                //释放其他对象占用资源
                m_DicABRelating.Clear();
                m_DicABRelating = null;
                m_CurrentABName = null;

                //卸载没有使用的资源
                Resources.UnloadUnusedAssets();
                //垃圾回收
                System.GC.Collect();
            }
        }
    }
}
