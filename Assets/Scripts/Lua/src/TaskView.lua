--  任务面板，显示控制脚本

TaskView={}
local this=TaskView
local TAG="TaskView"
local transform

function TaskView.Awake(obj)
    this.gameObject=obj
    transform=obj.transform
    this.InitView()
end

function TaskView.InitView()
    this.BtnClose=transform:Find("BtnClose"):GetComponent("UnityEngine.UI.Button")
end
