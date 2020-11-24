using UnityEngine;
using System.Collections.Generic;
using Mx.Utils;
using System;
using System.IO;

namespace Mx.Config
{
    [Serializable]
	public class TestIOCsvConfigData
	{
		public int Id;
		public string Name;
		public int Age;
		public float Score;
		public string[] Designation;
	}

	public class TestIOCsvConfigDatabase:IDatabase
	{
		public const uint TYPE_ID = 1;
		public const string DATA_PATH = "TestIOCsvConfig";
       
		private string[][] m_datas;
        private Dictionary<string, TestIOCsvConfigData> dicData = new Dictionary<string, TestIOCsvConfigData>();
        private List<TestIOCsvConfigData> listData = new List<TestIOCsvConfigData>();

		public TestIOCsvConfigDatabase(){}

		public uint TypeID()
		{
			return TYPE_ID;
		}

		public string DataPath()
		{
			return ConfigDefine.GetStreamingConfigOutPath+DATA_PATH+".txt";
		}

        public void Load()
        {
          dicData.Clear();
          listData.Clear();

          StreamReader streamReader = null;
          if (File.Exists(DataPath())) streamReader = File.OpenText(DataPath());
          else {
                 Debug.LogError(GetType() + "/Load() load config eroor!  path:" + DataPath());
                 return;
              }

          string str = streamReader.ReadToEnd();
          streamReader.Close();
          streamReader.Dispose();

          string textData = StringEncrypt.DecryptDES(str);
          m_datas = CSVConverter.SerializeCSVData(textData);
          Serialization();

        }

		private void Serialization()
		{
			for(int cnt = 0; cnt < m_datas.Length; cnt++)
			{
                TestIOCsvConfigData m_tempData = new TestIOCsvConfigData();
			    
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

        public TestIOCsvConfigData GetDataByKey(string key)
        {
            TestIOCsvConfigData data;
            dicData.TryGetValue(key, out data);
            return data;
        }

		public int GetCount()
		{
			return listData.Count;
		}

        public List <TestIOCsvConfigData> GetAllData()
        {
            return listData;
        }

	}
}
