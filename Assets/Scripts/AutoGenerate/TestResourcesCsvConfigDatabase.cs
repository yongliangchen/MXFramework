using UnityEngine;
using System.Collections.Generic;
using Mx.Util;
using System;

namespace Mx.Config
{
    [Serializable]
	public class TestResourcesCsvConfigData
	{
		public int Id;
		public string Name;
		public int Age;
		public float Score;
		public string[] Designation;
	}

	public class TestResourcesCsvConfigDatabase:IDatabase
	{
		public const uint TYPE_ID = 2;
		public const string DATA_PATH = "TestResourcesCsvConfig";
       
		private string[][] m_datas;
        private Dictionary<string, TestResourcesCsvConfigData> dicData = new Dictionary<string, TestResourcesCsvConfigData>();
        private List<TestResourcesCsvConfigData> listData = new List<TestResourcesCsvConfigData>();

		public TestResourcesCsvConfigDatabase(){}

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
                TestResourcesCsvConfigData m_tempData = new TestResourcesCsvConfigData();
			    
			if(!int.TryParse(m_datas[cnt][0], out m_tempData.Id))
			{
				m_tempData.Id = 0;
			}

		m_tempData.Name = m_datas[cnt][1];
		
			if(!int.TryParse(m_datas[cnt][2], out m_tempData.Age))
			{
				m_tempData.Age = 0;
			}

		
			if(!float.TryParse(m_datas[cnt][3], out m_tempData.Score))
			{
				m_tempData.Score = 0.0f;
			}

		m_tempData.Designation = CSVConverter.ConvertToArray<string>(m_datas[cnt][4]);
                if(!dicData.ContainsKey(m_datas[cnt][0]))
                {
                    dicData.Add(m_datas[cnt][0], m_tempData);
                    listData.Add(m_tempData);
                }
			}
		}

        public TestResourcesCsvConfigData GetDataByKey(string key)
        {
            TestResourcesCsvConfigData data;
            dicData.TryGetValue(key, out data);
            return data;
        }

		public int GetCount()
		{
			return listData.Count;
		}

        public List <TestResourcesCsvConfigData> GetAllData()
        {
            return listData;
        }

	}
}
