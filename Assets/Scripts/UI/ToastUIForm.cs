using System.Collections.Generic;
using Mx.Res;
using Mx.UI;
using Mx.Utils;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 吐司UI面板 </summary>
public class ToastUIForm : BaseUIForm
{
    private List<GameObject> m_ListToast = new List<GameObject>();

    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void OnRelease()
    {
        base.OnRelease();
    }

    public override void OnCloseUIEvent()
    {
        base.OnCloseUIEvent();

        if (m_ListToast == null || m_ListToast.Count < 1) return;
        for (int i = m_ListToast.Count - 1; i >= 0; i--)
        {
            m_ListToast[i].SetActive(false);
        }

        m_ListToast.Clear();
    }

    /// <summary>当前UI窗体消息事件监听</summary>
    public override void OnCurrentUIFormMsgEvent(string key, object values)
    {
        base.OnCurrentUIFormMsgEvent(key, values);

        if (key.Equals(UIDefine.TOAST_INFO_MSG)) createToast((ToastMsgInfo)values);
    }

    /// <summary>创建Toast对象</summary>
    private void createToast(ToastMsgInfo info)
    {
        if (info == null) return;
        GameObject item = Instantiate(ResoucesMgr.Instance.Load<GameObject>(info.Prefab, true),transform);
        item.SetActive(true);
        if (item.transform.Find("Content") != null)
        {
            item.transform.Find("Content").GetComponent<Text>().text = info.Content;

            if(item.transform.Find("Content").GetComponent<ContentSizeFitter>()!=null)
            item.transform.Find("Content").GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
        }

        if (item.GetComponent<ContentSizeFitter>()!=null)
        item.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();

        m_ListToast.Add(item);

        Timer.CreateTimer("Toast_Timer").StartTiming(info.ShowTime, () =>
        {
            if (item != null) Destroy(item);
            m_ListToast.Remove(item);
        });
    }
}

