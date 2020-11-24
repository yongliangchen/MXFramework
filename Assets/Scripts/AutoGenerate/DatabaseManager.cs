/*代码自动生成的类，请勿手动修改*/

using UnityEngine;
using System.Collections.Generic;

namespace Mx.Config
{
	public class DatabaseManager
	{
		private Dictionary<uint, IDatabase> m_databases;

		public DatabaseManager()
		{
			m_databases = new Dictionary<uint, IDatabase>();

			RegisterDataType(new TestIOCsvConfigDatabase());
RegisterDataType(new CnConfigDatabase());
			RegisterDataType(new EnConfigDatabase());
			RegisterDataType(new TestResourcesCsvConfigDatabase());
			RegisterDataType(new UIConfigDatabase());


			Load();
		}

		public void Load()
		{
			foreach(KeyValuePair<uint, IDatabase> data in m_databases)
			{
				data.Value.Load();
			}
		}


		public T GetDatabase<T>() where T : IDatabase, new()
		{
			T result = new T();
			if(m_databases.ContainsKey(result.TypeID()))
			{
				return (T)m_databases[result.TypeID()];
			}

			return default(T);
		}

		private void RegisterDataType(IDatabase database)
		{
			m_databases[database.TypeID()] = database;
		}
	}

}
