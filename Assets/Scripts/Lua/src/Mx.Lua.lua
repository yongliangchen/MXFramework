local abMgrObj = CS.Mx.Lua.LuaHelper.Instance

LuaHelper = {}
local this = LuaHelper
local TAG = "LuaHelper"

--给指定对象动态添加“BaseLuaUIForm”脚本
function LuaHelper.AddBaseLuaUIForm(go,luaScriptsName)
    abMgrObj:AddBaseLuaUIForm(go,luaScriptsName)
end

