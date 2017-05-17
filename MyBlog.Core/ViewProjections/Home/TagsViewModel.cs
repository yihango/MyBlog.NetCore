using System.Collections.Generic;
using MyBlog.Models;

namespace MyBlog.Core.ViewProjections.Home
{
    /// <summary>
    /// 标签视图模型
    /// </summary>
    public class TagsViewModel
    {
        /// <summary>
        /// 标签集合
        /// </summary>
        public  IList<tag_statistics_tb> TagList { get; set; }
    }
}
