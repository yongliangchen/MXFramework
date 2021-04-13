using UnityEngine;
using UnityEditor;
using Mx.Config;
using System.IO;
using System.Text.RegularExpressions;

namespace Mx.UI
{
    /// <summary>自动生成UI</summary>
    public class UIScriptGenerate : MonoBehaviour
    {
        //[MenuItem("MXFramework/UI/Generate UI Param", false, 301)]
        public static void GenerateUIParam()
        {
            CreateUIFormNames();
            AssetDatabase.Refresh();
        }

        [MenuItem("MXFramework/UI/Generate UI CSharp Script", false, 302)]
        public static void GenerateUICSharpScript()
        {
            CreateUICSharpScript();
            AssetDatabase.Refresh();
        }

        /// <summary>创建UI窗口</summary>
        private static void CreateUIFormNames()
        {
            string template = GetTemplate(UIDefine.Template_UIFORM_NAMES);

            string uiFormNameLiset = null;
            string uiuiFormNameType = null;

            UIConfigDatabase uIConfigInfo = new UIConfigDatabase();
            uIConfigInfo.Load();

            foreach (UIConfigData info in uIConfigInfo.GetAllDataList())
            {
                uiFormNameLiset += SpliceFormName(info.Name, info.Des) + "\n";
                uiuiFormNameType += SpliceFormType(info.Name, info.Des) + "\n";
            }

            template = template.Replace("$UIAttributes", uiFormNameLiset);
            template = template.Replace("$UIType", uiuiFormNameType);

            string dataName = ConfigDefine.GENERATE_SCRIPT_PATH + "/" + "UIFormNames.cs";

            GenerateScript(dataName, template);
        }

        /// <summary>
		/// 切割UI窗口名称
		/// </summary>
		/// <param name="uiFormName">UI窗口名称</param>
		/// <param name="des">描述</param>
		/// <returns></returns>
        private static string SpliceFormName(string uiFormName, string des)
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
		private static string SpliceFormType(string uiFormName, string des)
        {
            string note = string.Format(" /// <summary>{0}</summary> \n", des);
            string res = uiFormName + ",";

            return note + res;
        }

        /// <summary>自动创建UI脚本</summary>
        private static void CreateUICSharpScript()
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
                        string template = GetTemplate(UIDefine.Template_UIFORM_CSHARP_BASE);
                        template = template.Replace("$classNote", info.Des);
                        template = template.Replace("$className", info.Name);
                        template = template.Replace("$messageType", info.Name + "Event");

                        GenerateScript(scriptPath, template);
                    }
                }

                //创建Lua脚本
                else
                {
                    string scriptPath = UIDefine.UIFormLuaScriptsPath + info.Name + ".lua";
                    if (!File.Exists(scriptPath))
                    {
                        string template = GetTemplate(UIDefine.Template_UIFORM_LUA_BASE);
                        template = template.Replace("$classNote", info.Des);
                        template = template.Replace("$className", info.Name);

                        GenerateScript(scriptPath, template);
                    }
                }
            }
        }

        /// <summary>
		/// 获取UI模板
		/// </summary>
		/// <param name="path">UI模板路径</param>
		/// <returns></returns>
        private static string GetTemplate(string path)
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
        private static void GenerateScript(string dataName, string data)
        {
            if (File.Exists(dataName)) File.Delete(dataName);

            StreamWriter sr = File.CreateText(dataName);
            sr.WriteLine(data);
            sr.Close();
        }
    }
}