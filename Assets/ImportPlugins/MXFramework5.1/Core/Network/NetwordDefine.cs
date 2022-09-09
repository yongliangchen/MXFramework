/***
 * 
 *    Title: MXFramework
 *           主题: 定义网络模块的一下公共属性
 *    Description: 
 *           功能：1.公共变量和常量
 *                2.公共委托
 *                3.公共枚举
 *                
 *    Date: 2020
 *    Version: v5.0版本
 *    Modify Recoder:     
 */

using UnityEngine;
using UnityEngine.Networking;

namespace Mx.Net
{
    public class NetwordDefine
    {

    }

    public delegate void DelGetTextureCallback(string error, Texture2D texture2D);
    public delegate void DelGetTextCallback(string error, string text);
    public delegate void DelGetAbCallback(string error, AssetBundle assetBundle);
    public delegate void DelGetAudioClipCallback(string error, AudioClip audioClip);
    public delegate void DelWebRequestCallback(float progress, UnityWebRequest unityWeb);
}