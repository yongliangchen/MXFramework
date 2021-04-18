/***
 * 
 *    Title: MXFramework
 *           主题: 拷贝lua脚本
 *    Description: 
 *           功能：1.将lua脚本拷贝到指定的目录
 *                2.对lua脚本进行加密
 * 
 *    Date: 2021
 *    Version: v6.0版本
 *    Modify Recoder: 
 *      
 */

using System.IO;
using Mx.Utils;
using UnityEditor;
using UnityEngine;

namespace Mx.Lua
{
    public class CopyLuaScripts
    {
        /// <summary>是否自动拷贝lua脚本</summary>
        public static bool autoCopyLua = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BeforeRuntimeInitializeOnLoadMethod()
        {
            if (Application.isEditor && autoCopyLua) copyScripts();
        }

        /// <summary>Lua脚本的数量</summary>
        private static int m_LuaScriptsCount = 0;

        [MenuItem("MXFramework/Lua/Copy Lua Scripts #l", false, 401)]
        public static void copyScripts()
        {
            m_LuaScriptsCount = 0;

            if (Directory.Exists(LuaDefine.GetLuaScriptsOutPath)) Directory.Delete(LuaDefine.GetLuaScriptsOutPath,true);

            string path = LuaDefine.LUA_SCRIPTS_PATH;
            string[] files = System.IO.Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            if ((files != null) && (files.Length > 0))
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    string fileName = files[i];
                    FileInfo fileInfo = new FileInfo(fileName);

                    if (filter(fileInfo))
                    {
                        m_LuaScriptsCount++;
                        string str = readTxt(fileInfo.FullName);
                        writeTxt(fileInfo.Name, str);
                    }
                }

                Debug.Log("拷贝Lua脚本完成！ 脚本数量："+ m_LuaScriptsCount);
            }
        }

        /// <summary>筛选</summary>
        private static bool filter(FileInfo fileInfo)
        {
            if (fileInfo.Extension == ".lua" || fileInfo.Extension == ".txt") return true;
            return false;
        }

        /// <summary>
        /// 读取txt文本
        /// </summary>
        /// <param name="textPath">文本路径</param>
        /// <returns></returns>
        private static string readTxt(string textPath)
        {
            StreamReader streamReader = null;
            if (File.Exists(textPath)) streamReader = File.OpenText(textPath);
            else
            {
                Debug.LogError("CopyLuaScripts/readTxt()/ load text eroor!  path:" + textPath);
                return null;
            }

            string str = streamReader.ReadToEnd();
            streamReader.Close();
            streamReader.Dispose();

            return str;
        }

        /// <summary>
        /// 写入txt文本
        /// </summary>
        /// <param name="textName">文本名称</param>
        /// <param name="text">文本</param>
        private static void writeTxt(string textName,string text)
        {
            string newTextName = textName.Replace(".lua.txt", string.Empty);
            newTextName = newTextName.Replace(".lua", string.Empty);

            string data = (LuaDefine.Encrypt) ? StringEncrypt.EncryptDES(text) : text;
            string textPath = LuaDefine.GetLuaScriptsOutPath + "/" + ((LuaDefine.Encrypt) ?
               StringEncrypt.GetStringMd5(newTextName) : newTextName);

            if (File.Exists(textPath))
            {
                Debug.LogError("CopyLuaScripts/writeTxt()/拷贝失败，存在同名文件！ textName：" + textName);
                File.Delete(textPath);
            }

            StreamWriter sr = File.CreateText(textPath);
            sr.WriteLine(data);
            sr.Close();
        }

    }
}