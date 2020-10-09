using UnityEngine;
using Mx.Util;
using System.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Mx.Log
{
    /// <summary>管理日记的开启和关闭</summary>
    public class DebugManager : MonoSingleton<DebugManager>
    {
        #region 数据声明
       
        private static Socket socket;
        private static IPEndPoint iPEndPoint;
        private byte[] data;

        #endregion

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>关闭所有级别日记输出（建议项目正式上线的时候使用，以节省性能）</summary>
        public void CloseDebug()
        {
            UnityEngine.Debug.unityLogger.logEnabled = false;
        }

        /// <summary>打开调试功能(默认是开启的,正式上线的时候建议关闭)</summary>
        public void OpenDebug()
        {
            UnityEngine.Debug.unityLogger.logEnabled = true;
        }

        /// <summary>打开远程调试（项目正式上线的时候不建议打开）</summary>
        public void RemoteDebug()
        {
            Application.logMessageReceived += HandleLog;
            InitSocket();
            Debug.Log(GetType() + "Awake()/ open RemoteDebug, outPath:" + Application.persistentDataPath + "/Debug/");
        }

        private void HandleLog(string condition, string stackTrace, LogType type)
        {
            string time = DateTime.Now.ToString("yyy-MM-dd HH:mm");

            if (type == LogType.Error || type == LogType.Exception)
            {
                string message = string.Format("Error:{0}\nTime:{1}\n{2}", condition, time, stackTrace);
                SaveErrorLog(message);
            }

            DebugData debugData = new DebugData();
            debugData.ID = stackTrace + GetTimeStamp();
            debugData.Condition = condition;
            debugData.StackTrace = stackTrace;
            debugData.Type = type;
            debugData.Tiem = time;

            SedLogData(debugData);
        }

        private static void SaveErrorLog(string message)
        {
            string fileDif = Application.persistentDataPath + "/Debug/";

            if (!Directory.Exists(fileDif))
            {
                Directory.CreateDirectory(fileDif);
            }

            string filePath = fileDif + DateTime.Now.ToString("yyy-MM-dd") + ".txt";
            WriteData(filePath, message);
        }

        private static  void WriteData(string textPath, string message)
        {
            if (string.IsNullOrEmpty(textPath) || string.IsNullOrEmpty(message)) return;

            StreamWriter sw = null;
            if (!File.Exists(textPath)) sw = File.CreateText(textPath);
            else sw = File.AppendText(textPath);

            sw.WriteLine(message + '\n');

            sw.Close();
            sw.Dispose();
        }

        private void InitSocket()
        {
            iPEndPoint = new IPEndPoint(IPAddress.Broadcast, DebugDefine.UTP_PORT);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
        }

        private void SedLogData(DebugData debugData)
        {
            string sendStr = JsonUtility.ToJson(debugData);
            try
            {
                data = new byte[Encoding.UTF8.GetBytes(sendStr).Length];
                data = Encoding.UTF8.GetBytes(sendStr);
                socket.SendTo(data, data.Length, SocketFlags.None, iPEndPoint);
            }
            catch
            {
                Debug.LogWarning(GetType() + "/SedLogData() send debug errror!");
            }
        }

        private long GetTimeStamp(bool bflag = false)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long ret;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds);
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds);
            return ret;
        }

    }
}