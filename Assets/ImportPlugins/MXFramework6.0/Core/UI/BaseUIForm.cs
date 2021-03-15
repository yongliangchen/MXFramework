﻿using Mx.Config;
using Mx.Msg;
using Mx.Utils;
using UnityEngine;

namespace Mx.UI
{
    /// <summary>UI的父类</summary>
    public abstract class BaseUIForm : MonoBehaviour
    {
        /// <summary>当前类的名称</summary>
        private string m_CurrentClassName;

        private void Awake()
        {
            string[] tempStringArr = GetType().ToString().Split('.');
            m_CurrentClassName = tempStringArr[tempStringArr.Length - 1];
            OnAwake();

            MessageMgr.AddMsgListener(m_CurrentClassName + "Msg", OnCurrentUIFormMsgEvent);
            MessageMgr.AddMsgListener(UIDefine.GLOBAL_UI_FORM_MSG_EVENT, OnGlobalUIFormMsgEvent);
        }

        private void OnDestroy()
        {
            MessageMgr.RemoveMsgListener(m_CurrentClassName + "Msg", OnCurrentUIFormMsgEvent);
            MessageMgr.RemoveMsgListener(UIDefine.GLOBAL_UI_FORM_MSG_EVENT, OnGlobalUIFormMsgEvent);

            OnRelease();
        }

        public virtual void OnAwake() { }
        public virtual void OnRelease() { }

        /// <summary>打开UI窗体</summary>
        public virtual void OnOpenUIEvent() { }

        /// <summary>关闭UI窗体</summary>
        public virtual void OnCloseUIEvent() { }

        /// <summary>当前UI窗体消息事件监听</summary>
        public virtual void OnCurrentUIFormMsgEvent(string key, object values) { }

        /// <summary>全局UI窗体消息事件监听</summary>
        public virtual void OnGlobalUIFormMsgEvent(string key, object values) { }

        /// <summary>
        /// 注册按钮事件
        /// </summary>
        /// <param name="buttonName">按钮的名称</param>
        /// <param name="delHandle">委托的方法</param>
        protected void RigisterButtonObjectEvent(string buttonName, EventTriggerListener.VoidDelegate delHandle)
        {
            GameObject goButton = UnityHelper.FindTheChildNode(this.gameObject, buttonName).gameObject;
            if (goButton != null) { EventTriggerListener.Get(goButton).onClick = delHandle; }
            else
            {
                Debug.LogWarning(GetType() + "/RigisterButtonObjectEvent/add button event is error! button is null!  buttonName:" + buttonName);
            }
        }

        /// <summary>打开UI窗体</summary>
        protected void OpenUIForms(params string[] uiFormNames)
        {
            UIManager.Instance.OpenUIForms(uiFormNames);
        }

        /// <summary>打开UI窗体并且关闭当前UI窗体</summary>
        protected void OpenUIAndCloseCurrentUI(params string[] uiFormNames)
        {
            OpenUIForms(uiFormNames);
            CloseCurrentUIForm();
        }

        /// <summary>关闭当前UI窗体</summary>
        protected void CloseCurrentUIForm()
        {
            UIManager.Instance.CloseUIForms(m_CurrentClassName);
        }

        /// <summary>关闭UI窗体</summary>
        protected void CloseUIForms(params string[] uiFormNames)
        {
            UIManager.Instance.CloseUIForms(uiFormNames);
        }

        /// <summary>发送消息给指定UI面板</summary>
        public void SendMessageToUIForm(string key, object values, params string[] uiFormNames)
        {
            UIManager.Instance.SendMessageToUIForm(key, values, uiFormNames);
        }

        /// <summary>发送消息给全部UI面板</summary>
        public void SendGlobalUIFormMsg(string key, object values)
        {
            UIManager.Instance.SendGlobalUIFormMsg(key, values);
        }
    }
}