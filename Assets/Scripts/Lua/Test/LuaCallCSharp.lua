--[[测试Lua访问C#脚本]]

print("Hello World!")

1: Lua 中实例化一个 Unity 对象
local newGameObject = CS.UnityEngine.GameObject()
newGameObject.name="New GameObject"


--查找Unity中的对象
local findObject = CS.UnityEngine.GameObject.Find("Main Camera")

print(CS.UnityEngine.transform.name)


--[[测试Lua访问C#脚本]]

local subClass = CS.SubClass
local classObject = subClass()

classObject:ShowSubCalssInfo()            --调用子类普通函数
classObject:ShowBaseCalssInfo()			  --调用父类普通函数

print(classObject.SubCalssName)			  --调用父类普通字段
print(classObject.BaseCalssName)		  --调用父类字段


--[[测试Lua访问C#脚本，方法重载]]

local classObject = CS.MethodOverloading()
classObject:Method()
classObject:Method(10,20)
classObject:Method("Hello","World")


--[[测试Lua访问C#脚本，复杂参数]]

Student={name="张三",id=442213994,score=89.5,level=CS.EnumLevel.good}

local classObject = CS.ComplexPara()
classObject:Method(Student)


--[[测试Lua访问C#脚本，带接口参数]]
MyInterface=
{
	name="张三",
	id=1024,
	Speak=function()
		
		print("Lua中 Speak() 函数被调用")
	end

}

local classObject = CS.InterfacePara()
classObject:Method(MyInterface)


function OnEnter(prevState)
	
	print("Lua/OnEnter()/"..prevState)

end


function OnExit(nextState)
	
	print("Lua/OnExit()/"..nextState)

end

function OnUpdate()
	
	print("Lua/OnUpdate()")

end


--[测试调用委托]
TestState={

	OnEnter=function(prevState)
		
		print(prevState)

	end,

	OnEnter=function (nextState)
		
		print(nextState)

	end,

	OnUpdate=function ()

		print("Lua/OnUpdate()")
	end
}


local classObject = CS.UnityEngine.GameObject.Find("TestFiniteStateMachine");
local stateManager = classObject:GetComponent("TestStateManager")
stateManager:Register("TestState",TestState)


--[[测试Lua访问C#脚本，带委托参数]]

myDelegate=function(num)
	
	print("Lua 中对应的委托方法。num："..num)

end

local classObject=CS.DelegatePara()
classObject:Method(myDelegate)


--[[测试Lua访问C#脚本，泛型方法]]

local maxNum = CS.MyGengerric():GetMax(10,20)
print("maxNum:"..maxNum)



