using System;
using System.IO;
using Mx.Config;
using Mx.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Mx.Example
{
    /// <summary>测试加载外部数据</summary>
    public class TestIOCsvConfig : MonoBehaviour
    {
        private ContentSizeFitter m_ContentSizeFitter;
        private TestIOCsvConfigDatabase m_Database;
        private GameObject m_Prefab;
        private Transform m_Parent;

        private void Awake()
        {
            m_Prefab = transform.Find("Items/Item").gameObject;
            m_Prefab.SetActive(false);
            m_Parent = transform.Find("Items");
            m_ContentSizeFitter = m_Parent.GetComponent<ContentSizeFitter>();

            string inPath = Application.streamingAssetsPath + "/Config/TestIOCsvConfig.txt";
            string outPath = Application.persistentDataPath + "/Config/TestIOCsvConfig.txt";

            Debug.Log(GetType() + "/Awake()/outPath:" + outPath);

            CopyFile(inPath, outPath, () =>
            {
                m_Database = new TestIOCsvConfigDatabase();
                m_Database.Load();

                Create();
            });
        }

        private void Create()
        {
            if (m_Database == null || m_Database.GetAllData() == null || m_Database.GetAllData().Count == 0)
            {
                Debug.LogWarning(GetType() + "/Create()/ data is null!");
                return;
            }

            for (int i = 0; i < m_Database.GetAllData().Count; i++)
            {
                TestIOCsvConfigData data = m_Database.GetAllData()[i];

                GameObject item = Instantiate(m_Prefab, m_Parent);
                item.SetActive(true);
                item.name = data.Id.ToString();

                item.transform.Find("Id").GetComponent<Text>().text = data.Id.ToString();
                item.transform.Find("Name").GetComponent<Text>().text = data.Name;
                item.transform.Find("Age").GetComponent<Text>().text = data.Age.ToString();
                item.transform.Find("Score").GetComponent<Text>().text = data.Score.ToString();

                Text Designation = item.transform.Find("Designation").GetComponent<Text>();
                Designation.text = null;
                for (int j = 0; j < data.Designation.Length; j++)
                {
                    Designation.text += data.Designation[j] + ";";
                }

            }

            m_ContentSizeFitter.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_ContentSizeFitter.GetComponent<RectTransform>());
        }

        private void CopyFile(string inPath,string outPath, Action finish)
        {
            if (!File.Exists(outPath.Replace("file://", null)))
            {
                UnityWebRequestMgr.Instance.CopyFile(inPath, outPath, null, (unityWeb) =>
                {
                    if (!string.IsNullOrEmpty(unityWeb.error))
                    {
                        Debug.LogWarning(GetType() + "/CopyFile()/ copy error! " + unityWeb.error);
                    }
                    else { if (finish != null) finish(); }
                });
            }
            else { if (finish != null) finish(); }
        }
    }
}