--  UI根预设，显示脚本

UIRootView = {}
local this = UIRootView
local TAG = "UIRootView"

local transform

function UIRootView.Awake(gameObject)
    transform = gameObject.transform
    this.InitView()
end

function UIRootView.InitView()
    this.BtnStartGame=transform:Find("BtnStartGame"):GetComponent("UnityEngine.UI.Button")
end
