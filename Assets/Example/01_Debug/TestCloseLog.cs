using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.Log;
using UnityEngine.UI;

namespace Mx.Example
{
    /// <summary>测试关闭全部日记输出功能</summary>
    public class TestCloseLog : MonoBehaviour
    {
        private Text m_ButtonText;
        private bool m_IsOpenDebug = true;

        private void Awake()
        {
            m_ButtonText = GameObject.Find("Button/Text").GetComponent<Text>();

            InvokeRepeating("PrintLog", 0.2f, 0.2f);
        }

        public void OpenOrCloseLog()
        {
            m_IsOpenDebug = !m_IsOpenDebug;

            if(m_IsOpenDebug)
            {
                DebugManager.Instance.OpenDebug();
                m_ButtonText.text = "关闭日记输出";
            }
            else
            {
                DebugManager.Instance.CloseDebug();
                m_ButtonText.text = "打开日记输出";
            }
        }

        private void PrintLog()
        {
            int index1 = Random.Range(0, 3);
            int index2 = Random.Range(0, 10);

            if (index1 == 0)
            {
                Debug.Log(GetType() + "/PrintLog()/" + "请前往RemoteDebug工具查看Logo" + index2);
            }
            else if (index1 == 1)
            {
                Debug.LogWarning(GetType() + "/PrintLog()/" + "请前往RemoteDebug工具查看Logo" + index2);
            }
            else
            {
                Debug.LogError(GetType() + "/PrintLog()/" + "请前往RemoteDebug工具查看Logo" + index2);
            }
        }
    }
}