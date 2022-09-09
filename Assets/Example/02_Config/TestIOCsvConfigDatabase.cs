using UnityEngine;

//记得命名空间保持一致
namespace Mx.Config
{
    /// <summary>我是新建的部分类</summary>
    public partial class TestIOCsvConfigDatabase
    {
        //我是扩展的API
        public void PrintCount()
        {
            Debug.Log(listData.Count);
        }
    }
}


