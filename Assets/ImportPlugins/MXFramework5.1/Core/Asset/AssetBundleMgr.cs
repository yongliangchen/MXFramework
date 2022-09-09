using System.Collections;
using System.Collections.Generic;
using Mx.Utils;
using UnityEngine;
using System;

namespace Mx.Res
{
    /// <summary>管理整个AssetBundle (AB程序入口)</summary>
    public class AssetBundleMgr : MonoSingleton<AssetBundleMgr>
    {
        private Dictionary<string, MultiABMgr> dicAllScenes = new Dictionary<string, MultiABMgr>();
        private AssetBundleManifest m_Manifest = null;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            LoadManifest();
        }

        /// <summary>加载Manifest</summary>
        public void LoadManifest()
        {
            ABManifestLoader.Instance.LoadManifestFile();
            m_Manifest = ABManifestLoader.Instance.GetABManifest();

            if(m_Manifest==null)
            {
                Debug.LogError(GetType() + "/LoadManifest() load manifest eorro!");
            }
        }

        /// <summary>加载Manifest</summary>
        public void LoadAssetBunlde(string sceneName, string abName)
        {
            if (m_Manifest == null) return;
            MultiABMgr tmpMultiABMgr = GetMultiABMgr(sceneName, abName);
            tmpMultiABMgr.LoadAssetBunlde(abName);
        }

        /// <summary>
        /// 异步加载AssetBunde资源
        /// </summary>
        /// <param name="sceneName">场景名称（资源分组）</param>
        /// <param name="abName">资源名称(带后缀名)</param>
        /// <param name="finish">加载完成后的回调</param>
        /// <returns></returns>
        public void LoadAssetBunldeAsyn(string sceneName, string abName, Action finish)
        {
            StartCoroutine(loadAssetBunldeAsyn(sceneName, abName, finish));
        }

        private IEnumerator loadAssetBunldeAsyn(string sceneName, string abName, Action finish)
        {
            if (m_Manifest == null) yield break;

            MultiABMgr tmpMultiABMgr = GetMultiABMgr(sceneName,abName);
            yield return tmpMultiABMgr.LoadAssetBunldeAsyn(abName);

            if (finish != null) finish();
        }

        /// <summary>
        /// 获取多个Ab资源包
        /// </summary>
        /// <param name="sceneName">场景名称（资源分组）</param>
        /// <param name="abName">资源名称(带后缀名)</param>
        /// <returns></returns>
        private MultiABMgr GetMultiABMgr(string sceneName,string abName)
        {
            abName = abName.ToLower();//将名字转换成小写

            if (!dicAllScenes.ContainsKey(sceneName))
            {
                MultiABMgr multiABMgrObj = new MultiABMgr(abName);
                dicAllScenes.Add(sceneName, multiABMgrObj);
            }

            return dicAllScenes[sceneName];
        }

        /// <summary>
        /// 加载Ab包中资源
        /// </summary>
        /// <param name="sceneName">场景名称（资源分组）</param>
        /// <param name="abName">AssetBundle名称（带后缀名）</param>
        /// <param name="assetName">资源名称(带后缀名)</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string sceneName, string abName, string assetName)
        {
            //将名字转换成小写
            abName = abName.ToLower();

            if (dicAllScenes.ContainsKey(sceneName))
            {
                MultiABMgr multiABMgrObj = dicAllScenes[sceneName];
                return multiABMgrObj.LoadAsset(abName, assetName);
            }

            Debug.LogWarning(GetType() + "/LoadAsset()/找不到场景名称，无法加载（AB包）中资源！ sceneName=" + sceneName);

            return null;
        }

        /// <summary>
        /// 获取Ab包中所有资源名称
        /// </summary>
        /// <param name="sceneName">场景名称（资源分组）</param>
        /// <param name="abName">AssetBundle名称（带后缀名）</param>
        /// <returns></returns>
        public string[] RetriveAllAssetName(string sceneName, string abName)
        {
            abName = abName.ToLower();

            if (dicAllScenes.ContainsKey(sceneName))
            {
                MultiABMgr multiABMgrObj = dicAllScenes[sceneName];
                return multiABMgrObj.RetriveAllAssetName(abName);
            }

            return null;
        }

        /// <summary>
        /// 释放一个场景里面所有资源
        /// </summary>
        /// <param name="sceneName">场景名称（资源分组）</param>
        public void Dispose(string sceneName)
        {
            List<string> disposeAb = new List<string>();

            if (dicAllScenes.ContainsKey(sceneName))
            {
                MultiABMgr multiABMgrObj = dicAllScenes[sceneName];
                multiABMgrObj.DisposeAllAsset();
                dicAllScenes.Remove(sceneName);
            }
            else { Debug.LogWarning(GetType() + "/DisposeAllAssets()/找不到场景名,释放资源失败！ sceneName=" + sceneName); }
        }

        /// <summary>释放全部AssetBundle资源</summary>
        public void DisposeAllAssetBundle()
        {
            Debug.Log(GetType() + "/DisposeAllAssetBundle()/ dispose all asset");
            dicAllScenes.Clear();
            AssetBundle.UnloadAllAssetBundles(false);
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }
}