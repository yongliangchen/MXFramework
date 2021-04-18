using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using UnityEngine;

namespace Mx.Net
{
    public class DownloadFileHandler : DownloadHandlerScript
    {
        private string m_SavePath;
        private string m_FileName;
        private string m_PathName;
        private string m_Postfix = ".temp";
        private bool m_Isdown;

        private int m_SumLength;
        /// <summary>文件总长度</summary>
        public int SumLength { get { return m_SumLength; } }

        private int m_NowLength;
        /// <summary>已下载长度</summary>
        public int NowLength { get { return m_NowLength; } }

        /// <summary>下载进度</summary>
        public float DownloadProgress
        {
            get { return (SumLength == 0) ? 0 : (float)NowLength / SumLength; }
        }

        private new bool isDone = false;
        /// <summary>是否下载完成</summary>
        public bool IsDone { get { return isDone; } }

        private List<byte> m_Datas = new List<byte>();
        /// <summary>下载的数据</summary>
        public byte[] DownloadDatas { get { return m_Datas.ToArray(); } }

        public DownloadFileHandler(string savePath, string fileName, string version) : base(new byte[1024 * 200])//限制下载的长度
        {
            this.m_SavePath = savePath;
            this.m_FileName = fileName;

            m_PathName = savePath + "/" + erasePostfix(fileName) + "_" + version + m_Postfix;

            m_Isdown = true;
            m_NowLength = (int)GetFileLength(m_PathName);
        }

        public static long GetFileLength(string Path_Name)
        {
            if (!File.Exists(Path_Name))
                return 0;
            FileStream fs = File.OpenWrite(Path_Name);
            long ength = fs.Length;
            fs.Close();
            return ength;
        }

        public DownloadFileHandler()
        {
            m_Isdown = false;
            m_NowLength = 0;
        }

        [System.Obsolete]
        protected override void ReceiveContentLength(int contentLength)
        {
            m_SumLength = contentLength + NowLength;
        }

        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            if (!m_Isdown)
                return false;

            for (int i = 0; i < dataLength; i++)
            {
                m_Datas.Add(data[i]);
            }
            m_NowLength += dataLength;

            writeFile(m_PathName, data, dataLength);

            return true;
        }

        protected override void CompleteContent()
        {
            changeName();
            isDone = true;
        }

        private void writeFile(string Path_Name, byte[] dates, int length)
        {
            FileStream fs;
            if (!File.Exists(Path_Name))
                fs = File.Create(Path_Name);
            else
                fs = File.OpenWrite(Path_Name);
            long ength = fs.Length;

            fs.Seek(ength, SeekOrigin.Current);
            fs.Write(dates, 0, length);
            fs.Flush();

            fs.Close();
        }

        private void changeName()
        {
            string filepathName = m_SavePath + "/" + m_FileName;
            if (File.Exists(filepathName))
                File.Delete(filepathName);
            File.Move(m_PathName, filepathName);
        }

        /// <summary> 去掉‘.’后面的字符</summary>
        private string erasePostfix(string filePath)
        {
            if (filePath.LastIndexOf('.') < 0) return filePath;
            return filePath.Substring(0, filePath.LastIndexOf('.'));
        }
    }
}
