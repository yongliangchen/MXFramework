using System;
using UnityEngine;
using YZL.Compress.UPK;

namespace Mx.Util
{
    /// <summary>Upk工具</summary>
    public class UpkUtil : MonoBehaviour
    {
        /// <summary>
        /// 异步压缩文件（指定一个文件目录）
        /// </summary>
        /// <param name="inPath">需要压缩文件目录</param>
        /// <param name="outPath">压缩文件输出路径</param>
        /// <param name="progress">压缩进度</param>
        public static void PackFolderAsync(string inPath, string outPath, Action<float> progress = null)
        {
            UPKFolder.PackFolderAsync(inPath, outPath, (all, now) => { if (progress != null) progress(now / all); });
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="inPath">需要压缩文件目录</param>
        /// <param name="outPath">压缩文件输出路径</param>
        /// <param name="progress">压缩进度</param>
        public static void PackFolder(string inPath, string outPath, Action<float> progress = null)
        {
            UPKFolder.PackFolder(inPath, outPath, (all, now) => { if (progress != null) progress(now / all); });
        }

        /// <summary>
        /// 解压缩（异步）
        /// </summary>
        /// <param name="inPath">需要解压文件路径</param>
        /// <param name="outPath">解压输出路径</param>
        /// <param name="progress">解压进度</param>
        public static void UnPackFolderAsync(string inPath, string outPath, Action<float> progress = null)
        {
            UPKFolder.UnPackFolderAsync(inPath, outPath, (all, now) => { if (progress != null) progress(now / all); });
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="inPath">需要解压文件路径</param>
        /// <param name="outPath">解压输出路径</param>
        /// <param name="progress">解压进度</param>
        public static void UnPackFolder(string inPath, string outPath, Action<float> progress = null)
        {
            UPKFolder.UnPackFolder(inPath, outPath, (all, now) => { if (progress != null) progress(now / all); });
        }

    }
}