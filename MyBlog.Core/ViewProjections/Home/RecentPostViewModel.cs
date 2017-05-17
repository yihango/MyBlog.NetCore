using System.Collections.Generic;
using MyBlog.Models;

namespace MyBlog.Core.ViewProjections.Home
{
    /// <summary>
    /// 最近博文视图模型
    /// </summary>
    public class RecentPostViewModel
    {
        /// <summary>
        /// 最近博文集合
        /// </summary>
        public IList<post_tb> PostList { get; set; }
    }
}
