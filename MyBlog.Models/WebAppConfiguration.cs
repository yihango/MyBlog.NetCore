using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Models
{
    public class WebAppConfiguration
    {
        /// <summary>
        /// 设置节点
        /// </summary>
        public Settings settings { get; set; }
        /// <summary>
        /// 连接字符节点
        /// </summary>
        public ConnectionStrings connectionStrings { get; set; }
    }
    /// <summary>
    /// 配置文件中 Settings 节点模型
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// 博文存储的相对路径
        /// </summary>
        public string PostRelativeSavePath { get; set; }
        /// <summary>
        /// 上传图片的相对路径
        /// </summary>
        public string UpLoadImgRelativeSavePath { get; set; }
        /// <summary>
        /// 配置文件中设置的验证码字体
        /// </summary>
        public string FontFamily { get; set; }
    }

    /// <summary>
    /// 配置文件中 ConnectionStrings 节点模型
    /// </summary>
    public class ConnectionStrings
    {
        /// <summary>
        /// 配置文件中的连接字符串
        /// </summary>
        public string connStr { get; set; }
    }

}
