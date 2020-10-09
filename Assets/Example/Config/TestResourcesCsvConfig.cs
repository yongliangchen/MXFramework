using UnityEngine;
using Mx.Config;
using UnityEngine.UI;

namespace Mx.Example
{
    /// <summary>测试读取Resources路径下的配置表</summary>
    public class TestResourcesCsvConfig : MonoBehaviour
    {
        private ContentSizeFitter m_ContentSizeFitter;
        private TestResourcesCsvConfigDatabase m_Database;
        private GameObject m_Prefab;
        private Transform m_Parent;

        private void Awake()
        {
            m_Database = new TestResourcesCsvConfigDatabase();
            m_Database.Load();

            m_Prefab = transform.Find("Items/Item").gameObject;
            m_Prefab.SetActive(false);
            m_Parent = transform.Find("Items");
            m_ContentSizeFitter = m_Parent.GetComponent<ContentSizeFitter>();
        }

        private void Start()
        {
            Create();
        }

        private void Create()
        {
            if(m_Database==null|| m_Database.GetAllData()==null|| m_Database.GetAllData().Count==0)
            {
                Debug.LogWarning(GetType() + "/Create()/ data is null!");
                return;
            }

            for(int i=0;i< m_Database.GetAllData().Count;i++)
            {
                TestResourcesCsvConfigData data = m_Database.GetAllData()[i];

                GameObject item = Instantiate(m_Prefab, m_Parent);
                item.SetActive(true);
                item.name = data.Id.ToString();

                item.transform.Find("Id").GetComponent<Text>().text = data.Id.ToString();
                item.transform.Find("Name").GetComponent<Text>().text = data.Name;
                item.transform.Find("Age").GetComponent<Text>().text = data.Age.ToString();
                item.transform.Find("Score").GetComponent<Text>().text = data.Score.ToString();

                Text Designation = item.transform.Find("Designation").GetComponent<Text>();
                Designation.text = null;
                for (int j=0;j< data.Designation.Length;j++)
                {
                    Designation.text += data.Designation[j] + ";";
                }

            }

            m_ContentSizeFitter.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_ContentSizeFitter.GetComponent<RectTransform>());
        }
    }
}