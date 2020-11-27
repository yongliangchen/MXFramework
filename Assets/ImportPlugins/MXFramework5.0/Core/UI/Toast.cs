using UnityEngine;

namespace Mx.UI
{
    public class Toast
    {
        public static void Show(string content, float showTime = 2)
        {
            ToastMsgInfo info = new ToastMsgInfo { Content = content, ShowTime = showTime };
            sendMsgToToastUIForm(info);
        }

        public static void Show(string content, Vector3 position, float showTime = 2)
        {
            ToastMsgInfo info = new ToastMsgInfo { Content = content, Position = position, ShowTime = showTime };
            sendMsgToToastUIForm(info);
        }

        public static void Show(string content, string prefab, float showTime = 2)
        {
            ToastMsgInfo info = new ToastMsgInfo { Content = content, Prefab = prefab, ShowTime = showTime };
            sendMsgToToastUIForm(info);
        }

        public static void Show(string content, string prefab, Vector3 position, float showTime = 2)
        {
            ToastMsgInfo info = new ToastMsgInfo { Content = content, Prefab = prefab, Position = position, ShowTime = showTime };
            sendMsgToToastUIForm(info);
        }

        private static void sendMsgToToastUIForm(ToastMsgInfo info)
        {
            if (!UIManager.Instance.IsOpen(UIFormNames.TOAST_UIFORM)) UIManager.Instance.OpenUIForms(UIFormNames.TOAST_UIFORM);
            UIManager.Instance.SendMessageToUIForm(UIFormNames.TOAST_UIFORM, UIDefine.TOAST_INFO_MSG, info);
        }
    }

    public class ToastMsgInfo
    {
        public string Content { get; set; }
        public string Prefab { get; set; } = "UIPrefabs/Toast/Item01";
        public Vector3 Position { get; set; } = new Vector3(0,100,0);
        public float ShowTime { get; set; } = 2f;
        //public int AnimationMode { get; set; }
    }
}