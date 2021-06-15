using UnityEngine;
using UnityEditor;
using Mx.Config;
using System.IO;
using System.Text.RegularExpressions;
using Mx.Lua;

namespace Mx.UI
{
    /// <summary>自动生成UI</summary>
    public class UIScriptGenerate : MonoBehaviour
    {
        //[MenuItem("MXFramework/UI/Generate UI Param", false, 301)]
        public static void GenerateUIParam()
        {
            createCSharpUIFormNames();
            createLuaUIFormNames();
            //AssetDatabase.Refresh();
        }

        [MenuItem("MXFramework/UI/Generate UI CSharp Script", false, 302)]
        public static void GenerateUICSharpScript()
        {
            createUICSharpScript();
            AssetDatabase.Refresh();
        }

        /// <summary>创建UI窗口名称C#版本</summary>
        private static void createCSharpUIFormNames()
        {
            string template = getTemplate(UIDefine.Template_UIFORM_NAMES_CSHARP);

            string uiFormNameLiset = null;
            string uiFormNameType = null;

            UIConfigDatabase uIConfigInfo = new UIConfigDatabase();
            uIConfigInfo.Load();

            foreach (UIConfigData info in uIConfigInfo.GetAllDataList())
            {
                uiFormNameLiset += spliceFormName(info.Name, info.Des) + "\n";
                uiFormNameType += spliceFormType(info.Name, info.Des) + "\n";
            }

            template = template.Replace("$UIAttributes", uiFormNameLiset);
            template = template.Replace("$UIType", uiFormNameType);

            string dataName = ConfigDefine.GENERATE_SCRIPT_PATH + "/" + "UIFormNames.cs";

            generateScript(dataName, template);
        }

        /// <summary>创建UI窗口名称Lua版本</summary>
        private static void createLuaUIFormNames()
        {
            string template = getTemplate(UIDefine.Template_UIFORM_NAMES_LUA);

            string uiFormNames = null;
            string viewNames = null;

            UIConfigDatabase uIConfigInfo = new UIConfigDatabase();
            uIConfigInfo.Load();

            foreach (UIConfigData info in uIConfigInfo.GetAllDataList())
            {
                uiFormNames += info.Name + "=" + "\"" + info.Name + "\"" + "," + "\n";

                if (info.ScriptType == 1)
                {
                    viewNames += "\"" + info.Name + "\"" + "," + "\n";
                }
            }

            template = template.Replace("$UINames", uiFormNames);
            template = template.Replace("$ViewNames", viewNames);

            string folderPath = LuaDefine.LUA_SCRIPTS_PATH + "/src";
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            string dataName = folderPath+"/UIFormNames.lua";
            generateScript(dataName, template);
        }

        /// <summary>
		/// 切割UI窗口名称
		/// </summary>
		/// <param name="uiFormName">UI窗口名称</param>
		/// <param name="des">描述</param>
		/// <returns></returns>
        private static string spliceFormName(string uiFormName, string des)
        {
            string note = string.Format(" /// <summary>{0}</summary> \n", des);

            string temp = uiFormName.Replace("UIForm", null);
            string tempName = (Regex.Replace(temp, "(\\B[A-Z])", "_$1") + "_" + "UIFORM").ToUpper();

            string res = string.Format("public const string  {0} = \"" + uiFormName + "\"" + ";", tempName);

            return note + res;
        }

		/// <summary>
		/// 切割UI窗口类型
		/// </summary>
		/// <param name="uiFormName">UI窗口名称</param>
		/// <param name="des">描述</param>
		/// <returns></returns>
		private static string spliceFormType(string uiFormName, string des)
        {
            string note = string.Format(" /// <summary>{0}</summary> \n", des);
            string res = uiFormName + ",";

            return note + res;
        }

        /// <summary>自动创建UI脚本</summary>
        private static void createUICSharpScript()
        {
            UIConfigDatabase uIConfigInfo = new UIConfigDatabase();
            uIConfigInfo.Load();

            foreach (UIConfigData info in uIConfigInfo.GetAllDataList())
            {
                //创建C#脚本
                if (info.ScriptType == 0)
                {
                    string scriptPath = UIDefine.UIFormCSharpScriptsPath + info.Name + ".cs";
                    if (!File.Exists(scriptPath))
                    {
                        string template = getTemplate(UIDefine.Template_UIFORM_CSHARP_BASE);
                        template = template.Replace("$classNote", info.Des);
                        template = template.Replace("$className", info.Name);
                        template = template.Replace("$messageType", info.Name + "Event");

                        generateScript(scriptPath, template);
                    }
                }

                //创建Lua脚本
                else
                {
                    string scriptPath = UIDefine.UIFormLuaScriptsPath + info.Name + ".lua";
                    if (!File.Exists(scriptPath))
                    {
                        string template = getTemplate(UIDefine.Template_UIFORM_LUA_BASE);
                        template = template.Replace("$classNote", info.Des);
                        template = template.Replace("$className", info.Name);

                        generateScript(scriptPath, template);
                    }
                }
            }
        }

        /// <summary>
		/// 获取UI模板
		/// </summary>
		/// <param name="path">UI模板路径</param>
		/// <returns></returns>
        private static string getTemplate(string path)
        {
            //TextAsset txt = (TextAsset)AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));
            TextAsset txt = Resources.Load<TextAsset>(path);
            return txt.text;
        }

        /// <summary>
		/// 自动生成脚本
		/// </summary>
		/// <param name="dataName">数据名称</param>
		/// <param name="data">数据</param>
        private static void generateScript(string dataName, string data)
        {
            if (File.Exists(dataName)) File.Delete(dataName);

            StreamWriter sr = File.CreateText(dataName);
            sr.WriteLine(data);
            sr.Close();
        }
    }
}