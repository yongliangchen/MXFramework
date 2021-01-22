/***
 * 
 *    Title: MXFramework
 *           主题: UI控制器
 *    Description: 
 *           功能：是整个UI框架的核心。
 *                                  
 *    Date: 2021
 *    Version: v5.1版本
 *    Modify Recoder:      
 */

using System;
using Mx.Config;
using Mx.Msg;
using Mx.Utils;
using UnityEngine;

namespace Mx.UI
{
    /// <summary>UI控制器</summary>
    public class UIControl : MonoSingleton<UIControl>
    {
        UIFormDatas m_UIFormDatas = new UIFormDatas();

        /// <summary>添加一个UI到Ui管理库</summary>
        public void AddUIForm(UIConfigData uiconfig, string uiFormName, GameObject uiFormPrefab)
        {
            if (uiconfig == null || string.IsNullOrEmpty(uiFormName) || uiFormPrefab == null || m_UIFormDatas.dicAllUIForms.ContainsKey(uiFormName))
            {
                Debug.LogWarning(GetType() + "/AddUIForm()/ add uiform error! uiFormName:" + uiFormName);
                return;
            }

            GameObject uiFormObject = createUIFormObject(uiconfig, uiFormName, uiFormPrefab);
            UIFormInfo uIFormInfo= addUIFormInfoScripts(uiconfig, uiFormObject);
            BaseUIForm baseUIForm = addBaseUIFormScripts(uiFormObject, uiFormName);
            if (baseUIForm != null)
            {
                baseUIForm.uIFormsDepth = uIFormInfo.CurrentUIParam.uIFormDepth;
                baseUIForm.uIFormShowMode = uIFormInfo.CurrentUIParam.uIFormShowMode;
            }

            uiFormObject.SetActive(false);

            m_UIFormDatas.dicAllUIForms.Add(uiFormName, uIFormInfo);
        }


        /// <summary>将UI面板从UI管理库中移除</summary>
        public void DeleteUIForm(params string[] uiFormNames)
        {

        }

        /// <summary>打开UI面板</summary>
        public void OpenUIForms(params string[] uiFormNames)
        {

        }

        /// <summary>关闭UI面板</summary>
        public void CloseUIForms(params string[] uiFormNames)
        {

        }

        /// <summary>延迟关闭UI面板</summary>
        public void CloseUIFormsDelay(int time, params string[] uiFormNames)
        {

        }

        /// <summary>关闭所有UI面板</summary>
        public void CloseAllUIForms()
        {

        }

        /// <summary>隐藏全部打开的UI面板</summary>
        public void HideOpenUIForms()
        {

        }

        /// <summary>隐藏除排除（exclude）外，的所有打开的UI面板</summary>
        public void HideOther(params string[] excludeUIFormName)
        {

        }

        /// <summary>显示所有打开的UI面板（主要是将隐藏后的UI再次显示）</summary>
        public void DisplayOpenUIForms()
        {

        }

        /// <summary>判断给定UI是否已经打开</summary>
        public bool IsOpen(string uiFormName)
        {
            return false;
        }

        /// <summary>判断给定UI在UI管理库中是否存在</summary>
        public bool IsExist(string uiFormName)
        {
            return false;
        }

        //将UI层级设置成为最高Todo

        /// <summary>发送消息给UI面板</summary>
        public void SendMessageToUIForm(string key, object values, params string[] uiFormNames)
        {
            for (int i = 0; i < uiFormNames.Length; i++)
            {
                MessageCenter.SendMessage(uiFormNames[i] + "Msg", key, values);
            }
        }

        /// <summary>给全部UI面板发送消息</summary>
        public void SendMessageToAllUIForm(string key, object values)
        {
            MessageCenter.SendMessage(UIDefine.GLOBAL_UI_FORM_MSG_EVENT, key, values);
        }

        /// <summary>获取UI显示层级</summary>
        private Transform getParent(UIConfigData uiInfo)
        {
            Transform parent = null;

            if(uiInfo.UIFormsDepth>=UIRoot.Instance.uiFormDepthArr.Length)
            {
                Debug.LogError(GetType() + "/getParent()/Index was outside the bounds of the array！ uiFormName：" + uiInfo.Name);
                return parent;
            }

            parent = UIRoot.Instance.uiFormDepthArr[uiInfo.UIFormsDepth];

            return parent;
        }

        /// <summary>创建UI物体</summary>
        private GameObject createUIFormObject(UIConfigData uiInfo, string uiFormName, GameObject uiFormPrefab)
        {
            GameObject item = Instantiate(uiFormPrefab, getParent(uiInfo));
            item.name = uiFormName;
            return item;
        }

        /// <summary>添加UI数据管理脚本</summary>
        private UIFormInfo addUIFormInfoScripts(UIConfigData uiconfig, GameObject uiFormObject)
        {
            UIParam uiParam = new UIParam();
            uiParam.uIFormDepth = (EnumUIFormDepth)uiconfig.UIFormsDepth;
            uiParam.uIFormShowMode = (EnumUIFormShowMode)uiconfig.UIFormShowMode;
            UIFormInfo uIFormInfo = uiFormObject.GetComponent<UIFormInfo>();
            if (uIFormInfo == null) uIFormInfo = uiFormObject.AddComponent<UIFormInfo>();
            uIFormInfo.CurrentUIParam = uiParam;

            return uIFormInfo;
        }

        /// <summary>添加BaseUI脚本</summary>
        private BaseUIForm addBaseUIFormScripts(GameObject uiFormObject, string uiFormName)
        {
            Type type = Type.GetType(uiFormName);
            if (type != null && uiFormObject.GetComponent<BaseUIForm>() == null) { uiFormObject.AddComponent(type); }

            return uiFormObject.GetComponent<BaseUIForm>();

            //1添加C#脚本或者Lua脚本Todo
        }

    }
}
