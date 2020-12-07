
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleSQL;
using UnityEngine;

namespace Mx.Cache
{
    /// <summary>缓存管理</summary>
    public class CacheManager : MonoBehaviour
    {
        private SimpleSQLManager m_DbManager;
        //private TextAsset m_CacheBytes;

        private void Awake()
        {
            Debug.Log(Application.persistentDataPath);
            //m_CacheBytes = Resources.Load<TextAsset>("DataBases/Cache");
            GameObject dbPrefab = Resources.Load<GameObject>("DBManager");
            m_DbManager = Instantiate(dbPrefab, transform).GetComponent<SimpleSQLManager>();

            createTable();
        }

        /// <summary>获取所有缓存文件的大小，单位字节</summary>
        public long Length()
        {
            long capacity = 0;

            FileInfo[] filesArr = DirectoryEx.GetFiles(CacheDefine.CachePath);

            for (int i = 0; i < filesArr.Length; i++)
            {
                FileInfo fileInfo = filesArr[i];
                capacity += fileInfo.Length;
            }

            return capacity;
        }

        /// <summary>清理缓存</summary>
        public void ClearCache()
        {
            if (Directory.Exists(CacheDefine.CachePath)) Directory.Delete(CacheDefine.CachePath, true);
            DeleteAll();
        }

        /// <summary>添加一条数据</summary>
        public void Add(CacheInfo data)
        {
            bool recordExists = queryByName(data.Name);

            if (recordExists) updateData(data);
            else createData(data);
        }

        /// <summary>通过数据名字删除一条数据</summary>
        public void DeleteByName(string dataName)
        {
            CacheInfo cacheInfo = new CacheInfo { Name = dataName };
            m_DbManager.Delete<CacheInfo>(cacheInfo);
        }

        /// <summary>删除所有数据</summary>
        public void DeleteAll()
        {
            m_DbManager.Execute("delete from CacheInfo", new object[] { });
        }

        /// <summary>查找全部数据</summary>
        public List<CacheInfo> Query()
        {
            return new List<CacheInfo>(from cache in m_DbManager.Table<CacheInfo>() select cache);
        }

        /// <summary>查找一条数据</summary>
        public CacheInfo QueryFirstRecord(string dataName)
        {
            bool recordExists;
            return m_DbManager.QueryFirstRecord<CacheInfo>(out recordExists, "SELECT * FROM CacheInfo WHERE Name = ?", dataName);
        }

        /// <summary>创建表格</summary>
        private void createTable()
        {
            m_DbManager.CreateTable<CacheInfo>();
        }

        /// <summary>通过名字查找一条数据是否存在</summary>
        private bool queryByName(string dataName)
        {
            bool recordExists;
            var firstRecord = m_DbManager.QueryFirstRecord<CacheInfo>(out recordExists, "SELECT * FROM CacheInfo WHERE Name = ?", dataName);
            return recordExists;
        }

        /// <summary>更新一条数据</summary>
        private void updateData(CacheInfo data)
        {
            m_DbManager.UpdateTable(data);
        }

        /// <summary>创建一条数据</summary>
        private void createData(CacheInfo data)
        {
            m_DbManager.Insert(data);
        }

    }
}
