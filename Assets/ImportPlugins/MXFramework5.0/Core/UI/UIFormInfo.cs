using UnityEngine;

namespace Mx.UI
{
    /// <summary>储存当前UI面板的信息</summary>
    public sealed class UIFormInfo : MonoBehaviour
    {
        private UIParam currentUIParam = new UIParam();
        /// <summary>当前UI窗体类型</summary>
        public UIParam CurrentUIParam
        {
            get { return currentUIParam; }
            set { currentUIParam = value; }
        }

    }
}