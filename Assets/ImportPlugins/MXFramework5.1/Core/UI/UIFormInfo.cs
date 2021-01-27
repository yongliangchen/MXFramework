using Mx.Config;
using UnityEngine;

namespace Mx.UI
{
    /// <summary>储存当前UI面板的信息</summary>
    public sealed class UIFormInfo : MonoBehaviour
    {
        private UIParam m_CurrentUIParam = new UIParam();
        /// <summary>当前UI窗体类型</summary>
        public UIParam CurrentUIParam
        {
            get { return m_CurrentUIParam; }
            set { m_CurrentUIParam = value; }
        }

        private UIConfigData m_UIConfig;
        /// <summary>UI配置表</summary>
        public UIConfigData UIConfig
        {
            get { return m_UIConfig; }
            set { m_UIConfig = value; }
        }
    }
}