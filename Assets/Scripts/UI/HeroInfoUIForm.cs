using Mx.UI;
using UnityEngine.UI;
using Mx.Utils;
using Mx.Example;

/// <summary> 角色信息UI面板 </summary>
public class HeroInfoUIForm : BaseUIForm
{
    private Text m_TextName;
    private Text m_TextCoin;
    private Text m_TextCapability;

    public override void OnAwake()
    {
        base.OnAwake();

        m_TextName=UnityHelper.FindTheChildNode(gameObject, "Name").GetComponent<Text>();
        m_TextName.text = TestUserDatas.Name;
        m_TextCoin = UnityHelper.FindTheChildNode(gameObject, "Coin").GetComponent<Text>();
        m_TextCoin.text = TestUserDatas.Coin.ToString();
        m_TextCapability = UnityHelper.FindTheChildNode(gameObject, "Capability").GetComponent<Text>();
        m_TextCapability.text = m_TextCapability.text = "战斗力：" + TestUserDatas.Capability;

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

        if(key.Equals("Capability")) m_TextCapability.text = "战斗力："+ TestUserDatas.Capability;
    }

    public override void OnGlobalUIFormMsgEvent(string key, object values)
    {
        base.OnGlobalUIFormMsgEvent(key, values);
        if(key.Equals("ChangeCoin")) m_TextCoin.text = TestUserDatas.Coin.ToString();
    }

}

