/***
 * 
 *    Title: MXFramework
 *           主题: UI管理器
 *    Description: 
 *           功能：1.UI框架和业务逻辑的中间件
 *                2.UI框架和业务逻辑沟通的桥梁
 *                                  
 *    Date: 2021
 *    Version: v5.1版本
 *    Modify Recoder:      
 */


using System.Collections.Generic;
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
        private UIConfigDatabase m_UIConfig;
        private  UIControl m_UIControl;
        private Dictionary<string, string> m_DicLoadUIForm = new Dictionary<string, string>();

        private void Awake()
        {
            m_UIConfig = ConfigManager.Instance.GetDatabase<UIConfigDatabase>();
            m_UIControl = FindObjectOfType<UIControl>();
        }

        /// <summary>打开UI面板</summary>
        public void OpenUIForms(params string[] uiFormNames)
        {
            for(int i=0;i<uiFormNames.Length;i++)
            {
                string uiformName = uiFormNames[i];
                if (m_UIControl.IsOpen(uiformName)) continue;

                if (m_UIControl.IsExist(uiformName)) m_UIControl.OpenUIForms(uiformName);
                else loadUIForm(uiformName);
            }
        }

        /// <summary>关闭UI面板</summary>
        public void CloseUIForms(params string[] uiFormNames)
        {
            m_UIControl.CloseUIForms(uiFormNames);
        }

        /// <summary>延迟关闭UI面板</summary>
        public void CloseUIFormsDelay(int time, params string[] uiFormNames)
        {
            m_UIControl.CloseUIFormsDelay(time, uiFormNames);
        }

        /// <summary>关闭所有UI面板</summary>
        public void CloseAllUIForms()
        {
            m_UIControl.CloseAllUIForms();
        }

        /// <summary>隐藏全部打开的UI面板</summary>
        public void HideOpenUIForms()
        {
            m_UIControl.HideOpenUIForms();
        }

        /// <summary>隐藏除排除（exclude）外，的所有打开的UI面板</summary>
        public void HideOther(params string[] excludeUIFormNames)
        {
            m_UIControl.HideOther(excludeUIFormNames);
        }

        /// <summary>显示所有打开的UI面板（主要是将隐藏后的UI再次显示）</summary>
        public void DisplayOpenUIForms()
        {
            m_UIControl.DisplayOpenUIForms();
        }

        /// <summary>判断给定UI是否已经打开</summary>
        public bool IsOpen(string uiFormName)
        {
            return m_UIControl.IsOpen(uiFormName);
        }

        /// <summary>判断给定UI在UI管理库中是否存在</summary>
        public bool IsExist(string uiFormName)
        {
            return m_UIControl.IsExist(uiFormName);
        }

        /// <summary>发送消息给指定UI面板</summary>
        public void SendMessageToUIForm(string key, object values, params string[] uiFormNames)
        {
            for(int i=0;i< uiFormNames.Length;i++)
            {
                string uiFormName = uiFormNames[i];
                MessageCenter.SendMessage(uiFormName + "Msg", key, values);
            }
        }

        /// <summary>发送消息给全部UI面板</summary>
        public void SendGlobalUIFormMsg(string key, object values)
        {
            MessageCenter.SendMessage(UIDefine.GLOBAL_UI_FORM_MSG_EVENT, key, values);
        }

        /// <summary>加载UI面板</summary>
        private void loadUIForm(string uiFormName)
        {
            if (m_DicLoadUIForm.ContainsKey(uiFormName)) return;

            m_DicLoadUIForm.Add(uiFormName, uiFormName);

            UIConfigData uiInfo = m_UIConfig.GetDataByKey(uiFormName);
            if (uiInfo == null)
            {
                if (m_DicLoadUIForm.ContainsKey(uiFormName)) m_DicLoadUIForm.Remove(uiFormName);
                Debug.LogError(GetType() + "/loadUIForm()/ uiConfigData is null! uiFormName:" + uiFormName);
                return;
            }

            if ((EnumLoadType)uiInfo.LandType == EnumLoadType.Resources)
            {
                GameObject prefab = ResoucesMgr.Instance.Load<GameObject>(uiInfo.ResourcesPath, false);
                loadUIFormFinish(uiInfo,uiFormName, prefab);
            }
            else if ((EnumLoadType)uiInfo.LandType == EnumLoadType.AssetBundle)
            {
                AssetBundleMgr.Instance.LoadAssetBunlde("UI", uiInfo.AssetBundlePath);
                GameObject prefab = AssetBundleMgr.Instance.LoadAsset("UI", uiInfo.AssetBundlePath, uiInfo.AssetName) as GameObject;
                loadUIFormFinish(uiInfo,uiFormName, prefab);
            }
        }

        /// <summary>加载UI面板完成</summary>
        private void loadUIFormFinish(UIConfigData uiInfo,string uiFormName,GameObject uiFormPrefab)
        {
            if (m_DicLoadUIForm.ContainsKey(uiFormName)) m_DicLoadUIForm.Remove(uiFormName);

            if(uiFormPrefab==null)
            {
                Debug.LogError(GetType() + "/loadUIFormFinish()/ uiFormPrefab is null! uiFormName:"+ uiFormName);
                return;
            }

            if (m_UIControl.IsExist(uiFormName))
            {
                m_UIControl.OpenUIForms(uiFormName);
                return;
            }

            m_UIControl.AddUIForm(uiInfo, uiFormName, uiFormPrefab);

            m_UIControl.OpenUIForms(uiFormName);
        }

    }
}