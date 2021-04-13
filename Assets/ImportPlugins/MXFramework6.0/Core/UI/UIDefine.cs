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

        /// <summary>UI设计尺寸，宽</summary>
        public const int UI_ORIFINAL_WIDTH = 1280;
        /// <summary>UI设计尺寸，高</summary>
        public const int UI_ORIFINAL_HEIGHT = 720;

        /// <summary>UI层级名称</summary>
        public const string LAYER_UI = "UI";
        /// <summary>UIRoot的路径</summary>
        public const string NAME_UIROOT = "UIRoot";

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

        private static string uIFormLuaScriptsPath = Application.dataPath + "/Scripts/Lua/src/UI/";
        /// <summary>UI窗口Lua脚本存放路径</summary>
        public static string UIFormLuaScriptsPath
        {
            get
            {
                if (!Directory.Exists(uIFormLuaScriptsPath)) Directory.CreateDirectory(uIFormLuaScriptsPath);
                return uIFormLuaScriptsPath;
            }
        }

        /// <summary>UI名称类模板C#版本</summary>
        public const string Template_UIFORM_NAMES_CSHARP = "Template/UI/Template_UIFormNamesCSharp";
        /// <summary>UI名称类模板Lua版本</summary>
        public const string Template_UIFORM_NAMES_LUA = "Template/UI/Template_UIFormNamesLua";
        /// <summary>C#脚本模板</summary>
        public const string Template_UIFORM_CSHARP_BASE = "Template/UI/Template_UIFormCSharpBase";
        /// <summary>Lua脚本模板</summary>
        public const string Template_UIFORM_LUA_BASE = "Template/UI/Template_UIFormLuaBase";
    }

    #region 系统枚举类型

    /// <summary>Ui窗体层级</summary>
    public enum EnumUIFormDepth
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
    public enum EnumUIFormShowMode
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
    public enum EnumLoadType
    {
        /// <summary>通过Resources方式加载</summary>
        Resources,
        /// <summary>通过AssetBundle方式加载</summary>
        AssetBundle,
    }

    /// <summary>开发语言类型</summary>
    public enum EnumScriptType
    {
        CSharp,
        Lua,
    }

    /// <summary>UI动画样式</summary>
    public enum EnumUIAnimationStyle
    {
        /// <summary>没有动画</summary>
        None,
        /// <summary>中间由小变大 </summary>
        CenterScaleBigNomal = 0,
        /// <summary>由上往下 </summary>
        TopToSlide,
        /// <summary>由下往上 </summary>
        DoweToSlide,
        /// <summary>由左往中 </summary>
        LeftToSlide,
        /// <summary>由右往中 </summary>
        RightToSlide,
    }

    #endregion

}


