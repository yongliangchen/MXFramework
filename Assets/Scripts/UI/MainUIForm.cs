using Mx.UI;
using UnityEngine;

/// <summary> 首页UI面板 </summary>
public class MainUIForm : BaseUIForm
{
    public override void OnAwake()
    {
        base.OnAwake();
        rigisterButtonEvent();
    }

    public override void OnRelease()
    {
        base.OnRelease();
    }

    /// <summary>注册按钮事件</summary>
    private void rigisterButtonEvent()
    {
        RigisterButtonObjectEvent("BtnEquipment", openMarketUIForm);
        RigisterButtonObjectEvent("BtnSetting", openSettingsUIForm);
    }

    /// <summary>打开装备面板</summary>
    private void openMarketUIForm(GameObject btnObject)
    {
        OpenUIForms(UIFormNames.MARKET_UIFORM);
    }

    /// <summary>打开设置面板</summary>
    private void openSettingsUIForm(GameObject btnObject)
    {
        OpenUIForms(UIFormNames.SETTINGS_UIFORM);
    }

}

