using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.UI;
using Mx.Msg;
using Mx.Utils;
using UnityEngine.UI;

/// <summary> 登入UI面板 </summary>
public class LogonUIForm : BaseUIForm
{
    private void Awake()
    {
        InitUIForm();
        RigisterButtonEvent();

        MessageMgr.AddMsgListener(UIDefine.REFRESH_UI_FORM_EVENT, OnRefreshUIFormMessagesEvent);
        MessageMgr.AddMsgListener("LogonUIFormEvent", OnUIFormMessagesEvent);
    }

    private void OnDestroy()
    {
        MessageMgr.RemoveMsgListener(UIDefine.REFRESH_UI_FORM_EVENT, OnRefreshUIFormMessagesEvent);
        MessageMgr.RemoveMsgListener("LogonUIFormEvent", OnUIFormMessagesEvent);
    }

    /// <summary>初始化UI界面</summary>
    private void InitUIForm()
    {

    }

    /// <summary>注册按钮事件</summary>
    private void RigisterButtonEvent()
    {
        RigisterButtonObjectEvent("BtnLogin", Login);
        RigisterButtonObjectEvent("BtnRegister", Register);
    }

    /// <summary>刷新UI显示</summary>
    private void OnRefreshUIForm()
    {
        InitUIForm();
    }

    private void Login(GameObject click)
    {
       
    }

    private void Register(GameObject click)
    {

    }

    private void OnUIFormMessagesEvent(string key, object values)
    {
        
    }

    /// <summary>刷新UI事件监听</summary>
    private void OnRefreshUIFormMessagesEvent(string key, object values)
    {
        if (key.Equals(UIDefine.REFRESH_UI_FORM_MSG)) OnRefreshUIForm();
    }
}

