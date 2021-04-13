using Mx.UI;
using UnityEngine;
using UnityEngine.UI;
using Mx.Utils;

/// <summary> 注册UI面板 </summary>
public class RegisterUIForm : BaseUIForm
{
    private InputField m_InputName;
    private InputField m_InputPassword;

    public override void OnAwake()
    {
        base.OnAwake();

        m_InputName = UnityHelper.FindTheChildNode(gameObject, "InputName").GetComponent<InputField>();
        m_InputPassword = UnityHelper.FindTheChildNode(gameObject, "InputPassword").GetComponent<InputField>();

        rigisterButtonEvent();
    }

    /// <summary>关闭UI窗口事件</summary>
    public override void OnCloseUIEvent()
    {
        base.OnCloseUIEvent();

        m_InputName.text = null;
        m_InputPassword.text = null;
    }

    /// <summary>注册按钮事件</summary>
    private void rigisterButtonEvent()
    {
        RigisterButtonEvent("BtnDefine", define);
        RigisterButtonEvent("BtnClose", closeCurrentUIForm);
    }

    /// <summary>确认注册</summary>
    private void define(GameObject btnObject)
    {
        if (verify()) OpenUIAndCloseCurrentUI(UIFormNames.LOGIN_UIFORM);
    }

    /// <summary>关闭当前UI面板</summary>
    private void closeCurrentUIForm(GameObject btnObject)
    {
        OpenUIAndCloseCurrentUI(UIFormNames.LOGIN_UIFORM);
    }

    /// <summary>验证登录</summary>
    private bool verify()
    {
        if (string.IsNullOrEmpty(m_InputName.text) || string.IsNullOrEmpty(m_InputPassword.text))
        {
            Toast.Show("账号或者密码不能为空！");
            return false;
        }

        //链接服务器注册账号......

        return true;
    }

}

