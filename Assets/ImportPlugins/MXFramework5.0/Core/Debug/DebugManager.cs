/***
 * 
 *    Title: MXFramework
 *           主题: 日记管理
 *    Description: 
 *           功能：1.打开和关闭日记功能
 *                2.远程调试功能
 *                3.将错误数据保存到本地
 *                
 *    Date: 2020
 *    Version: v5.0版本
 *    Modify Recoder: 
 *      
 */

using System;
using System.IO;
using Mx.Utils;
using UnityEngine;
using Mx.Net;

namespace Mx.Log
{
    /// <summary>管理日记的开启和关闭</summary>
    public class DebugManager : MonoSingleton<DebugManager>
    {
        #region 数据声明

        private bool m_IsRemoteDebug = false;
        private UdpClient m_Client;

        #endregion

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Application.logMessageReceived += HandleLog;
            Debug.Log(GetType() + "Awake()/debug out Path:" + Application.persistentDataPath + "/Debug/");
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
            m_IsRemoteDebug = true;

            m_Client = GetComponent<UdpClient>();
            if(m_Client==null)
            {
                m_Client = gameObject.AddComponent<UdpClient>();
            }
            m_Client.OpenClient(DebugDefine.UTP_PORT);
        }

        /// <summary>日记回调</summary>
        private void HandleLog(string condition, string stackTrace, LogType type)
        {
            string time = DateTime.Now.ToString("yyy-MM-dd HH:mm");

            if (type == LogType.Error || type == LogType.Exception)
            {
                string message = string.Format("Error:{0}\nTime:{1}\n{2}", condition, time, stackTrace);
                SaveErrorLog(message);
            }

            DebugData debugData = new DebugData();
            debugData.ID = stackTrace + TimeUtils.GetTimeStampToMilliseconds();
            debugData.Condition = condition;
            debugData.StackTrace = stackTrace;
            debugData.Type = type;
            debugData.Tiem = time;

           if(m_IsRemoteDebug) SedLogData(debugData);
        }

        /// <summary>保存错误信息</summary>
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

        /// <summary>将日记数写入本地文本</summary>
        private static void WriteData(string textPath, string message)
        {
            if (string.IsNullOrEmpty(textPath) || string.IsNullOrEmpty(message)) return;

            StreamWriter sw = null;
            if (!File.Exists(textPath)) sw = File.CreateText(textPath);
            else sw = File.AppendText(textPath);

            sw.WriteLine(message + '\n');

            sw.Close();
            sw.Dispose();
        }

        /// <summary>发送日记数据</summary>
        private void SedLogData(DebugData debugData)
        {
            string msg = JsonUtility.ToJson(debugData);
            if (m_Client != null) m_Client.SendMsg(msg);
        }
    }
}
