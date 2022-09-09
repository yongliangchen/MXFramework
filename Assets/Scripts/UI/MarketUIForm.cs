using Mx.UI;
using UnityEngine;

/// <summary> 背包UI面板 </summary>
public class MarketUIForm : BaseUIForm
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
        RigisterButtonEvent("BtnClose", closeCurrentUIForm);
        RigisterButtonEvent("BtnMask", closeCurrentUIForm);

        //----------------------------------模拟购买装备----------------------------------
        RigisterButtonEvent("BtnClothes", (btnObject) => { openBuyUIForm(0); });
        RigisterButtonEvent("BtnTrousers", (btnObject) => { openBuyUIForm(1); });
        RigisterButtonEvent("BtnShoes", (btnObject) => { openBuyUIForm(2); });
    }

    /// <summary>关闭当前UI面板</summary>
    private void closeCurrentUIForm(GameObject btnObject)
    {
        CloseCurrentUIForm();
    }

    /// <summary>打开购买装备面板</summary>
    private void openBuyUIForm(int id)
    {
        OpenUIForms(UIFormNames.PTOP_DETAIL_UIFORM);
        SendMessageToUIForm("UpgradeEquipment", id, UIFormNames.PTOP_DETAIL_UIFORM);
    }

}
