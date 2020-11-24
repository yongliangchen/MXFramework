using Mx.Msg;
using Mx.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Mx.UI
{
    /// <summary>UI的父类</summary>
    public abstract class BaseUIForm : MonoBehaviour
    {
        #region 封装子类常用的方法

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

        /// <summary>
        /// 打开UI窗体
        /// </summary>
        /// <param name="uiFormNames">需要打开的窗体名字</param>
        protected void OpenUIForms(params string[] uiFormNames)
        {
            UIManager.Instance.OpenUIForms(uiFormNames);
        }


        /// <summary>关闭当前UI窗体</summary>
        protected void CloseUIForm()
        {
            string[] tempStringArr = GetType().ToString().Split('.');
            string struiFormName = tempStringArr[tempStringArr.Length - 1];
            UIManager.Instance.CloseUIForms(struiFormName);
        }

        /// <summary>
        /// 关闭UI窗体
        /// </summary>
        /// <param name="uiFormNames">需要关闭的窗体名字数组</param>
        protected void CloseUIForms(params string[] uiFormNames)
        {
            UIManager.Instance.CloseUIForms(uiFormNames);
        }

        /// <summary>
        /// 发送消息给UI窗体
        /// </summary>
        /// <param name="uiFormName">接收消息的UI窗体</param>
        /// <param name="key">消息名称</param>
        /// <param name="values">消息内容</param>
        protected void SendMessageToUIForm(string uiFormName, string key, object values)
        {
            MessageCenter.SendMessage(uiFormName + "Event", key, values);
        }

        /// <summary>打开UI窗体</summary>
        public virtual void OnOpenUI()
        {

        }

        /// <summary>关闭UI窗体</summary>
        public virtual void OnCloseUI()
        {

        }

    }

    #endregion
}