--设置UI面板

require("UnityEngine")

SettingsUIForm = {}
local this = SettingsUIForm
local TAG = "SettingsUIForm"
local transform

function SettingsUIForm.Awake(obj)
    this.gameObject = obj
    transform = obj.transform
    this.InitView()
end

function SettingsUIForm.InitView()

    print(TAG .. "/InitView()")
end

function SettingsUIForm.Update()

    print(TAG .. "/Update()")
end
