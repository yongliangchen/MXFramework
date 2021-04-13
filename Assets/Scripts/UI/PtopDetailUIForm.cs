using System.Collections.Generic;
using Mx.UI;
using UnityEngine;
using UnityEngine.UI;
using Mx.Utils;
using Mx.Res;
using Mx.Example;

/// <summary> 弹出框UI面板 </summary>
public class PtopDetailUIForm : BaseUIForm
{
    private List<EquipmentInfo> m_EquipmentInfosList = new List<EquipmentInfo>();
    private EquipmentInfo m_EquipmentInfo = null;

    private RawImage m_IconRawImage;
    private Text m_NameText;
    private Text m_DesText;
    private Text M_CoinText;

    public override void OnAwake()
    {
        base.OnAwake();

        m_IconRawImage = UnityHelper.FindTheChildNode(gameObject, "Icon").GetComponent<RawImage>();
        m_NameText = UnityHelper.FindTheChildNode(gameObject, "Name").GetComponent<Text>();
        m_DesText = UnityHelper.FindTheChildNode(gameObject, "Des").GetComponent<Text>();
        M_CoinText = UnityHelper.FindTheChildNode(gameObject, "Coin").GetComponent<Text>();

        //--------------------------------------------模拟装备数据库--------------------------------------------
        EquipmentInfo clothesInfo = new EquipmentInfo { Level = 1,Name= "普通布衣", Base=5, Coin=100, Icon= "1LVclothes" };
        EquipmentInfo trousersInfo = new EquipmentInfo { Level = 1, Name= "粗布麻库", Base =8, Coin = 200, Icon = "1LVpants" };
        EquipmentInfo shoesInfo = new EquipmentInfo { Level = 1, Name="敏捷鞋子", Base = 12, Coin = 300, Icon = "1LVshoes" };

        m_EquipmentInfosList.Add(clothesInfo);
        m_EquipmentInfosList.Add(trousersInfo);
        m_EquipmentInfosList.Add(shoesInfo);

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
        RigisterButtonEvent("BtnUpgrade", upgrade);
    }

    /// <summary>关闭当前UI面板</summary>
    private void closeCurrentUIForm(GameObject btnObject)
    {
        CloseCurrentUIForm();
    }

    public override void OnCloseUIEvent()
    {
        base.OnCloseUIEvent();

        m_EquipmentInfo = null;
    }

    /// <summary>升级武器</summary>
    private void upgrade(GameObject btnObject)
    {
        int cost = m_EquipmentInfo.Coin * m_EquipmentInfo.Level;

        //升级成功
        if(TestUserDatas.Coin>= cost)
        {
            TestUserDatas.Coin -= cost;
            m_EquipmentInfo.Level++;
            setPanelInfo();
        }
        //升级失败
        else
        {
            Toast.Show("金币不足，升级失败！", new Vector3(0, 100, 0));
        }
    }

    public override void OnCurrentUIFormMsgEvent(string key, object values)
    {
        base.OnCurrentUIFormMsgEvent(key, values);

        if (key.Equals("UpgradeEquipment"))
        {
            m_EquipmentInfo = m_EquipmentInfosList[(int)values];
            setPanelInfo();
        }
    }

    /// <summary>设置购买面板信息</summary>
    private void setPanelInfo()
    {
        Texture2D texture2D = ResoucesMgr.Instance.Load<Texture2D>("Packages/" + m_EquipmentInfo.Icon, true);
        m_IconRawImage.texture = texture2D;
        m_NameText.text = m_EquipmentInfo.Level + "级" + m_EquipmentInfo.Name;
        m_DesText.text = "战斗力+" + m_EquipmentInfo.Level * m_EquipmentInfo.Base;
        M_CoinText.text = m_EquipmentInfo.Coin* m_EquipmentInfo.Level + " 金币";

        refreshCapability();
    }

    /// <summary>刷新战斗力</summary>
    private void refreshCapability()
    {
        int capability = 0;

        for(int i=0;i< m_EquipmentInfosList.Count;i++)
        {
            capability += (m_EquipmentInfosList[i].Level * m_EquipmentInfosList[i].Base);
        }

        TestUserDatas.Capability = capability;
        SendMessageToUIForm("Capability", "战斗力增加了", UIFormNames.HERO_INFO_UIFORM, UIFormNames.SETTINGS_UIFORM);
    }

}

/// <summary>模拟装备数据</summary>
public class EquipmentInfo
{
    public int Level { get; set; }
    public string Name { get; set; }
    public int Base { get; set; }
    public int Coin { get; set; }
    public string Icon { get; set; }

}

