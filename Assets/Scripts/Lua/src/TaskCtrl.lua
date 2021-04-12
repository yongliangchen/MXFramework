--  任务面板，控制层脚本

require("UnityEngine")
require("Mx.Res")
require("Mx.Lua")

TaskCtrl = {}
local this = TaskCtrl
local TAG = "TaskCtrl"

local sceneName = "UI"     --场景名称
local abName = "ui/uiprefabs.data"    --Ab包名称
local assetName = "TaskView.prefab"      --资源名称

--得到实例
function TaskCtrl.GetInstance()
    return this
end

--开始处理核心逻辑
function TaskCtrl.StartProcess()

    local tmpObj = AssetBundleMgr.LoadAsset(sceneName, abName, assetName)
    if (tmpObj ~= nil) then
        local closeObj = Instantiate(tmpObj)
        LuaHelper.AddBaseLuaUIForm(closeObj, "TaskView")
    end

    local btnClose = TaskView.BtnClose
    btnClose.onClick:AddListener(this.OnClickCloseButton)

end

function TaskCtrl.OnClickCloseButton()
    Destroy(TaskView.gameObject)
end