local abMgrObj = CS.Mx.Res.AssetBundleMgr.Instance
AssetBundleMgr = {}
local this = AssetBundleMgr

--加载Manifest
function AssetBundleMgr.LoadManifest()
    abMgrObj:LoadManifest()
end

--同步加载AssetBunde资源
function AssetBundleMgr.LoadAssetBunlde(sceneName, abName)
    abMgrObj:LoadAssetBunlde(sceneName, abName)
end

--异步加载AssetBunde资源
function AssetBundleMgr.LoadAssetBunldeAsyn(sceneName, abName, finish)
    abMgrObj:LoadAssetBunldeAsyn(sceneName, abName, finish)
end

--加载Ab包中资源
function AssetBundleMgr.LoadAsset(sceneName, abName, assetName)
    return abMgrObj:LoadAsset(sceneName, abName, assetName)
end

--获取Ab包中所有资源名称
function AssetBundleMgr.RetriveAllAssetName(sceneName, abName)
    return abMgrObj:RetriveAllAssetName(sceneName, abName)
end

--释放一个场景里面所有资源
function AssetBundleMgr.Dispose(sceneName)
    abMgrObj:Dispose(sceneName)
end

--释放全部AssetBundle资源
function AssetBundleMgr.DisposeAllAssetBundle()
    abMgrObj:DisposeAllAssetBundle()
end

