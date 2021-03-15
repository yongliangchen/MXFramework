using Mx.Config;
using UnityEngine;

namespace Mx.UI
{
    /// <summary>储存当前UI面板的信息</summary>
    public sealed class UIFormInfo : MonoBehaviour
    {
        /// <summary>UI层级</summary>
        public EnumUIFormDepth uIFormDepth { get; set; }
        /// <summary>UI显示模式</summary>
        public EnumUIFormShowMode uIFormShowMode { get; set; }
        /// <summary>UI配置表</summary>
        public UIConfigData UIConfig { get; set; }
    }
}