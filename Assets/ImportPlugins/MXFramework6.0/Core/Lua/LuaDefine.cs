using System.IO;
using Mx.Utils;
using UnityEngine;

namespace Mx.Lua
{
    public class LuaDefine
    {
        /// <summary>映射脚本的名称</summary>
        public static string MappingScriptName { get; set; }

        /// <summary>是否对Lua进行加密</summary>
        public static bool Encrypt = UserData.Encrypt;
        /// <summary>Lua脚本后缀</summary>
        public const string LUA_SCRIPTS_EXTENSIONS = "lua";

        /// <summary>Lua脚本存放路径</summary>
        public static string LUA_SCRIPTS_PATH = Application.dataPath + "/Scripts/Lua";

        /// <summary>获取Lua脚本输入路径</summary>
        public static string GetLuaScriptsOutPath
        {
            get
            {
                string folderName = Encrypt ? StringEncrypt.GetStringMd5("Lua") : "Lua";
                string path = string.Format(PathTools.InitialResPath + "/{0}/", folderName);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }
    }
}