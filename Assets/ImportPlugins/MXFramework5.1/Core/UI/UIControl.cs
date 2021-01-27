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
using System.Linq;
using Mx.Config;
using Mx.Utils;
using UnityEngine;

namespace Mx.UI
{
    /// <summary>UI控制器</summary>
    public class UIControl : MonoBehaviour
    {
        UIFormDatas m_UIFormDatas = new UIFormDatas();

        /// <summary>添加一个UI到Ui管理库</summary>
        public void AddUIForm(UIConfigData uiconfig, string uiFormName, GameObject uiFormPrefab)
        {
            if (uiconfig == null || string.IsNullOrEmpty(uiFormName) || uiFormPrefab == null)
            {
                Debug.LogError(GetType() + "/AddUIForm()/ add uiform error! uiFormName:" + uiFormName);
                return;
            }

            if (m_UIFormDatas.dicAllUIForms.ContainsKey(uiFormName)) return;

            GameObject uiFormObject = createUIFormObject(uiconfig, uiFormName, uiFormPrefab);
            UIFormInfo uIFormInfo = addUIFormInfoScripts(uiconfig, uiFormObject);

            addBaseUIFormScripts(uiconfig,uiFormObject, uiFormName);

            uiFormObject.SetActive(false);

            m_UIFormDatas.dicAllUIForms.Add(uiFormName, uIFormInfo);
        }

        /// <summary>将UI面板从UI管理库中移除</summary>
        public void RemoveUIForm(params string[] uiFormNames)
        {
            for (int i = 0; i < uiFormNames.Length; i++)
            {
                string uiFormName = uiFormNames[i];

                if (m_UIFormDatas.dicOpenUIForms.ContainsKey(uiFormName)) m_UIFormDatas.dicOpenUIForms.Remove(uiFormName);
                if (m_UIFormDatas.dicFreezeOtherUIForms.ContainsKey(uiFormName)) m_UIFormDatas.dicFreezeOtherUIForms.Remove(uiFormName);
                if (m_UIFormDatas.dicReverseChangeUIForms.ContainsKey(uiFormName)) m_UIFormDatas.dicReverseChangeUIForms.Remove(uiFormName);
                if (m_UIFormDatas.dicAllUIForms.ContainsKey(uiFormName))
                {
                    Destroy(m_UIFormDatas.dicAllUIForms[uiFormName].gameObject);
                    m_UIFormDatas.dicAllUIForms.Remove(uiFormName);
                }
            }
        }

        /// <summary>打开UI面板</summary>
        public void OpenUIForms(params string[] uiFormNames)
        {
            for (int i = 0; i < uiFormNames.Length; i++)
            {
                string uiFormName = uiFormNames[i];

                if (!m_UIFormDatas.dicAllUIForms.ContainsKey(uiFormName))
                {
                    Debug.LogError(GetType() + "/OpenUIForms()/ open uiForms error， ui not exist！ uiFormName:" + uiFormName);
                    continue;
                }

                if (m_UIFormDatas.dicOpenUIForms.ContainsKey(uiFormName))
                {
                    Debug.LogWarning(GetType() + "/OpenUIForms()/ open uiForms error， ui exist！ uiFormName:" + uiFormName);
                    continue;
                }

                openUIForm(uiFormName);
            }
        }

        /// <summary>关闭UI面板</summary>
        public void CloseUIForms(params string[] uiFormNames)
        {
            for (int i = 0; i < uiFormNames.Length; i++)
            {
                string uiFormName = uiFormNames[i];

                if (!m_UIFormDatas.dicOpenUIForms.ContainsKey(uiFormName)) continue;
                m_UIFormDatas.dicOpenUIForms[uiFormName].gameObject.SetActive(false);
                closeUIForms(uiFormName);
            }
        }

        /// <summary>延迟关闭UI面板</summary>
        public void CloseUIFormsDelay(int time, params string[] uiFormNames)
        {
            for (int i = 0; i < uiFormNames.Length; i++)
            {
                string uiFormName = uiFormNames[i];
                if (!m_UIFormDatas.dicOpenUIForms.ContainsKey(uiFormName)) continue;

                Timer.CreateTimer(uiFormName + "_Timer").StartTiming(time, () =>
                {
                    m_UIFormDatas.dicOpenUIForms[uiFormName].gameObject.SetActive(false);
                });

                closeUIForms(uiFormName);
            }
        }

