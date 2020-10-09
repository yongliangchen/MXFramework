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
        public string ID;
        public int Count = 1;
        public string Condition;
        public string StackTrace;
        public LogType Type;
        public string Tiem;
    }
}