using MyBlog.Core.Tags;
using MyBlog.Core.TagStatistics;
using System.Collections.Generic;


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
        public  IList<TagStatistic> TagList { get; set; }
    }
}
