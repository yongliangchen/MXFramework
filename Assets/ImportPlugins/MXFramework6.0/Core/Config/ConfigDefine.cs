using System.IO;
using UnityEngine;
using Mx.Utils;

namespace Mx.Config
{
    public sealed class ConfigDefine
    {
        /// <summary>是否对配置表进行加密</summary>
        public static bool Encrypt = UserData.Encrypt;

        public static string GENERATE_SCRIPT_PATH = Application.dataPath + "/Scripts/AutoGenerate/";

        public static string TEMPLATE_IDATABASE_PATH = "Template/Config/Template_IDatabase";

        public static string TEMPLATE_DATABASE_STREAMING_PATH = "Template/Config/Template_StreamingDatabase";

        public static string TEMPLATE_DATABASE_RESOURECES_PATH = "Template/Config/Template_ResourcesDatabase";

        public static string TEMPLATE_DATABASEMANAGER_PATH = "Template/Config/Template_DatabaseManager";

        private static string csvResourcesPath = Application.dataPath + "/Res/Csv/ResourcesCsv";
        /// <summary>Csv格式Resources配置表储存路径</summary>
        public static string CsvResourcesPath
        {
            get
            {
                if (!Directory.Exists(csvResourcesPath)) Directory.CreateDirectory(csvResourcesPath);
                return csvResourcesPath;
            }
        }

        private static string csvStreamingPath = Application.dataPath + "/Res/Csv/IOCsv";
        /// <summary>Csv格式Streaming配置表储存路径</summary>
        public static string CsvStreamingPath
        {
            get
            {
                if (!Directory.Exists(csvStreamingPath)) Directory.CreateDirectory(csvStreamingPath);
                return csvStreamingPath;
            }
        }

        /// <summary>Resouces加密配置表存放路径</summary>
        public static string CSV_RESOUCES_ENCRYPT_PATH = Application.dataPath + "/Resources/Config/";

        /// <summary>获取外部文件夹资源的加载路径</summary>
        public static string GetExternalConfigOutPath
        {
            get
            {
                string folderName = ConfigDefine.Encrypt ? StringEncrypt.GetStringMd5("Config") : "Config";
                string path = string.Format(PathTools.InitialResPath + "/{0}/", folderName);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }

        /// <summary>获取Resouces文件夹下的配置文档路径</summary>
        public static string GetResoucesConfigOutPath
        {
            get { return "Config/"; }
        }
    }

    /// <summary>配置表的加载方式</summary>
    public enum EnumCofigLoadType
    {
        Streaming,
        Resources,
    }
}