--  Lua框架，项目初始化
--  功能：
--      1.引入项目中所有的视图层脚本
--      2.缓存系统中所有的控制层脚本
--      3.提供访问其他控制层的入口函数
--      4.调用项目中第一个UI预设控制层脚本

--引入控制层管理脚本
require("CtrlMag")

ProjectInit = {}
local this = ProjectInit
local TAG = "ProjectInit"

--初始化
function ProjectInit.Init()
    print(TAG .. ("/Init()"))

    --引入项目中所有视图控制脚本
    this.ImportAllViews()
    --Lua控制器初始化
    CtrlMag.Init()
    --加载UI'根窗体'控制脚本
    CtrlMag.StartProcess(CtrlName.UIRootCtrl)
end

--引入项目中所有视图控制脚本
function ProjectInit.ImportAllViews()
    for i = 1, #ViewNames do
        require(tostring(ViewNames[i]))
    end
end