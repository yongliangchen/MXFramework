local luaMgrObj = CS.Mx.Lua.LuaHelper.Instance

LuaHelper = {}
local this = LuaHelper
local TAG = "Mx.Lua"

--给指定对象动态添加“BaseLuaUIForm”脚本
function LuaHelper.AddBaseLuaUIForm(go,luaScriptsName)
    luaMgrObj:AddBaseLuaUIForm(go,luaScriptsName)
end

