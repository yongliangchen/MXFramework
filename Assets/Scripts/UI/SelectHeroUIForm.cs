using Mx.UI;
using UnityEngine;

/// <summary> 选择英雄UI面板 </summary>
public class SelectHeroUIForm : BaseUIForm
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
        RigisterButtonEvent("BtnConfirm", openMainUIForm);
        RigisterButtonEvent("BtnClose", openLoginUIForm);
    }

    /// <summary>打开首页UI面板</summary>
    private void openMainUIForm(GameObject btnObject)
    {
        OpenUIAndCloseCurrentUI(UIFormNames.MAIN_UIFORM, UIFormNames.HERO_INFO_UIFORM);
    }

    /// <summary>打开登入UI面板</summary>
    private void openLoginUIForm(GameObject btnObject)
    {
        OpenUIAndCloseCurrentUI(UIFormNames.LOGIN_UIFORM,UIFormNames.LOGIN_BG_UIFORM);
    }

}

