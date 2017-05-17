using MyExtensionsLib.SqlSugar;
using System;
namespace MyBlog.Models
{
    /// <summary>
    /// 博文类
    /// </summary>
    public partial class post_tb
    {
        /// <summary>
        /// 博文编号
        /// </summary>
        public string post_id { get; set; }

        /// <summary>
        /// 博文标题
        /// </summary>
        public string post_title { get; set; }

        /// <summary>
        /// 博文摘要信息
        /// </summary>
        public string post_summary { get; set; }

        /// <summary>
        /// 博文发布时间
        /// </summary>
        public DateTime? post_pub_time { get; set; }

        /// <summary>
        /// 博文发布的短时间 yyyy_MM_dd
        /// </summary>
        public string post_pub_sortTime { get; set; }

        /// <summary>
        /// 博文标签集合(","号分割)
        /// </summary>
        public string post_tags { get; set; }


        public string[] GetTags()
        {
            return post_tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 博文本地路径
        /// </summary>
        public string post_path { get; set; }

        /// <summary>
        /// 博文的发布状态:1为已发布，其他为未发布
        /// </summary>
        public int post_pub_state { get; set; }
    }
}
