using System;
using Mx.Lua;
using Mx.Msg;
using UnityEngine;
using XLua;

namespace Mx.UI
{
    /// <summary></summary>
    public class BaseLuaUIForm : LuaBehaviour
    {
        [CSharpCallLua]
        public delegate void DelLuaUIFormMsgEvent(string key, object values);

        private Action<GameObject> m_LuaOpenUIEvent;
        private Action<GameObject> m_LuaCloseUIEvent;

        private DelLuaUIFormMsgEvent m_LuaCurrentUIFormMsgEvent;
        private DelLuaUIFormMsgEvent m_LuaGlobalUIFormMsgEvent;

        public override void onAwake()
        {
            base.onAwake();
            luaScriptsName = LuaDefine.MappingScriptName;

            m_LuaOpenUIEvent = luaTable.GetInPath<Action<GameObject>>(luaScriptsName + ".OnOpenUIEvent");
            m_LuaCloseUIEvent = luaTable.GetInPath<Action<GameObject>>(luaScriptsName + ".OnCloseUIEvent");
            m_LuaCurrentUIFormMsgEvent= luaTable.GetInPath<DelLuaUIFormMsgEvent>(luaScriptsName + ".OnCurrentUIFormMsgEvent");
            m_LuaGlobalUIFormMsgEvent = luaTable.GetInPath<DelLuaUIFormMsgEvent>(luaScriptsName + ".OnGlobalUIFormMsgEvent");

            MessageMgr.AddMsgListener(luaScriptsName + "Msg", OnCurrentUIFormMsgEvent);
            MessageMgr.AddMsgListener(UIDefine.GLOBAL_UI_FORM_MSG_EVENT, OnGlobalUIFormMsgEvent);
        }

        public override void onDestroy()
        {
            MessageMgr.RemoveMsgListener(luaScriptsName + "Msg", OnCurrentUIFormMsgEvent);
            MessageMgr.RemoveMsgListener(UIDefine.GLOBAL_UI_FORM_MSG_EVENT, OnGlobalUIFormMsgEvent);

            base.onDestroy();
        }

        /// <summary>UI打开事件</summary>
        public void OnOpenUIEvent()
        {
            if (m_LuaOpenUIEvent != null) m_LuaOpenUIEvent(gameObject);
        }

        /// <summary>UI关闭事件</summary>
        public void OnCloseUIEvent()
        {
            if (m_LuaCloseUIEvent != null) m_LuaCloseUIEvent(gameObject);
        }

        /// <summary>当前UI窗体消息事件监听</summary>
        public void OnCurrentUIFormMsgEvent(string key, object values)
        {
            if (m_LuaCurrentUIFormMsgEvent != null) m_LuaCurrentUIFormMsgEvent(key, values);
        }

        /// <summary>全局UI窗体消息事件监听</summary>
        public void OnGlobalUIFormMsgEvent(string key, object values)
        {
            if (m_LuaGlobalUIFormMsgEvent != null) m_LuaGlobalUIFormMsgEvent(key, values);
        }
    }
}