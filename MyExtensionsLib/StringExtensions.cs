using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using KingLion.WebUtils.NPinyin;

namespace MyExtensionsLib
{
    /// <summary>
    /// 字符串的扩展函数
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// 判断内容是否为空
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNullOrWhitespace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }



        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string text, params object[] args)
        {
            return string.Format(text, args);
        }



        /// <summary>
        /// 将字符串进行32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(this string input)
        {
            if (input == null)
                input = string.Empty;
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
            var result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("x2").ToLowerInvariant());
            }
            return result.ToString();
        }



        /// <summary>
        /// 从html中提取纯文本
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string RemoveHtml(this string strHtml)
        {
            //删除脚本
            strHtml = Regex.Replace(strHtml, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            strHtml = Regex.Replace(strHtml, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, @"-->", "", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, @"<!--.*", "", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            strHtml.Replace("<", "");
            strHtml.Replace(">", "");
            strHtml.Replace("\r\n", "");

            return strHtml;
        }



        /// <summary>
        /// 简体中文转拼音扩展
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <returns>拼音字符串</returns>
        public static string ConvertChineseToPY(this string value)
        {
            return Regex.Replace(value, "[\u4e00-\u9fa5]", (m) => string.Format(" {0} ", m.Value.ChsToPinYin())).Replace(" ", "");
        }



        #region 汉字转拼音

        /// <summary>
        /// 简体中文转拼音
        /// </summary>
        /// <param name="chs">简体中文字</param>
        /// <returns>拼音</returns>
        private static string ChsToPinYin(this string chs)
        {
            var res = Pinyin.GetPinyin(chs, Encoding.UTF8);
            return res;
        }

        #endregion 汉字转拼音
    }
}