        /// <summary>关闭所有UI面板</summary>
        public void CloseAllUIForms()
        {
            m_UIFormDatas.dicFreezeOtherUIForms.Clear();
            m_UIFormDatas.dicReverseChangeUIForms.Clear();

            if (m_UIFormDatas.dicOpenUIForms == null || m_UIFormDatas.dicOpenUIForms.Count < 1) return;
            string[] openUIForms = m_UIFormDatas.dicOpenUIForms.Keys.ToArray<string>();
            for (int i = 0; i < openUIForms.Length; i++) { CloseUIForms(openUIForms[i]); }
        }

        /// <summary>隐藏全部打开的UI面板</summary>
        public void HideOpenUIForms()
        {
            foreach (string uiFormName in m_UIFormDatas.dicOpenUIForms.Keys)
            {
                UIFormInfo uiFormItem = m_UIFormDatas.dicOpenUIForms[uiFormName];
                if (uiFormItem != null) uiFormItem.gameObject.SetActive(false);
            }
        }

        /// <summary>隐藏除排除（exclude）外，的所有打开的UI面板</summary>
        public void HideOther(params string[] excludeUIFormNames)
        {
            foreach (string uiFormName in m_UIFormDatas.dicOpenUIForms.Keys)
            {
                bool isExclude = false;
                for (int i = 0; i < excludeUIFormNames.Length; i++)
                {
                    if (uiFormName.Equals(excludeUIFormNames[i]))
                    {
                        isExclude = true;
                        continue;
                    }
                }

                UIFormInfo uIFormInfo = m_UIFormDatas.dicOpenUIForms[uiFormName];
                if (uIFormInfo != null) uIFormInfo.gameObject.SetActive(isExclude);
            }
        }

        /// <summary>显示所有打开的UI面板（主要是将隐藏后的UI再次显示）</summary>
        public void DisplayOpenUIForms()
        {
            foreach (string uiFormName in m_UIFormDatas.dicOpenUIForms.Keys)
            {
                UIFormInfo uiFormItem = m_UIFormDatas.dicOpenUIForms[uiFormName];
                if (uiFormItem != null) uiFormItem.gameObject.SetActive(true);
            }

            refreshReverseChangeUIForms();
        }

        /// <summary>判断给定UI是否已经打开</summary>
        public bool IsOpen(string uiFormName)
        {
            return m_UIFormDatas.dicOpenUIForms.ContainsKey(uiFormName);
        }

        /// <summary>判断给定UI在UI管理库中是否存在</summary>
        public bool IsExist(string uiFormName)
        {
            return m_UIFormDatas.dicAllUIForms.ContainsKey(uiFormName);
        }

        /// <summary>获取UI显示层级</summary>
        private Transform getParent(UIConfigData uiInfo)
        {
            Transform parent = null;

            if (uiInfo.UIFormsDepth >= UIRoot.Instance.uiFormDepthArr.Length)
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
            uIFormInfo.UIConfig = uiconfig;

            return uIFormInfo;
        }

        /// <summary>添加BaseUI脚本</summary>
        private void addBaseUIFormScripts(UIConfigData uiconfig,GameObject uiFormObject, string uiFormName)
        {
            EnumScriptType scriptType = (EnumScriptType)uiconfig.ScriptType;

            if (scriptType == EnumScriptType.CSharp)
            {
                Type type = Type.GetType(uiFormName);
                if (type != null && uiFormObject.GetComponent<BaseUIForm>() == null) { uiFormObject.AddComponent(type); }

                uiFormObject.GetComponent<BaseUIForm>();
            }
            if(scriptType== EnumScriptType.Lua)
            {
                //添加Lua脚本Todo
            }
        }

