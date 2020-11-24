/*代码自动生成的类，请勿手动修改*/

using UnityEngine;
using System.Collections.Generic;
using Mx.Utils;
using System;

namespace Mx.Config
{
    [Serializable]
	public class CnConfigData
	{
		public string Id;
		public string Value;
		public string[] Scene;
		public string Des;
	}

    [Serializable]
    public class CnConfigDatas
    {
        public CnConfigData[] content;
    }

	public partial class CnConfigDatabase:IDatabase
	{
		public const uint TYPE_ID = 2;
		public const string DATA_PATH = "CnConfig";
       
		private string[][] m_datas;
        private Dictionary<string, CnConfigData> dicData = new Dictionary<string, CnConfigData>();
        private List<CnConfigData> listData = new List<CnConfigData>();
        private CnConfigDatas configDatas;
		public CnConfigDatabase(){}

		public uint TypeID()
		{
			return TYPE_ID;
		}

		public string DataPath()
		{
			return ConfigDefine.GetResoucesConfigOutPath + DATA_PATH;
		}

        public void Load()
        {
          configDatas = new CnConfigDatas();
          dicData.Clear();
          listData.Clear();

           TextAsset textAsset = Resources.Load<TextAsset>(DataPath());
           string str = textAsset.text;
           if (string.IsNullOrEmpty(str))
           {
               Debug.LogError(GetType() + "/Load()/ load config error! path:" + DataPath());
           }
         
          string textData = StringEncrypt.DecryptDES(str);
          m_datas = CSVConverter.SerializeCSVData(textData);
          Serialization();

        }

		private void Serialization()
		{
			for(int cnt = 0; cnt < m_datas.Length; cnt++)
			{
                CnConfigData m_tempData = new CnConfigData();
			    m_tempData.Id = m_datas[cnt][0];
		m_tempData.Value = m_datas[cnt][1];
		m_tempData.Scene = CSVConverter.ConvertToArray<string>(m_datas[cnt][2]);
		m_tempData.Des = m_datas[cnt][3];
                if(!dicData.ContainsKey(m_datas[cnt][0]))
                {
                    dicData.Add(m_datas[cnt][0], m_tempData);
                    listData.Add(m_tempData);
                }
			}
		}

        public CnConfigData GetDataByKey(string key)
        {
            CnConfigData data;
            dicData.TryGetValue(key, out data);
            return data;
        }

        public string GetJsonStringBykey(string key)
        {
            string data = string.Empty;
            CnConfigData jsonData = GetDataByKey(key);
            if (data != null) data = JsonUtility.ToJson(jsonData);
            return data;
        }

        public string GetAllJsonString()
        {
            configDatas.content = GetAllDataArray();

            string datas = string.Empty;
            if (listData!= null) datas = JsonUtility.ToJson(configDatas);
            return datas;
        }

		public int GetCount()
		{
			return listData.Count;
		}

        public List <CnConfigData> GetAllDataList()
        {
            return listData;
        }

        public CnConfigData[] GetAllDataArray()
        {
            return listData.ToArray();
        }

	}
}
