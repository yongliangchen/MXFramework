/***
 * 
 *    Title: MXFramework
 *           主题: 获取本机IP
 *    Description: 
 *           功能：获取局域网和广域网IP
 * 
 *    Date: 2020
 *    Version: v5.0版本
 *    Modify Recoder: 
 *      
 */

using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;

namespace Mx.Net
{
    /// <summary>本机IP</summary>
    public class LocalIP :MonoBehaviour
    {
        /// <summary>获取局域网IP</summary>
        public static string GetLANIP(ADDRESSFAM Addfam)
        {
            if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6)
            {
                return null;
            }

            string output = string.Empty;

            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
               #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
               NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
               NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;
               if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
               #endif
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        //IPv4
                        if (Addfam == ADDRESSFAM.IPv4)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                output = ip.Address.ToString();
                            }
                        }

                        //IPv6
                        else if (Addfam == ADDRESSFAM.IPv6)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                            {
                                output = ip.Address.ToString();
                            }
                        }
                    }
                }
            }

            return output;
        }

        /// <summary>获取广域网IP</summary>
        public static void GetWANIp(Action<string> finish)
        {
            string url = "http://icanhazip.com/";
            //string url = "https://www.ip.cn";

            WebRequest.Instance.Get(url, (uwr) =>
            {
                if (!string.IsNullOrEmpty(uwr.error))
                {
                    Debug.LogWarning("Amap/Location()/获取IP信息失败！");
                    return;
                }

                string ip = uwr.downloadHandler.text;
                if (string.IsNullOrEmpty(ip))
                {
                    Debug.LogWarning("Amap/Location()/获取IP信息失败！");
                    return;
                }

                if (finish != null) finish(ip);
            });
        }
    }

    public enum ADDRESSFAM
    {
        IPv4, IPv6
    }
}