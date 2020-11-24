using UnityEngine;
using System.Collections.Generic;
using Mx.Utils;
using System;

namespace Mx.Config
{
    [Serializable]
	public class LanguageConfigData
	{
		public string En;
		public string Cn;
		public string[] Scene;
	}

	public class LanguageConfigDatabase:IDatabase
	{
		public const uint TYPE_ID = 2;
		public const string DATA_PATH = "LanguageConfig";
       
		private string[][] m_datas;
        private Dictionary<string, LanguageConfigData> dicData = new Dictionary<string, LanguageConfigData>();
        private List<LanguageConfigData> listData = new List<LanguageConfigData>();

		public LanguageConfigDatabase(){}

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
                LanguageConfigData m_tempData = new LanguageConfigData();
			    m_tempData.En = m_datas[cnt][0];
		m_tempData.Cn = m_datas[cnt][1];
		m_tempData.Scene = CSVConverter.ConvertToArray<string>(m_datas[cnt][2]);
                if(!dicData.ContainsKey(m_datas[cnt][0]))
                {
                    dicData.Add(m_datas[cnt][0], m_tempData);
                    listData.Add(m_tempData);
                }
			}
		}

        public LanguageConfigData GetDataByKey(string key)
        {
            LanguageConfigData data;
            dicData.TryGetValue(key, out data);
            return data;
        }

		public int GetCount()
		{
			return listData.Count;
		}

        public List <LanguageConfigData> GetAllData()
        {
            return listData;
        }

	}
}
