--  UI根预设，控制脚本

require("UnityEngine")
require("Mx.Res")
require("Mx.Lua")


UIRootCtrl = {}
local this = UIRootCtrl
local TAG = "UIRootCtrl"

local sceneName = "UI"     --场景名称
local abName = "ui/uiprefabs.data"    --Ab包名称
local assetName = "UIRootView.prefab"      --资源名称

--得到实例
function UIRootCtrl.GetInstance()
    return this
end

--开始处理核心逻辑
function UIRootCtrl.StartProcess()
    AssetBundleMgr.LoadAssetBunldeAsyn(sceneName, abName, UIRootCtrl.ProcessComplete)
end

--处理核心逻辑完毕的回调函数
function UIRootCtrl.ProcessComplete()
    local tmpObj = AssetBundleMgr.LoadAsset(sceneName, abName, assetName)
    if (tmpObj ~= nil) then
        local closeObj = Instantiate(tmpObj)
        LuaHelper.AddBaseLuaUIForm(closeObj, "UIRootView")

        local btnStartGame = UIRootView.BtnStartGame
        btnStartGame.onClick:AddListener(this.OnClickStartGameButton)
    end
end

function UIRootCtrl.OnClickStartGameButton()

    CtrlMag.StartProcess(CtrlName.TaskCtrl)
end