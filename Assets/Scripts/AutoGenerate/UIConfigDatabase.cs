/*代码自动生成的类，请勿手动修改*/

using UnityEngine;
using System.Collections.Generic;
using Mx.Utils;
using System;

namespace Mx.Config
{
    [Serializable]
	public class UIConfigData
	{
		public string Name;
		public int LandType;
		public int UIFormsDepth;
		public int UIFormShowMode;
		public string ResourcesPath;
		public string AssetBundlePath;
		public string AssetName;
		public string Des;
	}

    [Serializable]
    public class UIConfigDatas
    {
        public UIConfigData[] content;
    }

	public partial class UIConfigDatabase:IDatabase
	{
		public const uint TYPE_ID = 4;
		public const string DATA_PATH = "UIConfig";
       
		private string[][] m_datas;
        private Dictionary<string, UIConfigData> dicData = new Dictionary<string, UIConfigData>();
        private List<UIConfigData> listData = new List<UIConfigData>();
        private UIConfigDatas configDatas;
		public UIConfigDatabase(){}

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
          configDatas = new UIConfigDatas();
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
                UIConfigData m_tempData = new UIConfigData();
			    m_tempData.Name = m_datas[cnt][0];
		
			if(!int.TryParse(m_datas[cnt][1], out m_tempData.LandType))
			{
				m_tempData.LandType = 0;
			}

		
			if(!int.TryParse(m_datas[cnt][2], out m_tempData.UIFormsDepth))
			{
				m_tempData.UIFormsDepth = 0;
			}

		
			if(!int.TryParse(m_datas[cnt][3], out m_tempData.UIFormShowMode))
			{
				m_tempData.UIFormShowMode = 0;
			}

		m_tempData.ResourcesPath = m_datas[cnt][4];
		m_tempData.AssetBundlePath = m_datas[cnt][5];
		m_tempData.AssetName = m_datas[cnt][6];
		m_tempData.Des = m_datas[cnt][7];
                if(!dicData.ContainsKey(m_datas[cnt][0]))
                {
                    dicData.Add(m_datas[cnt][0], m_tempData);
                    listData.Add(m_tempData);
                }
			}
		}

        public UIConfigData GetDataByKey(string key)
        {
            UIConfigData data;
            dicData.TryGetValue(key, out data);
            return data;
        }

        public string GetJsonStringBykey(string key)
        {
            string data = string.Empty;
            UIConfigData jsonData = GetDataByKey(key);
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

        public List <UIConfigData> GetAllDataList()
        {
            return listData;
        }

        public UIConfigData[] GetAllDataArray()
        {
            return listData.ToArray();
        }

	}
}
