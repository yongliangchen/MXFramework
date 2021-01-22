using Mx.Msg;
using Mx.Utils;
using UnityEngine;

namespace Mx.UI
{
    /// <summary>UI的父类</summary>
    public abstract class BaseUIForm : MonoBehaviour
    {
        public EnumUIFormDepth uIFormsDepth { get; set; }
        public EnumUIFormShowMode uIFormShowMode { get; set; }
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
            //UIManager.Instance.OpenUIForms(uiFormNames);
        }

        /// <summary>打开UI窗体并且关闭当前UI窗体</summary>
        protected void OpenUIAndCloseCurrentUI(params string[] uiFormNames)
        {
            OpenUIForms(uiFormNames);
            CloseUIForm();
        }

        /// <summary>关闭当前UI窗体</summary>
        protected void CloseUIForm()
        {
            //UIManager.Instance.CloseUIForms(m_CurrentClassName);
        }

        /// <summary>关闭UI窗体</summary>
        protected void CloseUIForms(params string[] uiFormNames)
        {
            //UIManager.Instance.CloseUIForms(uiFormNames);
        }

        /// <summary>
        /// 发送消息给UI窗体
        /// </summary>
        /// <param name="uiFormName">接收消息的UI窗体</param>
        /// <param name="key">消息名称</param>
        /// <param name="values">消息内容</param>
        protected void SendMessageToUIForm(string uiFormName, string key, object values)
        {
            MessageCenter.SendMessage(uiFormName + "Msg", key, values);
        }

        /// <summary>
        /// 发送消息给全部UI窗体
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        protected void SendGlobalUIFormMsg(string key,object values)
        {
            MessageCenter.SendMessage(UIDefine.GLOBAL_UI_FORM_MSG_EVENT, key, values);
        }
    }
}
