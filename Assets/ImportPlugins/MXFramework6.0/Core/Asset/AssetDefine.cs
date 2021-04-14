/***
 * 
 *    Title: MXFramework
 *           主题: 资源模块全局定义
 *    Description: 
 *           功能：1.资源模块全局枚举定义
 *                2.资源模块全局委托定义
 *                3.资源模块全局数据定义            
 *                                  
 *    Date: 2021
 *    Version: v6.0版本
 *    Modify Recoder:      
 *
 */

using UnityEngine;


namespace Mx.Res
{
    public class AssetDefine 
    {
        /// <summary>Unity场景打包AB时候的后缀名称</summary>
        public const string AB_SCENE_EXTENSIONS = "u3d";
        /// <summary>Unity资源打包AB时候的后缀名称</summary>
        public const string AB_RES_EXTENSIONS = "data";
        /// <summary>压缩文件的后缀名称</summary>
        public const string UPK_EXTENSIONS = "upk";

        /// <summary>需要打包资源的文件路径</summary>
        public const string AB_RESOURCES = "Res/AbRes";

        /// <summary>获取需要打包AB资源的目录路径</summary>
        public static string GetSourceDataPath()
        {
            string path= Application.dataPath + "/" + AB_RESOURCES;

            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return path.Replace("/",@"\");
            }
            else return path;
        }

        /// <summary>获取AB资源加载路径</summary>
        public static string GetABLoadPath()
        {
            return PathTools.InitialResPath + "/AssetsBundles" + "/" + GetPlatformName();

            //return Application.streamingAssetsPath + "/AssetsBundles" + "/" + GetPlatformName();
        }

        /// <summary>获取AB资源打包路径</summary>
        public static string GetBuildAssetOutPath()
        {
            return PathTools.InitialResPath + "/AssetsBundles";

            //return Application.streamingAssetsPath + "/AssetsBundles";
        }

        /// <summary>获取 Manifest 文件存放路径</summary>
        public static string GetManifestPath()
        {
            return GetBuildAssetOutPath() + "/" + GetPlatformName() + "/" + GetPlatformName();
        }

        /// <summary>获取平台名称(苹果电脑使用ios的资源，Wind电脑使用Android资源)</summary>
        public static string GetPlatformName()
        {
            string strReturnPlatformName = string.Empty;

            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.IPhonePlayer:

                    strReturnPlatformName = "iOS";
                    break;

                case RuntimePlatform.Android:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:

                    strReturnPlatformName = "Android";
                    break;
            }

            return strReturnPlatformName;
        }

        /// <summary>获取平台名称</summary>
        //public static string GetPlatformName()
        //{
        //    string strReturnPlatformName = string.Empty;

        //    switch (Application.platform)
        //    {
        //        case RuntimePlatform.WindowsPlayer:
        //        case RuntimePlatform.WindowsEditor:

        //            strReturnPlatformName = "Windows";
        //            break;

        //        case RuntimePlatform.OSXEditor:
        //        case RuntimePlatform.OSXPlayer:

        //            strReturnPlatformName = "OSX";
        //            break;

        //        case RuntimePlatform.IPhonePlayer:

        //            strReturnPlatformName = "iOS";
        //            break;

        //        case RuntimePlatform.Android:

        //            strReturnPlatformName = "Android";
        //            break;
        //    }

        //    return strReturnPlatformName;
        //}

        /// <summary>Upk压缩文件输出目录</summary>
        public static string UpkOutPant = "/Users/chenyongliang/UnityProject/AssetsBundles/" + Application.productName;
        /// <summary>Upk压缩缓存路径</summary>
        public static string UpkTempCompressionPath = Application.persistentDataPath + "/Cache/Upk/Compression";
    }

}