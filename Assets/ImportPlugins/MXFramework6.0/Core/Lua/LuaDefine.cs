using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Mx.Lua
{
    public class LuaDefine
    {
        /// <summary>获取lua脚本的输出路径</summary>
        public static string GetLuaOutPath()
        {
            string path = string.Empty;

            if (Application.isEditor) path = Application.streamingAssetsPath + "/Lua";
            else path = Application.persistentDataPath + "/Lua";

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path;
        }

        /// <summary>脚本的后缀名</summary>
        public const string SCRIPTS_EXTENSIONS = "lua";
    }
}