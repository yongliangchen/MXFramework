using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.Collections;

namespace Mx.Net
{
    /// <summary>UDP通信服务器</summary>
    public  class UdpServer : MonoBehaviour
    {
        private Socket m_Socket;
        private EndPoint m_EndPoint;
        private IPEndPoint m_IpEnd;
        private byte[] m_RecvData = new byte[1024];
        private byte[] m_SendData = new byte[1024];
        private Thread m_Thread;
        private Queue m_MsgQueue;
        public Action<byte[]> onReceiveEvent = null;

        private void Awake()
        {
            m_MsgQueue = new Queue();
        }

        private void OnDestroy()
        {
            Close();
        }

        private void Update()
        {
            if (m_MsgQueue.Count != 0)
            {
                OnReceiveEvent((byte[])m_MsgQueue.Dequeue());
            }
        }

        /// <summary>开启UPD服务器</summary>
        public void OpenServer(int port)
        {
            m_IpEnd = new IPEndPoint(IPAddress.Any, port);
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            m_Socket.Bind(m_IpEnd);
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            m_EndPoint = (EndPoint)sender;
            m_Thread = new Thread(new ThreadStart(SocketReceive));
            m_Thread.Start();
        }

        /// <summary>发送消息</summary>
        public void SendMsg(string msg)
        {
            if (m_Socket == null||string.IsNullOrEmpty(msg)) return;

            m_SendData = new byte[1024];
            m_SendData = Encoding.ASCII.GetBytes(msg);
            m_Socket.SendTo(m_SendData, m_SendData.Length, SocketFlags.None, m_EndPoint);
        }

        /// <summary>发送Bytes数据</summary>
        public void SendBytes(byte[] data)
        {
            if (m_Socket == null|| data==null) return;

            m_Socket.SendTo(data, data.Length, SocketFlags.None, m_EndPoint);
        }

        /// <summary>接收消息事件</summary>
        private void OnReceiveEvent(byte[] data)
        {
            if (onReceiveEvent != null) onReceiveEvent(data);

            string recvStr = Encoding.ASCII.GetString(data);
            Debug.Log(GetType() + "/Receive()/接收到的消息：" + recvStr);
        }

        /// <summary>接收消息</summary>
        private void SocketReceive()
        {
            while (true)
            {
                m_RecvData = new byte[1024];
                int length = m_Socket.ReceiveFrom(m_RecvData, ref m_EndPoint);
                byte[] data = new byte[length];
                Array.Copy(m_RecvData, data, length);
                m_MsgQueue.Enqueue(data);
            }
        }

        /// <summary>关闭连接</summary>
        private void Close()
        {
            if (m_Thread != null)
            {
                m_Thread.Interrupt();
                m_Thread.Abort();
            }

            if (m_Socket != null) m_Socket.Close();
        }

    }
}