/***
 * 
 *    Title: MXFramework
 *           主题: UI管理器
 *    Description: 
 *           功能：是整个UI框架的核心。
 *                                  
 *    Date: 2020
 *    Version: v4.1版本
 *    Modify Recoder:      
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Mx.Config;
using Mx.Res;
using Mx.Utils;
using UnityEngine;


namespace Mx.UI
{
    /// <summary>UI管理器</summary>
    public class UIManager : MonoSingleton<UIManager>
    {
        #region 私有函数

        private UIConfigDatabase uIConfig;

        /// <summary>缓存中所有的UI窗体</summary>
        private Dictionary<string, UIFormItem> dicAllUIForms = new Dictionary<string, UIFormItem>();
        /// <summary>当前显示的UI窗体</summary>
        private Dictionary<string, UIFormItem> dicOpenUIForms = new Dictionary<string, UIFormItem>();
        /// <summary>定义“栈”集合，储存显示当前所有【反向切换】的窗体集合</summary>
        private Stack<string> staCurrentUIForms = new Stack<string>();

        /// <summary>UI根节点</summary>
        private Transform traUiRoot = null;
        /// <summary>全屏幕显示的节点</summary>
        private Transform traNormal = null;
        /// <summary>固定显示节点</summary>
        private Transform traFixed = null;
        /// <summary>弹出节点</summary>
        private Transform traPopUp = null;
        /// <summary>通知面板</summary>
        private Transform traNotice = null;
        /// <summary>UI管理脚本节点</summary>
        private Transform traScripts = null;

        #endregion

        #region Unity函数

        private void Awake()
        {
            InitUI();
        }

        #endregion

        #region 公开函数

        public void OpenUIForms(params string[] uiFormNames)
        {
            for (int i = 0; i < uiFormNames.Length; i++)
            {
                string uiFormName = uiFormNames[i];

                if (dicOpenUIForms.ContainsKey(uiFormName)) continue;

                UIConfigData uiInfo = uIConfig.GetDataByKey(uiFormName);
                if (uiInfo == null)
                {
                    Debug.LogWarning(GetType() + "/OpenUIForms()/ Open ui error! ui is null! uiFormName:" + uiFormName);
                    return;
                }

                if (dicAllUIForms.ContainsKey(uiFormName))
                {
                    if (dicAllUIForms[uiFormName] != null) dicAllUIForms[uiFormName].gameObject.SetActive(true);
                    if (dicAllUIForms[uiFormName] != null) dicOpenUIForms.Add(uiFormName, dicAllUIForms[uiFormName]);

                    BaseUIForm baseUIForm = dicAllUIForms[uiFormName].gameObject.GetComponent<BaseUIForm>();
                    if (baseUIForm != null) baseUIForm.OnOpenUI();
                }
                else
                {
                    dicAllUIForms.Add(uiFormName, null);
                    LoadUIForm(uiInfo, uiFormName);
                }
            }
        }

        public void CloseUIForms(params string[] uiFormNames)
        {
            for (int i = 0; i < uiFormNames.Length; i++)
            {
                string uiFormName = uiFormNames[i];

                if (dicOpenUIForms.ContainsKey(uiFormName))
                {
                    if (dicOpenUIForms[uiFormName] != null) dicOpenUIForms[uiFormName].gameObject.SetActive(false);
                    dicOpenUIForms.Remove(uiFormName);

                    BaseUIForm baseUIForm = dicAllUIForms[uiFormName].gameObject.GetComponent<BaseUIForm>();
                    if (baseUIForm != null) baseUIForm.OnCloseUI();
                }
            }
        }

        public void CloseAllUIForms()
        {
            if (dicOpenUIForms == null || dicOpenUIForms.Count < 1) return;

            string[] openUIForms = dicOpenUIForms.Keys.ToArray<string>();
            for (int i = 0; i < openUIForms.Length; i++)
            {
                CloseUIForms(openUIForms[i]);
            }
        }

        public void HideOpenUIForms()
        {
            foreach (string uiFormName in dicOpenUIForms.Keys)
            {
                UIFormItem uiFormItem = dicOpenUIForms[uiFormName];
                if (uiFormItem != null) uiFormItem.gameObject.SetActive(false);
            }
        }

        public void DisplayOpenUIForms()
        {
            foreach (string uiFormName in dicOpenUIForms.Keys)
            {
                UIFormItem uiFormItem = dicOpenUIForms[uiFormName];
                if (uiFormItem != null) uiFormItem.gameObject.SetActive(true);
            }
        }

        //public bool IsOpen(EnumUIFormType uIFormType)
        //{
        //    return IsOpen(uIFormType.ToString());
        //}

        public bool IsOpen(string uiFormName)
        {
            return dicOpenUIForms.ContainsKey(uiFormName);
        }

        #endregion

        #region 私有函数

        private void InitUI()
        {
            DatabaseManager databaseManager = new DatabaseManager();
            databaseManager.Load();
            uIConfig = databaseManager.GetDatabase<UIConfigDatabase>();

            traUiRoot = Mx.Res.ResoucesMgr.Instance.CreateGameObject(UIDefine.PATH_UIROOT, false).transform;
            traUiRoot.name = "UIRoot";

            traNormal = UnityHelper.FindTheChildNode(traUiRoot.gameObject, UIDefine.NORMAL_MODE);
            traFixed = UnityHelper.FindTheChildNode(traUiRoot.gameObject, UIDefine.FIXED_MODE);
            traPopUp = UnityHelper.FindTheChildNode(traUiRoot.gameObject, UIDefine.POPUP_MODE);
            traNotice = UnityHelper.FindTheChildNode(traUiRoot.gameObject, UIDefine.NOTICE_MODE);
            traScripts = UnityHelper.FindTheChildNode(traUiRoot.gameObject, UIDefine.SCRIPTSLMANAGER_MODE);

            this.gameObject.transform.SetParent(traScripts, false);
            DontDestroyOnLoad(traUiRoot);//加载场景的时候不销毁
        }

        private void LoadUIForm(UIConfigData uiInfo, string uiFormName)
        {
            Transform parent = null;
            switch ((UIFormDepth)uiInfo.UIFormsDepth)
            {
                case UIFormDepth.Normal: parent = traNormal; break;
                case UIFormDepth.Fixed: parent = traFixed; break;
                case UIFormDepth.PopUp: parent = traPopUp; break;
                case UIFormDepth.Notice: parent = traNotice; break;
            }

            UIParam uiParam = new UIParam();
            uiParam.UIFormsDepth = (UIFormDepth)uiInfo.UIFormsDepth;
            //uiParam.UIFormsShowMode = (UIFormShowMode)uiInfo.UIFormShowMode;

            if ((LoadType)uiInfo.LandType == LoadType.Resources)
            {
                GameObject prefab = ResoucesMgr.Instance.Load<GameObject>(uiInfo.ResourcesPath, false);
                LoadUIFormFinish(uiFormName, prefab, parent, uiParam);
            }
            else if ((LoadType)uiInfo.LandType == LoadType.AssetBundle)
            {
                AssetBundleMgr.Instance.LoadAssetBunlde("UI", uiInfo.AssetBundlePath);
                GameObject prefab = AssetBundleMgr.Instance.LoadAsset("UI", uiInfo.AssetBundlePath, uiInfo.AssetName) as GameObject;
                LoadUIFormFinish(uiFormName, prefab, parent, uiParam);
            }
        }

        private void LoadUIFormFinish(string uiFormName, GameObject prefab, Transform parent, UIParam uiParam)
        {
            if (prefab == null)
            {
                if (dicAllUIForms.ContainsKey(uiFormName)) dicAllUIForms.Remove(uiFormName);
                Debug.LogWarning(GetType() + "/LoadUIForm()/ load ui error! uiFormName:" + uiFormName);
                return;
            }

            GameObject item = Instantiate(prefab, parent);
            item.name = uiFormName;

            UIFormItem uIFormItem = item.GetComponent<UIFormItem>();
            if (uIFormItem == null) uIFormItem = item.AddComponent<UIFormItem>();
            uIFormItem.CurrentUIParam = uiParam;
            dicAllUIForms[uiFormName] = uIFormItem;

            Type type = Type.GetType(uiFormName);
            if (type != null && item.GetComponent<BaseUIForm>() == null) { item.AddComponent(type); }

            BaseUIForm baseUIForm = item.GetComponent<BaseUIForm>();
            if (baseUIForm != null) baseUIForm.OnOpenUI();

            item.SetActive(true);
            if (!dicOpenUIForms.ContainsKey(uiFormName))
            {
                dicOpenUIForms.Add(uiFormName, uIFormItem);
            }
        }

        #endregion

    }
}