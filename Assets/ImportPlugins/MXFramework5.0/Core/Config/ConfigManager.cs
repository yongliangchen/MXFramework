using Mx.Utils;

namespace Mx.Config
{
    /// <summary>配置表管理</summary>
    public class ConfigManager : MonoSingleton<ConfigManager>
    {
        private DatabaseManager databaseManager;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            databaseManager = new DatabaseManager();
        }

        /// <summary>加载数据</summary>
        public void Load()
        {
            databaseManager.Load();
        }

        /// <summary>
        /// 获取数据（泛型）
        /// </summary>
        /// <returns></returns>
        public T GetDatabase<T>() where T : IDatabase, new()
        {
            return databaseManager.GetDatabase<T>();
        }
    }
}