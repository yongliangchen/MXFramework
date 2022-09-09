using UnityEngine;
using System;

namespace Mx.Log
{
    public sealed class DebugDefine
    {
        /// <summary>UTP端口号</summary>
        public const int UTP_PORT = 9621;
    }

    [Serializable]
    public class DebugData
    {
        /// <summary>日记编号</summary>
        public string ID;
        /// <summary>日记数量</summary>
        public int Count = 1;
        /// <summary>日记内容</summary>
        public string Condition;
        /// <summary>堆栈跟踪</summary>
        public string StackTrace;
        /// <summary>日记类型</summary>
        public LogType Type;
        /// <summary>日记时间</summary>
        public string Tiem;
        /// <summary>设备型号</summary>
        public string DeviceModel;
    }
}