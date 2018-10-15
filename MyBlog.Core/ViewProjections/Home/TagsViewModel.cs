using MyBlog.Tags;
using MyBlog.TagStatistics;
using System.Collections.Generic;


namespace MyBlog.ViewProjections.Home
{
    /// <summary>
    /// 标签视图模型
    /// </summary>
    public class TagsViewModel
    {
        /// <summary>
        /// 标签集合
        /// </summary>
        public  IList<TagStatistic> TagList { get; set; }
    }
}
