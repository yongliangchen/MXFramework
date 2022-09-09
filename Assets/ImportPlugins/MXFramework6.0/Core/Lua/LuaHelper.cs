using System.Collections.Generic;
using System.IO;
using Mx.Utils;
using XLua;
using UnityEngine;
using Mx.UI;

namespace Mx.Lua
{
    public class LuaHelper 
    {
        //本类静态实例
        private static LuaHelper m_Instance;
        //Lua 环境
        private LuaEnv m_luaEnv = new LuaEnv();
        //缓存lua文件名称与对应的lua信息。
        private Dictionary<string, byte[]> m_DicLuaFileArray = new Dictionary<string, byte[]>();
        /// <summary>Lua脚本存放路径</summary>
        private Dictionary<string, string> m_DicLuaScriptsPaht = new Dictionary<string, string>();

        private LuaHelper()
        {
            getAllLuaScriptsPath();

            m_luaEnv.AddLoader(customLoader);
        }

        /// <summary>
        /// 得到帮助类实例
        /// </summary>
        /// <returns></returns>
        public static LuaHelper Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new LuaHelper();
                }
                return m_Instance;
            }
        }

        /// <summary>得到lua环境</summary>
        public LuaEnv GetLuaEnv()
        {
            if (m_luaEnv != null) return m_luaEnv;
            else
            {
                Debug.LogError(GetType() + "/GetLuaEnv()/ luaEnv is null!");
                return null;
            }
        }

        /// <summary>执行lua代码</summary>
        public void DoString(string chunk, string chunkName = "chunk", LuaTable env = null)
        {
            m_luaEnv.DoString(chunk, chunkName, env);
        }

        /// <summary>直接运行代码（简写'require'关键字）</summary>
        public void RunScripts(string chunk, string chunkName = "chunk", LuaTable env = null)
        {
            if (!chunk.StartsWith("require"))
            {
                chunk = string.Format("require '{0}'", chunk);
            }

            m_luaEnv.DoString(chunk, chunkName, env);
        }

        /// <summary>
        /// 调用Lua函数
        /// </summary>
        /// <param name="luaScriptName">lua脚本名称</param>
        /// <param name="luaMethodName">lua的函数名称</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public object[] CallLuaFunction(string luaScriptName,string luaMethodName,params object[] args)
        {
            LuaTable luaTable = m_luaEnv.Global.Get<LuaTable>(luaScriptName);
            LuaFunction luaFunction = luaTable.Get<LuaFunction>(luaMethodName);
            return luaFunction.Call(args);
        }

        /// <summary>给指定对象动态添加“BaseLuaUIForm”脚本</summary>
        public void AddBaseLuaUIForm(GameObject go,string luaScriptsName)
        {
            BaseLuaUIForm baseLuaUIForm = go.GetComponent<BaseLuaUIForm>();
            LuaDefine.MappingScriptName = luaScriptsName;
            if (baseLuaUIForm == null) baseLuaUIForm=go.AddComponent<BaseLuaUIForm>();
        }

        /// <summary>获取所有Lua脚本的路径</summary>
        private void getAllLuaScriptsPath()
        {
            string path = LuaDefine.GetLuaScriptsOutPath;
            string[] files = System.IO.Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            if ((files != null) && (files.Length > 0))
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    string fileName = files[i];
                    FileInfo fileInfo = new FileInfo(fileName);

                    if (!filter(fileInfo) || m_DicLuaScriptsPaht.ContainsKey(fileInfo.Name)) continue;
                    m_DicLuaScriptsPaht.Add(fileInfo.Name, fileInfo.FullName);
                }
            }
        }

        /// <summary>筛选</summary>
        private bool filter(FileInfo fileInfo)
        {
            if (fileInfo.Extension == ".meta" || fileInfo.Extension == ".DS_Store" || fileInfo.Extension == ".cs" ||
                fileInfo.Extension == ".dll" || fileInfo.Extension == ".cpp" || fileInfo.Extension == ".a"
                || fileInfo.Extension == ".so"

               ) return false;

            else return true;
        }

        /// <summary>
        /// 自定义调取lua文件内容
        /// </summary>
        /// <param name="fileName">lua文件名称</param>
        /// <returns></returns>
        private byte[] customLoader(ref string fileName)
        {
            string luaName = fileName.Replace(".lua.txt", string.Empty);
            luaName = luaName.Replace(".lua", string.Empty);
            luaName = (LuaDefine.Encrypt) ? StringEncrypt.GetStringMd5(luaName) : luaName;

            if (!m_DicLuaScriptsPaht.ContainsKey(luaName))
            {
                Debug.LogError(GetType() + "/customLoader()/ lua scripts is null! luaName:" + fileName);
                return null;
            }

            //缓存判断处理： 根据lua文件路径，获取lua的内容
            if (m_DicLuaFileArray.ContainsKey(luaName))
            {
                return m_DicLuaFileArray[luaName];
            }

            return loadLuaScripts(luaName,m_DicLuaScriptsPaht[luaName]);
        }

        /// <summary>加载Lua脚本</summary>
        private byte[] loadLuaScripts(string luaName, string luaScriptsPath)
        {
            StreamReader streamReader = null;
            if (File.Exists(luaScriptsPath)) streamReader = File.OpenText(luaScriptsPath);
            else
            {
                Debug.LogError(GetType() + "/luaScriptsPath()/ load luaScripts eroor!  path:" + luaScriptsPath);
                return null;
            }

            string str = streamReader.ReadToEnd();
            streamReader.Close();
            streamReader.Dispose();

            if (LuaDefine.Encrypt) str = StringEncrypt.DecryptDES(str);
            byte[] decBytes = System.Text.Encoding.UTF8.GetBytes(str);
            m_DicLuaFileArray.Add(luaName, decBytes);
            return decBytes;
        }

    }
}