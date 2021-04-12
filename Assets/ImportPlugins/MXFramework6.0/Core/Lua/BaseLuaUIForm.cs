using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using Mx.Lua;

public class BaseLuaUIForm : MonoBehaviour
{
    /// <summary>需要映射的Lua脚本名称</summary>
    public string luaScriptsName { get; private set; }

    [CSharpCallLua]
    public delegate void delLuaAwake(GameObject go);
    BaseLuaUIForm.delLuaAwake luaAwake;

    [CSharpCallLua]
    public delegate void delLuaOnEnable(GameObject go);
    BaseLuaUIForm.delLuaOnEnable luaOnEnable;

    [CSharpCallLua]
    public delegate void delLuaStart(GameObject go);
    BaseLuaUIForm.delLuaStart luaStart;

    [CSharpCallLua]
    public delegate void delLuaFixedUpdate(GameObject go);
    BaseLuaUIForm.delLuaFixedUpdate luaFixedUpdate;

    [CSharpCallLua]
    public delegate void delLuaUpdate(GameObject go);
    BaseLuaUIForm.delLuaUpdate luaUpdate;

    [CSharpCallLua]
    public delegate void delLuaLateUpdate(GameObject go);
    BaseLuaUIForm.delLuaLateUpdate luaLateUpdate;

    [CSharpCallLua]
    public delegate void delLuaOnDisable(GameObject go);
    BaseLuaUIForm.delLuaOnDisable luaOnDisable;

    [CSharpCallLua]
    public delegate void delLuaOnDestroy(GameObject go);
    BaseLuaUIForm.delLuaOnDestroy luaOnDestroy;

    private LuaTable m_LuaTable;
    private LuaEnv m_LuaEnv;


    private void Awake()
    {
        
        luaScriptsName= LuaDefine.MappingScriptName;

        m_LuaEnv = LuaHelper.Instance.GetLuaEnv();
        m_LuaTable = m_LuaEnv.NewTable();
        LuaTable tmpTab = m_LuaEnv.NewTable();
        tmpTab.Set("__index", m_LuaEnv.Global);
        m_LuaTable.SetMetaTable(tmpTab);
        tmpTab.Dispose();

        luaAwake= m_LuaTable.GetInPath<BaseLuaUIForm.delLuaAwake>(luaScriptsName + ".Awake");
        luaOnEnable = m_LuaTable.GetInPath<BaseLuaUIForm.delLuaOnEnable>(luaScriptsName + ".OnEnable");
        luaStart = m_LuaTable.GetInPath<BaseLuaUIForm.delLuaStart>(luaScriptsName + ".Start");
        luaFixedUpdate = m_LuaTable.GetInPath<BaseLuaUIForm.delLuaFixedUpdate>(luaScriptsName + ".FixedUpdate");
        luaUpdate = m_LuaTable.GetInPath<BaseLuaUIForm.delLuaUpdate>(luaScriptsName + ".Update");
        luaLateUpdate = m_LuaTable.GetInPath<BaseLuaUIForm.delLuaLateUpdate>(luaScriptsName + ".LateUpdate");
        luaOnDisable = m_LuaTable.GetInPath<BaseLuaUIForm.delLuaOnDisable>(luaScriptsName + ".OnDisable");
        luaOnDestroy = m_LuaTable.GetInPath<BaseLuaUIForm.delLuaOnDestroy>(luaScriptsName + ".OnDestroy");

        if (luaAwake != null) luaAwake(gameObject);
    }

    private void OnEnable()
    {
        if (luaOnEnable != null) luaOnEnable(gameObject);
    }

    private void Start()
    {
        if (luaStart != null) luaStart(gameObject);
    }

    private void FixedUpdate()
    {
        if (luaFixedUpdate != null) luaFixedUpdate(gameObject);
    }

    private void Update()
    {
        if (luaUpdate != null) luaUpdate(gameObject);
    }

    private void LateUpdate()
    {
        if (luaLateUpdate != null) luaLateUpdate(gameObject);
    }

    private void OnDisable()
    {
        if (luaOnDisable != null) luaOnDisable(gameObject);
    }

    private void OnDestroy()
    {
        if (luaOnDestroy != null) luaOnDestroy(gameObject);
    }

}
