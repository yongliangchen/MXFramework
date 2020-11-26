/***
 * 
 *    Title: MXFramework
 *           主题: UI管理器(有空的时候重构，拆分脚本Todo)
 *    Description: 
 *           功能：是整个UI框架的核心。
 *                                  
 *    Date: 2020
 *    Version: v4.1版本
 *    Modify Recoder:      
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Mx.Config;
using Mx.Msg;
using Mx.Res;
using Mx.Utils;
using UnityEngine;

namespace Mx.UI
{
    /// <summary>UI管理器</summary>
    public class UIManager : MonoSingleton<UIManager>
    {
        private UIConfigDatabase uIConfig;

        private Dictionary<string, UIFormItem> m_DicAllUIForms = new Dictionary<string, UIFormItem>();
        private Dictionary<string, UIFormItem> m_DicOpenUIForms = new Dictionary<string, UIFormItem>();
        private Dictionary<string, BaseUIForm> m_DicReverseChangeUIForms = new Dictionary<string, BaseUIForm>();
        private Dictionary<string, BaseUIForm> m_DicFreezeOtherUIForms = new Dictionary<string, BaseUIForm>();
        private Dictionary<string,BaseUIForm> m_DicOpenUIFormsStack = new Dictionary<string, BaseUIForm>();

        private Transform m_TraUiRoot = null;
        private Transform m_TraNormal = null;
        private Transform m_TraFixed = null;
        private Transform m_TraPopUp = null;
        private Transform m_TraNotice = null;
        private Transform m_TraScripts = null;

        private void Awake()
        {
            initUI();
        }

        public void OpenUIForms(params string[] uiFormNames)
        {
            for (int i = 0; i < uiFormNames.Length; i++)
            {
                string uiFormName = uiFormNames[i];

                if (m_DicOpenUIForms.ContainsKey(uiFormName)) continue;

                UIConfigData uiInfo = uIConfig.GetDataByKey(uiFormName);
                if (uiInfo == null)
                {
                    Debug.LogWarning(GetType() + "/OpenUIForms()/ui is null! uiFormName:" + uiFormName);
                    return;
                }

                if (m_DicAllUIForms.ContainsKey(uiFormName)) openUIForm(uiFormName);
                else
                {
                    m_DicAllUIForms.Add(uiFormName, null);
                    loadUIForm(uiInfo, uiFormName);
                }
            }
        }

        public void CloseUIFormsDelay(int time, params string[] uiFormNames)
        {
            for (int i = 0; i < uiFormNames.Length; i++)
            {
                string uiFormName = uiFormNames[i];

                if (m_DicOpenUIForms.ContainsKey(uiFormName))
                {
                    Timer.CreateTimer(uiFormName + "_Timer").StartTiming(time, () =>
                    {
                        if (!m_DicOpenUIForms.ContainsKey(uiFormName)) { m_DicAllUIForms[uiFormName].gameObject.SetActive(false); }
                    });

                    closeUIForms(uiFormName);
                }
            }
        }

        public void CloseUIForms(params string[] uiFormNames)
        {
            for (int i = 0; i < uiFormNames.Length; i++)
            {
                string uiFormName = uiFormNames[i];

                if (m_DicOpenUIForms.ContainsKey(uiFormName))
                {
                    if (m_DicOpenUIForms[uiFormName] != null) m_DicOpenUIForms[uiFormName].gameObject.SetActive(false);
                    closeUIForms(uiFormName);
                }
            }
        }

        public void CloseAllUIForms()
        {
            m_DicFreezeOtherUIForms.Clear();
            m_DicReverseChangeUIForms.Clear();
            m_DicOpenUIFormsStack.Clear();
            if (m_DicOpenUIForms == null || m_DicOpenUIForms.Count < 1) return;
            string[] openUIForms = m_DicOpenUIForms.Keys.ToArray<string>();
            for (int i = 0; i < openUIForms.Length; i++) { CloseUIForms(openUIForms[i]); }
        }

        public void HideOpenUIForms()
        {
            foreach (string uiFormName in m_DicOpenUIForms.Keys)
            {
                UIFormItem uiFormItem = m_DicOpenUIForms[uiFormName];
                if (uiFormItem != null) uiFormItem.gameObject.SetActive(false);
            }
        }

        public void HideOther(string excludeUIFormName)
        {
            foreach (string uiFormName in m_DicOpenUIForms.Keys)
            {
                if (uiFormName.Equals(excludeUIFormName)) continue;
                UIFormItem uiFormItem = m_DicOpenUIForms[uiFormName];
                if (uiFormItem != null) uiFormItem.gameObject.SetActive(false);
            }
        }

        public void DisplayOpenUIForms()
        {
            foreach (string uiFormName in m_DicOpenUIForms.Keys)
            {
                UIFormItem uiFormItem = m_DicOpenUIForms[uiFormName];
                if (uiFormItem != null) uiFormItem.gameObject.SetActive(true);
            }

            refreshReverseChangeUIForms();
        }

        public bool IsOpen(string uiFormName)
        {
            return m_DicOpenUIForms.ContainsKey(uiFormName);
        }

        public void SendMessageToUIForm(string uiFormName, string key, object values)
        {
            MessageCenter.SendMessage(uiFormName + "Msg", key, values);
        }

        public void SendGlobalUIFormMsg(string key, object values)
        {
            MessageCenter.SendMessage(UIDefine.GLOBAL_UI_FORM_MSG_EVENT, key, values);
        }

        private void initUI()
        {
            uIConfig = ConfigManager.Instance.GetDatabase<UIConfigDatabase>();

            m_TraUiRoot = Mx.Res.ResoucesMgr.Instance.CreateGameObject(UIDefine.PATH_UIROOT, false).transform;
            m_TraUiRoot.name = "UIRoot";

            m_TraNormal = UnityHelper.FindTheChildNode(m_TraUiRoot.gameObject, UIDefine.NORMAL_MODE);
            m_TraFixed = UnityHelper.FindTheChildNode(m_TraUiRoot.gameObject, UIDefine.FIXED_MODE);
            m_TraPopUp = UnityHelper.FindTheChildNode(m_TraUiRoot.gameObject, UIDefine.POPUP_MODE);
            m_TraNotice = UnityHelper.FindTheChildNode(m_TraUiRoot.gameObject, UIDefine.NOTICE_MODE);
            m_TraScripts = UnityHelper.FindTheChildNode(m_TraUiRoot.gameObject, UIDefine.SCRIPTSLMANAGER_MODE);

            this.gameObject.transform.SetParent(m_TraScripts, false);
            DontDestroyOnLoad(m_TraUiRoot);//加载场景的时候不销毁
        }

        private void loadUIForm(UIConfigData uiInfo, string uiFormName)
        {
            Transform parent = null;
            switch ((UIFormDepth)uiInfo.UIFormsDepth)
            {
                case UIFormDepth.Normal: parent = m_TraNormal; break;
                case UIFormDepth.Fixed: parent = m_TraFixed; break;
                case UIFormDepth.PopUp: parent = m_TraPopUp; break;
                case UIFormDepth.Notice: parent = m_TraNotice; break;
            }

            UIParam uiParam = new UIParam();
            uiParam.uIFormDepth = (UIFormDepth)uiInfo.UIFormsDepth;
            uiParam.uIFormShowMode = (UIFormShowMode)uiInfo.UIFormShowMode;

            if ((LoadType)uiInfo.LandType == LoadType.Resources)
            {
                GameObject prefab = ResoucesMgr.Instance.Load<GameObject>(uiInfo.ResourcesPath, false);
                loadUIFormFinish(uiFormName, prefab, parent, uiParam);
            }
            else if ((LoadType)uiInfo.LandType == LoadType.AssetBundle)
            {
                AssetBundleMgr.Instance.LoadAssetBunlde("UI", uiInfo.AssetBundlePath);
                GameObject prefab = AssetBundleMgr.Instance.LoadAsset("UI", uiInfo.AssetBundlePath, uiInfo.AssetName) as GameObject;
                loadUIFormFinish(uiFormName, prefab, parent, uiParam);
            }
        }

        private void loadUIFormFinish(string uiFormName, GameObject prefab, Transform parent, UIParam uiParam)
        {
            if (prefab == null)
            {
                if (m_DicAllUIForms.ContainsKey(uiFormName)) m_DicAllUIForms.Remove(uiFormName);
                Debug.LogWarning(GetType() + "/loadUIForm()/ load ui error! uiFormName:" + uiFormName);
                return;
            }

            GameObject item = Instantiate(prefab, parent);
            item.name = uiFormName;

            UIFormItem uIFormItem = item.GetComponent<UIFormItem>();
            if (uIFormItem == null) uIFormItem = item.AddComponent<UIFormItem>();
            uIFormItem.CurrentUIParam = uiParam;
            m_DicAllUIForms[uiFormName] = uIFormItem;

            Type type = Type.GetType(uiFormName);
            if (type != null && item.GetComponent<BaseUIForm>() == null) { item.AddComponent(type); }

            BaseUIForm baseUIForm = item.GetComponent<BaseUIForm>();
            if (baseUIForm != null)
            {
                baseUIForm.uIFormsDepth = uIFormItem.CurrentUIParam.uIFormDepth;
                baseUIForm.uIFormShowMode = uIFormItem.CurrentUIParam.uIFormShowMode;
            }

            if (!m_DicOpenUIForms.ContainsKey(uiFormName)) { openUIForm(uiFormName); }
        }

        private void openUIForm(string uiFormName)
        {
            if (m_DicAllUIForms[uiFormName] != null) m_DicAllUIForms[uiFormName].gameObject.SetActive(true);
            if (m_DicAllUIForms[uiFormName] != null) m_DicOpenUIForms.Add(uiFormName, m_DicAllUIForms[uiFormName]);

            BaseUIForm baseUIForm = m_DicAllUIForms[uiFormName].gameObject.GetComponent<BaseUIForm>();
            if (baseUIForm != null)
            {
                if (baseUIForm.uIFormShowMode == UIFormShowMode.ReverseChange && !m_DicReverseChangeUIForms.ContainsKey(uiFormName))
                {
                    m_DicReverseChangeUIForms.Add(uiFormName, baseUIForm);
                    refreshReverseChangeUIForms();
                }

                if (baseUIForm.uIFormShowMode == UIFormShowMode.FreezeOther && !m_DicFreezeOtherUIForms.ContainsKey(uiFormName))
                    m_DicFreezeOtherUIForms.Add(uiFormName, baseUIForm);

                if((baseUIForm.uIFormsDepth == UIFormDepth.PopUp || baseUIForm.uIFormsDepth == UIFormDepth.Notice) &&
                    baseUIForm.uIFormShowMode!= UIFormShowMode.FreezeOther && baseUIForm.uIFormShowMode != UIFormShowMode.Toast &&
                    !m_DicOpenUIFormsStack.ContainsKey(uiFormName))
                    m_DicOpenUIFormsStack.Add(uiFormName, baseUIForm);

                if (baseUIForm.uIFormShowMode == UIFormShowMode.HideOther) HideOther(uiFormName);

                baseUIForm.OnOpenUIEvent();
            }
        }

        private void closeUIForms(string uiFormName)
        {
            m_DicOpenUIForms.Remove(uiFormName);
            BaseUIForm baseUIForm = m_DicAllUIForms[uiFormName].gameObject.GetComponent<BaseUIForm>();
            if (baseUIForm != null) baseUIForm.OnCloseUIEvent();

            if (m_DicReverseChangeUIForms.ContainsKey(uiFormName))
            {
                m_DicReverseChangeUIForms.Remove(uiFormName);
                refreshReverseChangeUIForms();
            }

            if (m_DicFreezeOtherUIForms.ContainsKey(uiFormName)) m_DicFreezeOtherUIForms.Remove(uiFormName);
            if (baseUIForm.uIFormShowMode == UIFormShowMode.HideOther) DisplayOpenUIForms();

            if(m_DicOpenUIFormsStack.ContainsKey(uiFormName)) m_DicOpenUIFormsStack.Remove(uiFormName);
        }

        private void refreshReverseChangeUIForms()
        {
            if (m_DicReverseChangeUIForms == null || m_DicReverseChangeUIForms.Count == 0) return;
            int i = 0;
            foreach (BaseUIForm baseUIForm in m_DicReverseChangeUIForms.Values)
            {
                baseUIForm.gameObject.SetActive(i >= m_DicReverseChangeUIForms.Count - 1);
                i++;
            }
        }
    }
}