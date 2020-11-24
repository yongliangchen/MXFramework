using System;

namespace Mx.Cache
{
    [Serializable]
    public class CacheInfo 
    {
        public string Id;
        public string Version;
        public string LastModify;
        public string Md5;
    }

    [Serializable]
    public class CacheInfos
    {
        public string LastModify;
        public int Count;
        public CacheInfo[] content;
    }
}