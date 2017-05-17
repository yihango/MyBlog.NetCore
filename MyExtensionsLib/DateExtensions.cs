using System;

namespace MyExtensionsLib
{
    /// <summary>
    /// 时间扩展类
    /// </summary>
    public static class DateExtensions
    {
        /// <summary>
        /// 将时间转换为时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToStamp(this DateTime date)
        {
            var startDt = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            var res = (date - startDt).TotalMilliseconds.ToString();
            return res;
        }
    }
}

