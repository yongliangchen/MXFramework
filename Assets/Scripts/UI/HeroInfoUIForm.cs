using Mx.UI;
using UnityEngine.UI;
using Mx.Utils;
using Mx.Example;

/// <summary> 角色信息UI面板 </summary>
public class HeroInfoUIForm : BaseUIForm
{
    private Text m_CoinText;

    public override void OnAwake()
    {
        base.OnAwake();

        m_CoinText = UnityHelper.FindTheChildNode(gameObject, "Coin").GetComponent<Text>();
        m_CoinText.text = UserData.Coin.ToString();

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

    public override void OnGlobalUIFormMsgEvent(string key, object values)
    {
        base.OnGlobalUIFormMsgEvent(key, values);

        if(key.Equals("ChangeCoin")) m_CoinText.text = UserData.Coin.ToString();
    }

}

