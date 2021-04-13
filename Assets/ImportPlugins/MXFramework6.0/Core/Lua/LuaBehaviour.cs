/***
 * 
 *    Title: MXFramework
 *           主题: 映射lua脚本生命周期
 *    Description: 
 *           功能：1.将Unity声明周期映射到lua脚本上                        
 *           
 *           
 *    Date: 2021
 *    Version: v6.0版本
 *    Modify Recoder:      
 *
 */

using System;
using UnityEngine;
using XLua;

namespace Mx.Lua
{
    /// <summary>映射lua脚本生命周期</summary>
    public class LuaBehaviour : MonoBehaviour
    {
        /// <summary>Lua的运行环境</summary>
        public LuaEnv luaEnv { get; set; }
        public LuaTable luaTable { get; set; }

        /// <summary>需要映射的Lua脚本名称</summary>
        public string luaScriptsName { get;set; }

        private Action<GameObject> m_LuaAwake;
        private Action<GameObject> m_LuaOnEnable;
        private Action<GameObject> m_LuaStart;
        private Action<GameObject> m_LuaFixedUpdate;
        private Action<GameObject> m_LuaUpdate;
        private Action<GameObject> m_LuaLateUpdate;
        private Action<GameObject> m_LuaOnDisable;
        private Action<GameObject> m_LuaOnDestroy;

        private void Awake()
        {
            setMetaTable();
            onAwake();
            init();
        }

        private void OnEnable()
        {
            if (m_LuaOnEnable != null) m_LuaOnEnable(gameObject);
        }

        private void Start()
        {
            if (m_LuaStart != null) m_LuaStart(gameObject);

            onStart();
        }

        private void FixedUpdate()
        {
            if (m_LuaFixedUpdate != null) m_LuaFixedUpdate(gameObject);
        }

        private void Update()
        {
            if (m_LuaUpdate != null) m_LuaUpdate(gameObject);

            onUpdate();
        }

        private void LateUpdate()
        {
            if (m_LuaLateUpdate != null) m_LuaLateUpdate(gameObject);
        }

        private void OnDisable()
        {
            if (m_LuaOnDisable != null) m_LuaOnDisable(gameObject);
        }

        private void OnDestroy()
        {
            onDestroy();

            if (m_LuaOnDestroy != null) m_LuaOnDestroy(gameObject);
        }

        public virtual void onAwake(){}
        public virtual void onStart(){}
        public virtual void onUpdate(){}
        public virtual void onDestroy() { }

        /// <summary>设置元表</summary>
        private void setMetaTable()
        {
            luaEnv = LuaHelper.Instance.GetLuaEnv();
            luaTable = luaEnv.NewTable();
            LuaTable tmpTab = luaEnv.NewTable();
            tmpTab.Set("__index", luaEnv.Global);
            luaTable.SetMetaTable(tmpTab);
            tmpTab.Dispose();
        }

        /// <summary>初始化</summary>
        private void init()
        {
            m_LuaAwake = luaTable.GetInPath<Action<GameObject>>(luaScriptsName + ".Awake");
            m_LuaOnEnable = luaTable.GetInPath<Action<GameObject>>(luaScriptsName + ".OnEnable");
            m_LuaStart = luaTable.GetInPath<Action<GameObject>>(luaScriptsName + ".Start");
            m_LuaFixedUpdate = luaTable.GetInPath<Action<GameObject>>(luaScriptsName + ".FixedUpdate");
            m_LuaUpdate = luaTable.GetInPath<Action<GameObject>>(luaScriptsName + ".Update");
            m_LuaLateUpdate = luaTable.GetInPath<Action<GameObject>>(luaScriptsName + ".LateUpdate");
            m_LuaOnDisable = luaTable.GetInPath<Action<GameObject>>(luaScriptsName + ".OnDisable");
            m_LuaOnDestroy = luaTable.GetInPath<Action<GameObject>>(luaScriptsName + ".OnDestroy");

            if (m_LuaAwake != null) m_LuaAwake(gameObject);
        }
    }
}