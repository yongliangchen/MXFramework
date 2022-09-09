--UI管理器

local uiMgrObj = CS.Mx.UI.UIManager.Instance
UIManager = {}
local this = UIManager
local TAG = "Mx.UI"

--打开UI面板
function UIManager.OpenUIForms(...)
    uiMgrObj:OpenUIForms(...)
end

--并关闭除当前打开的其他UI面板
function UIManager.OpenUIFormsAndCloseOther(...)
    uiMgrObj:OpenUIFormsAndCloseOther(...)
end

--关闭UI面板
function UIManager.CloseUIForms(...)
    uiMgrObj:CloseUIForms(...)
end

--延迟关闭UI面板
function UIManager.CloseUIFormsDelay(time, ...)
    uiMgrObj:CloseUIFormsDelay(time, ...)
end

--关闭所有UI面板
function UIManager.CloseAllUIForms()
    uiMgrObj:CloseAllUIForms()
end

--隐藏全部打开的UI面板
function UIManager.HideOpenUIForms()
    uiMgrObj:HideOpenUIForms()
end

--隐藏除排除（exclude）外，的所有打开的UI面板
function UIManager.HideOther(...)
    uiMgrObj:HideOther(...)
end

--显示所有打开的UI面板（主要是将隐藏后的UI再次显示）
function UIManager.DisplayOpenUIForms()
    uiMgrObj:DisplayOpenUIForms()
end

--判断给定UI是否已经打开
function UIManager.IsOpen(uiFormName)
    return uiMgrObj:IsOpen(uiFormName)
end

--判断给定UI在UI管理库中是否存在
function UIManager.IsExist(uiFormName)
    return uiMgrObj:IsExist(uiFormName)
end

--发送消息给指定UI面板
function UIManager.SendMessageToUIForm(key, values, ...)
    uiMgrObj:SendMessageToUIForm(key, values, ...)
end

--发送消息给全部UI面板
function UIManager.SendGlobalUIFormMsg(key, values)
    uiMgrObj:SendGlobalUIFormMsg(key, values)
end

--注册按钮事件
function UIManager.RigisterButtonEvent(uiFormObj, buttonName, callback)
    uiMgrObj:RigisterButtonEvent(uiFormObj, buttonName, callback)
end