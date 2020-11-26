/***
 * 
 *    Title: MXFramework
 *           主题: UI模块全局定义
 *    Description: 
 *           功能：1.UI模块全局枚举定义
 *                2.UI模块全局委托定义
 *                3.UI模块全局数据定义 
 *               
 *                                  
 *    Date: 2020
 *    Version: v4.1版本
 *    Modify Recoder:      
 *
 */

using UnityEngine;
using System.IO;

namespace Mx.UI
{
    public sealed class UIDefine
    {
        /// <summary>全局UI消息事件</summary>
        public const string GLOBAL_UI_FORM_MSG_EVENT = "GlobalUIFormMsgEvent";
        /// <summary>吐司消息</summary>
        public const string TOAST_INFO_MSG = "ToastInfoMsg";

        /// <summary>UIRoot的路径</summary>
        public const string PATH_UIROOT = "UIRoot";
        /// <summary>普通窗体节点常量</summary>
        public const string NORMAL_MODE = "Normal";
        /// <summary>固定窗体节点常量</summary>
        public const string FIXED_MODE = "Fixed";
        /// <summary>弹出窗体节点常量</summary>
        public const string POPUP_MODE = "PopUp";
        /// <summary>通知窗体节点常量</summary>
        public const string NOTICE_MODE = "Notice";
        /// <summary>脚本管理节点常量</summary>
        public const string SCRIPTSLMANAGER_MODE = "ScriptsManager";

        private static string uiFormCSharpScriptsPath = Application.dataPath + "/Scripts/UI/";
        /// <summary>Ui窗口C#脚本存放路径</summary>
        public static string UIFormCSharpScriptsPath
        {
            get
            {
                if (!Directory.Exists(uiFormCSharpScriptsPath)) Directory.CreateDirectory(uiFormCSharpScriptsPath);
                return uiFormCSharpScriptsPath;
            }
        }

        public const string Template_UIFORM_NAMES = "Template/UI/Template_UIFormNames";
        public const string Template_UIFORM_CSHARP_BASE = "Template/UI/Template_UIFormCSharpBase";
    }

    #region 系统枚举类型

    /// <summary>Ui窗体层级</summary>
    public enum UIFormDepth
    {
        /// <summary>普通窗体</summary>
        Normal,
        /// <summary>固定窗体</summary>
        Fixed,
        /// <summary>弹出窗体</summary>
        PopUp,
        /// <summary>通知面板</summary>
        Notice,
    }

    /// <summary>UI窗体显示模式</summary>
    public enum UIFormShowMode
    {
        /// <summary>普通窗体模式</summary>
        Normal,
        /// <summary>反向切换模式</summary>
        ReverseChange,
        /// <summary>隐藏其他模式</summary>
        HideOther,
        /// <summary>冻结其他模式</summary>
        FreezeOther,
        /// <summary>吐司模式</summary>
        Toast,
    }

    /// <summary>UI加载方式</summary>
    public enum LoadType
    {
        /// <summary>通过Resources方式加载</summary>
        Resources,
        /// <summary>通过AssetBundle方式加载</summary>
        AssetBundle,
    }

    /// <summary>UI方向</summary>
    public enum UIDirection
    {
        Horizontal,
        Vertical,
    }

    #endregion

}


