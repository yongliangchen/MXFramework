--设置UI面板

SettingsUIForm = {}
local this = SettingsUIForm
local TAG = "SettingsUIForm"
local transform

local m_TextUserName
local m_TestUserDatas = CS.Mx.Example.TestUserDatas

--映射Unity Awake 生命周期
function this.Awake(obj)
    this.gameObject = obj
    transform = obj.transform
    this.InitView()
end

--初始化面板事件
function this.InitView()

    m_TextUserName = transform:Find("UserName"):GetComponent(Text)
    this.RefreshUserInfo()

    this.RigisterButtonEvent("BtnClose", this.OnClickCloseButton)
    this.RigisterButtonEvent("BtnMask", this.OnClickCloseButton)
    this.RigisterButtonEvent("BtnSignOut", this.OnClickSignOutButton)
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

--注册按钮事件
function this.RigisterButtonEvent(buttonName, callback)
    UIManager.RigisterButtonEvent(this.gameObject, buttonName, callback)
end

--关闭当前UI面板
function this.CloseCurrentUIForm()
    UIManager.CloseUIForms(TAG)
end

--当前UI窗体消息事件
function this.OnCurrentUIFormMsgEvent(key, values)
    print(TAG .. "/OnCurrentUIFormMsgEvent()/key:" .. key)

    if (key == "Capability") then
        this.RefreshUserInfo()
    end
end

--全局UI窗体消息事件
function this.OnGlobalUIFormMsgEvent(key, values)
    print(TAG .. "/OnGlobalUIFormMsgEvent()/key:" .. key)
end

--点击了关闭当前面板按钮
function this.OnClickCloseButton(buttonObj)
    this.CloseCurrentUIForm()
end

--点击退出登录按钮
function this.OnClickSignOutButton(buttonObj)
    UIManager.OpenUIFormsAndCloseOther(UINames.LoginBgUIForm, UINames.LoginUIForm)
end

--点击退出应用按钮
function this.OnClickQuitAppButton(buttonObj)
    UIManager.OpenUIForms(UINames.QuitAppUIForm)
end

--刷新用户信息显示
function this.RefreshUserInfo()
    local tempUserInfo = string.format("%s   战斗力：%d", m_TestUserDatas.Name, m_TestUserDatas.Capability)
    m_TextUserName.text = tempUserInfo
    print(m_TestUserDatas.Capability)
end
