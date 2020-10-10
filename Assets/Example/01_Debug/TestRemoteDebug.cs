using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.Log;

namespace Mx.Example
{
    /// <summary>测试远程调试功能</summary>
    public class TestRemoteDebug : MonoBehaviour
    {
        private void Awake()
        {
            DebugManager.Instance.RemoteDebug();
           

            InvokeRepeating("PrintLog", 0.2f, 0.2f);
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