        /// <summary>打开一个UI窗体</summary>
        private void openUIForm(string uiFormName)
        {
            UIFormInfo uIFormInfo = null;
            m_UIFormDatas.dicAllUIForms.TryGetValue(uiFormName, out uIFormInfo);

            if (uIFormInfo == null)
            {
                Debug.LogError(GetType() + "/openUIForm()/ open uiForm error, uIFormInfo is null!  uiFormName:" + uiFormName);
                return;
            }

            uIFormInfo.gameObject.SetActive(true);
            m_UIFormDatas.dicOpenUIForms.Add(uiFormName, uIFormInfo);

            setDepthToTop(uiFormName);

            //打开的是反向切换类型UI窗体
            if (uIFormInfo.CurrentUIParam.uIFormShowMode == EnumUIFormShowMode.ReverseChange &&
                !m_UIFormDatas.dicReverseChangeUIForms.ContainsKey(uiFormName))
            {
                m_UIFormDatas.dicReverseChangeUIForms.Add(uiFormName, uIFormInfo);
                refreshReverseChangeUIForms();
            }

            //打开的是冻结其他类型UI窗体
            if (uIFormInfo.CurrentUIParam.uIFormShowMode == EnumUIFormShowMode.FreezeOther &&
                !m_UIFormDatas.dicFreezeOtherUIForms.ContainsKey(uiFormName))
            {
                m_UIFormDatas.dicFreezeOtherUIForms.Add(uiFormName, uIFormInfo);
            }

            //打开的是隐藏其他UI窗口类型
            if (uIFormInfo.CurrentUIParam.uIFormShowMode == EnumUIFormShowMode.HideOther) HideOther(uiFormName);


            EnumScriptType scriptType = (EnumScriptType)uIFormInfo.UIConfig.ScriptType;

            if (scriptType == EnumScriptType.CSharp)
            {
                BaseUIForm baseUIForm = uIFormInfo.gameObject.GetComponent<BaseUIForm>();
                if (baseUIForm != null) baseUIForm.OnOpenUIEvent();
            }
            if (scriptType == EnumScriptType.Lua)
            {
                //通知Lua脚本刷新Todo
            }
        }

        /// <summary>UI层级置顶</summary>
        private void setDepthToTop(string uiFormName)
        {
            UIFormInfo uIFormInfo = null;
            m_UIFormDatas.dicOpenUIForms.TryGetValue(uiFormName, out uIFormInfo);

            if (uIFormInfo == null) return;
            uIFormInfo.gameObject.transform.SetAsLastSibling();
        }

        /// <summary>关闭UI窗体</summary>
        private void closeUIForms(string uiFormName)
        {
            UIFormInfo uIFormInfo = null;
            m_UIFormDatas.dicAllUIForms.TryGetValue(uiFormName, out uIFormInfo);

            m_UIFormDatas.dicOpenUIForms.Remove(uiFormName);

            if (uIFormInfo == null)
            {
                Debug.LogError(GetType() + "/openUIForm()/ open uiForm error, uIFormInfo is null!  uiFormName:" + uiFormName);
                return;
            }

            EnumScriptType scriptType = (EnumScriptType)uIFormInfo.UIConfig.ScriptType;

            if(scriptType== EnumScriptType.CSharp)
            {
                BaseUIForm baseUIForm = uIFormInfo.gameObject.GetComponent<BaseUIForm>();
                if (baseUIForm != null) baseUIForm.OnCloseUIEvent();
            }
            if(scriptType== EnumScriptType.Lua)
            {
                //通知Lua脚本刷新Todo
            }

            if (m_UIFormDatas.dicReverseChangeUIForms.ContainsKey(uiFormName))
            {
                m_UIFormDatas.dicReverseChangeUIForms.Remove(uiFormName);
                refreshReverseChangeUIForms();
            }

            if (m_UIFormDatas.dicFreezeOtherUIForms.ContainsKey(uiFormName)) m_UIFormDatas.dicFreezeOtherUIForms.Remove(uiFormName);
            if (uIFormInfo.CurrentUIParam.uIFormShowMode == EnumUIFormShowMode.HideOther) DisplayOpenUIForms();
        }

        /// <summary>刷新反向切换UI显示</summary>
        private void refreshReverseChangeUIForms()
        {
            if (m_UIFormDatas.dicReverseChangeUIForms == null || m_UIFormDatas.dicReverseChangeUIForms.Count == 0) return;
            int i = 0;

            foreach (UIFormInfo uIFormInfo in m_UIFormDatas.dicReverseChangeUIForms.Values)
            {
                uIFormInfo.gameObject.SetActive(i >= m_UIFormDatas.dicReverseChangeUIForms.Count - 1);
                i++;
            }
        }

    }
}
