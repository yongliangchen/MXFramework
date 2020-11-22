using System.Collections.Generic;

namespace Mx.Res
{
    /// <summary>
    /// 管理引用关系和依赖关系
    /// </summary>
    public class ABRelating
    {
        /// <summary>所有的依赖包集合</summary>
        private List<string> m_LisAllDependenceAB;
        /// <summary>本包中所有的引用包集合</summary>
        private List<string> m_LisAllReferenceAB;

        public ABRelating(string abName)
        {
            m_LisAllDependenceAB = new List<string>();
            m_LisAllReferenceAB = new List<string>();
        }

        #region 依赖关系

        /// <summary>
        /// 添加依赖关系
        /// </summary>
        /// <param name="abName">添加依赖项AssetBundle名字</param>
        public void AddDependence(string abName)
        {
            if (!m_LisAllDependenceAB.Contains(abName))
            {
                m_LisAllDependenceAB.Add(abName);
            }
        }

        /// <summary>
        /// 移除依赖关系
        /// </summary>
        /// <param name="abName">移除AssetBundle包名称</param>
        /// true:此 AssetBundle 没有依赖项
        /// false:此 AssetBundle 还有其他依赖项
        public bool RemoveDependence(string abName)
        {
            if (m_LisAllDependenceAB.Contains(abName))
            {
                m_LisAllDependenceAB.Remove(abName);
            }

            return m_LisAllDependenceAB.Count > 0;
        }


        /// <summary>
        /// 获取所有的依赖关系
        /// </summary>
        /// <returns>The all dependenc.</returns>
        public List<string> GetAllDependenc()
        {
            return m_LisAllDependenceAB;
        }

        #endregion

        #region 引用关系

        /// <summary>
        /// 添加引用关系
        /// </summary>
        /// <param name="abName">添加引用关系AssetBundle名字</param>
        public void AddReference(string abName)
        {
            if (!m_LisAllReferenceAB.Contains(abName))
            {
                m_LisAllReferenceAB.Add(abName);
            }
        }

        /// <summary>
        /// 移除引用关系
        /// </summary>
        /// <param name="abName">移除AssetBundle包名称</param>
        /// true:此 AssetBundle 没有引用项
        /// false:此 AssetBundle 还有其他引用项
        public bool RemoveReference(string abName)
        {
            if (m_LisAllReferenceAB.Contains(abName))
            {
                m_LisAllReferenceAB.Remove(abName);
            }

            return m_LisAllReferenceAB.Count > 0;
        }

        /// <summary>
        /// 获取所有的引用关系
        /// </summary>
        /// <returns>The all dependenc.</returns>
        public List<string> GetAllReference()
        {
            return m_LisAllReferenceAB;
        }

        #endregion

    }
}

