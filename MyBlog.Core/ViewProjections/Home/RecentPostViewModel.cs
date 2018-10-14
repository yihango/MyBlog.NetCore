using MyBlog.Posts;
using System.Collections.Generic;


namespace MyBlog.ViewProjections.Home
{
    /// <summary>
    /// 最近博文视图模型
    /// </summary>
    public class RecentPostViewModel
    {
        /// <summary>
        /// 最近博文集合
        /// </summary>
        public IList<Post> PostList { get; set; }
    }
}
