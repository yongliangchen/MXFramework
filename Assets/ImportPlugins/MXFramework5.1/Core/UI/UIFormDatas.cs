/***
 * 
 *    Title: MXFramework
 *           主题: UI面板数据
 *    Description: 
 *           功能：UI面板数据
 *                                  
 *    Date: 2020
 *    Version: v5.1版本
 *    Modify Recoder:      
 */

using System.Collections.Generic;

namespace Mx.UI
{
    /// <summary>UI面板数据</summary>
    public class UIFormDatas
    {
        /// <summary>UI管理库</summary>
        public Dictionary<string, UIFormInfo> dicAllUIForms { get; private set; } 
        /// <summary>全部已经打开的UI面板</summary>
		public Dictionary<string, UIFormInfo> dicOpenUIForms { get; private set; } 
        /// <summary>全部反向切换的UI面板</summary>
		public Dictionary<string, UIFormInfo> dicReverseChangeUIForms { get; private set; }
        /// <summary>全部隐藏其他的UI面板</summary>
        public Dictionary<string, UIFormInfo> dicFreezeOtherUIForms { get; private set; }

        public UIFormDatas()
        {
            dicAllUIForms = new Dictionary<string, UIFormInfo>();
            dicOpenUIForms = new Dictionary<string, UIFormInfo>();
            dicReverseChangeUIForms = new Dictionary<string, UIFormInfo>();
            dicFreezeOtherUIForms = new Dictionary<string, UIFormInfo>();
        }
    }
}