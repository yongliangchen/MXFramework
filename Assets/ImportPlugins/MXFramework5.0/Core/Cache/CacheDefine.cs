using System.IO;
using UnityEngine;

namespace Mx.Cache
{
    public class CacheDefine
    {
        private static string cachePath = Application.persistentDataPath + "/Cache";
        /// <summary>缓存存放路径</summary>
        public static string CachePath
        {
            get
            {
                if (!Directory.Exists(cachePath)) { Directory.CreateDirectory(cachePath); }
                return cachePath;
            }
        }
    }
}