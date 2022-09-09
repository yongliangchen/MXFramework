using System;
using SimpleSQL;

namespace Mx.Cache
{

    public class CacheInfo 
    {
        [PrimaryKey]
        public string Name { get; set; }
        public string Version { get; set; }
        public string LastModify { get; set; }
        public string Md5 { get; set; }
    }
}