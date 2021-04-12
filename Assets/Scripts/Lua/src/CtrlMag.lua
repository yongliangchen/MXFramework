--  Lua框架，控制层管理器
--  功能：
--      1.缓存所有项目中控制层lua脚本
--      2.提供访问项目中所有控制层lua脚本的入口函数

require("UIRootCtrl")
require("TaskCtrl")

CtrlMag = {}
local this = CtrlMag
local ctrlList = {}
local TAG = "CtrlMag"

--初始化（缓存所有项目中控制层lua脚本）
function CtrlMag.Init()
    ctrlList[CtrlName.UIRootCtrl] = UIRootCtrl.GetInstance()
    ctrlList[CtrlName.TaskCtrl] = TaskCtrl.GetInstance()
end

--获取控制器lua脚本
function CtrlMag.GetCtrlInstance(ctrlName)
    return ctrlList[ctrlName]
end

--获取控制器lua脚本，且调用StartProcess函数
function CtrlMag.StartProcess(ctrlName)
    local ctrlObj = CtrlMag.GetCtrlInstance(ctrlName)
    if (ctrlObj ~= nil) then
        ctrlObj.StartProcess()
    end
end