/***
 * 
 *    Title: MXFramework
 *           主题: 时间工具类
 *    Description: 
 *           功能：1.获取时间戳
 * 
 *    Date: 2020
 *    Version: v4.0版本
 *    Modify Recoder: 
 *      
 */

using System;

namespace Mx.Utils
{
    public class TimeUtils
    {
        /// <summary>获取秒级别时间戳（10位）</summary>
        public static long GetTimestampToSeconds()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>获取毫秒级别时间戳（13位）</summary>
        public static long GetTimeStampToMilliseconds()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
    }
}