using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.UI;
using Mx.Msg;
using Mx.Utils;
using UnityEngine.UI;

/// <summary> 角色信息UI面板 </summary>
public class HeroInfoUIForm : BaseUIForm
{
    private void Awake()
    {
        InitUIForm();
        RigisterButtonEvent();

        MessageMgr.AddMsgListener(UIDefine.REFRESH_UI_FORM_EVENT, OnRefreshUIFormMessagesEvent);
        MessageMgr.AddMsgListener("HeroInfoUIFormEvent",OnUIFormMessagesEvent);
    }

    private void OnDestroy()
    {
        MessageMgr.RemoveMsgListener(UIDefine.REFRESH_UI_FORM_EVENT, OnRefreshUIFormMessagesEvent);
        MessageMgr.RemoveMsgListener("HeroInfoUIFormEvent", OnUIFormMessagesEvent);
    }

    /// <summary>初始化UI界面</summary>
    private void InitUIForm()
    {

    }

    /// <summary>注册按钮事件</summary>
    private void RigisterButtonEvent()
    {
      
    }

    /// <summary>刷新UI显示</summary>
    private void OnRefreshUIForm()
    {
        InitUIForm();
    }

    /// <summary>当前UI事件监听</summary>
    private void OnUIFormMessagesEvent(string key, object values)
    {
        
    }

    /// <summary>刷新UI事件监听</summary>
    private void OnRefreshUIFormMessagesEvent(string key, object values)
    {
        if (key.Equals(UIDefine.REFRESH_UI_FORM_MSG)) OnRefreshUIForm();
    }

}

