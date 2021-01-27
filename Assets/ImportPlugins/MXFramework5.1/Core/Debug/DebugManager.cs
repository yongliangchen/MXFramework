using System;
using System.IO;
using Mx.Utils;
using UnityEngine;

namespace Mx.Log
{
    /// <summary>>管理日记</summary>
    public class DebugManager : MonoSingleton<DebugManager>
    {
        /// <summary>日记时间</summary>
        public Action<DebugData> onLogEvent = null;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Application.logMessageReceived += onHandleLog;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= onHandleLog;
        }

        /// <summary>打开调试功能(默认是开启的,正式上线的时候建议关闭)</summary>
        public void OpenLog()
        {
            UnityEngine.Debug.unityLogger.logEnabled = true;
        }

        /// <summary>关闭所有级别日记输出（建议项目正式上线的时候使用，以节省性能）</summary>
        public void CloseLog()
        {
            UnityEngine.Debug.unityLogger.logEnabled = false;
        }

        /// <summary>打开远程调试功能</summary>
        public void OpenRemoteLog()
        {

        }

        /// <summary>关闭远程调试功能</summary>
        public void CloseRemoteLog()
        {

        }

        /// <summary>日记回调</summary>
        private void onHandleLog(string condition, string stackTrace, LogType type)
        {
            string time = DateTime.Now.ToString("yyy-MM-dd HH:mm");

            if (type == LogType.Error || type == LogType.Exception)
            {
                string message = string.Format("Error:{0}\nTime:{1}\n{2}", condition, time, stackTrace);
                saveErrorLog(message);
            }

            DebugData debugData = new DebugData();
            debugData.ID = stackTrace + TimeUtils.GetTimeStampToMilliseconds();
            debugData.Condition = condition;
            debugData.StackTrace = stackTrace;
            debugData.Type = type;
            debugData.Tiem = time;

            if (onLogEvent != null) onLogEvent(debugData);
        }

        /// <summary>保存错误信息</summary>
        private static void saveErrorLog(string message)
        {
            string fileDif = Application.persistentDataPath + "/Debug/";

            if (!Directory.Exists(fileDif))
            {
                Directory.CreateDirectory(fileDif);
            }

            string filePath = fileDif + DateTime.Now.ToString("yyy-MM-dd") + ".txt";
            writeData(filePath, message);
        }

        /// <summary>写入数据</summary>
        private static void writeData(string textPath, string message)
        {
            if (string.IsNullOrEmpty(textPath) || string.IsNullOrEmpty(message)) return;

            StreamWriter sw = null;
            if (!File.Exists(textPath)) sw = File.CreateText(textPath);
            else sw = File.AppendText(textPath);

            sw.WriteLine(message + '\n');

            sw.Close();
            sw.Dispose();
        }

    }
}