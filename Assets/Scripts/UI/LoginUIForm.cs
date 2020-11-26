using Mx.UI;
using Mx.Utils;
using UnityEngine;
using UnityEngine.UI;

    /// <summary> 登入UI面板 </summary>
    public class LoginUIForm : BaseUIForm
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

        public override void OnRelease()
        {
            base.OnRelease();
        }

        /// <summary>注册按钮事件</summary>
        private void rigisterButtonEvent()
        {
            RigisterButtonObjectEvent("BtnLogin", login);
            RigisterButtonObjectEvent("BtnRegister", register);
        }

        /// <summary>登录</summary>
        private void login(GameObject btnObject)
        {
            OpenUIAndCloseCurrentUI(UIFormNames.SELECT_HERO_UIFORM);
        }

        /// <summary>注册</summary>
        private void register(GameObject btnObject)
        {

        }

    }