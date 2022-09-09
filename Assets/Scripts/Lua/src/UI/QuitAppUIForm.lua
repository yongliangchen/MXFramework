--退出应用UI面板

QuitAppUIForm = {}
local this = QuitAppUIForm
local TAG = "QuitAppUIForm"
local transform

--映射Unity Awake 生命周期
function this.Awake(obj)
    this.gameObject = obj
    transform = obj.transform
    this.InitView()
end

--初始化面板事件
function this.InitView()
    this.RigisterButtonEvent("BtnMask", this.OnClickCloseButton)
    this.RigisterButtonEvent("BtnClose", this.OnClickCloseButton)
    this.RigisterButtonEvent("BtnQuitGame", this.OnClickQuitAppButton)
end

--映射Unity Update 生命周期
function this.Update()

end

--UI面板打开事件
function this.OnOpenUIEvent()

end

--UI面板关闭事件
function this.OnCloseUIEvent()

end

--当前UI窗体消息事件
function this.OnCurrentUIFormMsgEvent(key, values)

end

--全局UI窗体消息事件
function this.OnGlobalUIFormMsgEvent(key, values)

end

--注册按钮事件
function this.RigisterButtonEvent(buttonName, callback)
    UIManager.RigisterButtonEvent(this.gameObject, buttonName, callback)
end

--关闭当前UI面板
function this.CloseCurrentUIForm()
    UIManager.CloseUIForms(TAG)
end

--点击了关闭当前面板按钮
function this.OnClickCloseButton(buttonObj)
    this.CloseCurrentUIForm()
end

--点击退出应用按钮
function this.OnClickQuitAppButton(buttonObj)
    Application.Quit()
end
