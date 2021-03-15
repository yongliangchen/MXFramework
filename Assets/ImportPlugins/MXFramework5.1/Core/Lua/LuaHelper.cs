using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Mx.Lua
{
    public class LuaHelper 
    {
        //本类静态实例
        private static LuaHelper _Instance;
        //Lua 环境
        private LuaEnv _luaEnv = new LuaEnv();
        //缓存lua文件名称与对应的lua信息。
        private Dictionary<string, byte[]> _DicLuaFileArray = new Dictionary<string, byte[]>();


        private LuaHelper()
        {
            //私有构造函数
            _luaEnv.AddLoader(customLoader);
        }

        /// <summary>
        /// 得到帮助类实例
        /// </summary>
        /// <returns></returns>
        public static LuaHelper Instance()
        {
            if (_Instance == null)
            {
                _Instance = new LuaHelper();
            }
            return _Instance;
        }

        public void DoString()
        {

        }

        /// <summary>
        /// 自定义调取lua文件内容
        /// </summary>
        /// <param name="fileName">lua文件名称</param>
        /// <returns></returns>
        private byte[] customLoader(ref string fileName)
        {
            //缓存判断处理： 根据lua文件路径，获取lua的内容
            if (_DicLuaFileArray.ContainsKey(fileName))
            {
                //如果在缓存中可以查找成功，则直接返回结果。
                return _DicLuaFileArray[fileName];
            }
            //else
            //{
            //    return ProcessDIR(new DirectoryInfo(luaPath), fileName);
            //}

            return null;
        }
    }
}