using Mx.UI;
using UnityEngine.UI;
using Mx.Utils;
using Mx.Example;

/// <summary> 角色信息UI面板 </summary>
public class HeroInfoUIForm : BaseUIForm
{
    private Text m_CoinText;
    private Text m_CapabilityText;

    public override void OnAwake()
    {
        base.OnAwake();

        m_CoinText = UnityHelper.FindTheChildNode(gameObject, "Coin").GetComponent<Text>();
        m_CoinText.text = UserData.Coin.ToString();
        m_CapabilityText = UnityHelper.FindTheChildNode(gameObject, "Capability").GetComponent<Text>();
        m_CapabilityText.text = m_CapabilityText.text = "战斗力：" + UserData.Capability;

        rigisterButtonEvent();
    }

    public override void OnRelease()
    {
        base.OnRelease();
    }

    /// <summary>注册按钮事件</summary>
    private void rigisterButtonEvent()
    {

    }

    public override void OnCurrentUIFormMsgEvent(string key, object values)
    {
        base.OnCurrentUIFormMsgEvent(key, values);

        if(key.Equals("Capability")) m_CapabilityText.text = "战斗力："+UserData.Capability;
    }

    public override void OnGlobalUIFormMsgEvent(string key, object values)
    {
        base.OnGlobalUIFormMsgEvent(key, values);
        if(key.Equals("ChangeCoin")) m_CoinText.text = UserData.Coin.ToString();
    }

}

