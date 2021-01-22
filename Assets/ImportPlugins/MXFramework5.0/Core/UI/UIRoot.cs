/***
 * 
 *    Title: MXFramework
 *           主题: UI根节点管理
 *    Description: 
 *           功能：1.负责生成UI根目录
 *                2.负责生成UI层级目录
 *                3.负责生成UI相机
 *                4.负责生成EventSystem
 *                                  
 *    Date: 2021
 *    Version: v5.1版本
 *    Modify Recoder:      
 */

using Mx.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace Mx.UI
{
    /// <summary>UI根节点管理</summary>
    public class UIRoot : MonoSingleton<UIRoot>
    {
        private int uiLayer;
        public GameObject uiRootObject { get; private set; }
        public Camera uICamera { get; private set; }
        public Transform[] uiFormDepthArr { get; private set; }

        private Transform scriptsParent;

        private void Awake()
        {
            uiLayer = LayerMask.NameToLayer(UIDefine.LAYER_UI);
            uiRootObject = transform.gameObject;

            scriptsParent = createScriptsParent(uiRootObject.transform).transform;
            uICamera = createUICamera(uiRootObject.transform);
            setUIRootParam();
            createEventSystem(uiRootObject.transform);
            createUIFormDepth();
            addScripts();

            this.gameObject.transform.SetParent(scriptsParent, false);
            DontDestroyOnLoad(uiRootObject);//加载场景的时候不销毁
        }

        /// <summary>设置UIRoot参数</summary>
        private void setUIRootParam()
        {
            uiRootObject.name = UIDefine.NAME_UIROOT;
            uiRootObject.gameObject.layer = uiLayer;
            resetObject(uiRootObject);

            if(uiRootObject.GetComponent<Canvas>()==null) uiRootObject.gameObject.AddComponent<Canvas>();
            if (uiRootObject.GetComponent<CanvasScaler>() == null) uiRootObject.AddComponent<CanvasScaler>();
            if (uiRootObject.GetComponent<GraphicRaycaster>() == null) uiRootObject.AddComponent<GraphicRaycaster>();

            Canvas canvas = uiRootObject.GetComponent<Canvas>();
            CanvasScaler canvasScaler= uiRootObject.GetComponent<CanvasScaler>();
            GraphicRaycaster graphicRaycaster= uiRootObject.GetComponent<GraphicRaycaster>();

            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = uICamera;

            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            //说明是横屏
            if(Screen.width> Screen.height)
            {
                canvasScaler.referenceResolution = new Vector2(UIDefine.UI_ORIFINAL_WIDTH, UIDefine.UI_ORIFINAL_HEIGHT);
            }
            //说明是竖屏
            else
            {
                canvasScaler.referenceResolution = new Vector2(UIDefine.UI_ORIFINAL_HEIGHT,UIDefine.UI_ORIFINAL_WIDTH);
            }

            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            canvasScaler.referencePixelsPerUnit = 100;
        }

        /// <summary>创建存放脚本的根目录</summary>
        private GameObject createScriptsParent(Transform parent)
        {
            GameObject obj = createObject(UIDefine.SCRIPTSLMANAGER_MODE, parent, uiLayer);
            return obj;
        }

        /// <summary>创建UI相机</summary>
        private Camera createUICamera(Transform parent)
        {
            Camera uiCamera=null;
            GameObject obj = createObject("UICamera", parent, uiLayer);

            if (obj.GetComponent<Camera>() == null) obj.AddComponent<Camera>();
            uiCamera = obj.GetComponent<Camera>();

            obj.transform.localPosition = new Vector3(0, 0, -1000);

            uiCamera.clearFlags = CameraClearFlags.Depth;
            uiCamera.cullingMask = 1 << uiLayer;
            uiCamera.orthographic = true;
            uiCamera.orthographicSize = 5;
            uiCamera.depth = 100;

            return uiCamera;
        }

        /// <summary>创建EventSystem</summary>
        private void createEventSystem(Transform parent)
        {
            GameObject obj = createObject("EventSystem", parent, uiLayer);

            if (obj.GetComponent<UnityEngine.EventSystems.EventSystem>() == null)
                obj.AddComponent<UnityEngine.EventSystems.EventSystem>();

            if (obj.GetComponent<UnityEngine.EventSystems.StandaloneInputModule>() == null)
                obj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        /// <summary>创建UI层级节点</summary>
        private void createUIFormDepth()
        {
            EnumUIFormDepth enumUIForm = EnumUIFormDepth.Normal;
            string[] values = System.Enum.GetNames(enumUIForm.GetType());
            uiFormDepthArr = new Transform[values.Length];

            for(int i=0;i< values.Length;i++)
            {
                GameObject obj = createObject(values[i], uiRootObject.transform, uiLayer);
                uiFormDepthArr[i] = obj.transform;
            }
        }

        /// <summary>添加管理脚本</summary>
        private void addScripts()
        {
            UIControl uIControl = FindObjectOfType<UIControl>();
            if (uIControl == null)
            {
                GameObject obj = new GameObject("_UIControl");
                uIControl = obj.AddComponent<UIControl>();
            }
            uIControl.gameObject.transform.SetParent(scriptsParent);
            resetObject(uIControl.gameObject);

            //创建UI管理脚本
        }

        /// <summary>创建空对象</summary>
        private GameObject createObject(string objectNeme, Transform parent, int layer)
        {
            GameObject obj = null;

            if (uiRootObject.transform.Find(objectNeme) != null)
            {
                obj = uiRootObject.transform.Find(objectNeme).gameObject;
            }
            else
            {
                obj = new GameObject(objectNeme);
                obj.transform.SetParent(parent);
            }

            resetObject(obj);
            obj.gameObject.layer = layer;

            return obj;
        }

        /// <summary>重置对象（位置归零、旋转归零、缩放归一）</summary>
        private void resetObject(GameObject obj)
        {
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;
            obj.transform.localScale = Vector3.one;
        }

    }
}