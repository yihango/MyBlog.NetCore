using MyBlog.Posts;
using System.Collections.Generic;


namespace MyBlog.ViewProjections.AdminPost
{
    /// <summary>
    /// 所有博文视图绑定模型
    /// </summary>
    public class AllBlogPostViewModel
    {
        /// <summary>
        /// 博文集合
        /// </summary>
        public IEnumerable<Post> Posts { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 所有页码
        /// </summary>
        public int AllPageNum { get; set; }

        /// <summary>
        /// 是否能下一页
        /// </summary>
        public bool IsNext
        {
            get
            {
                return PageNum < AllPageNum;
            }
        }

        /// <summary>
        /// 能否上一页
        /// </summary>
        public bool IsPrev
        {
            get
            {
                return PageNum > 1;
            }
        }

    }
}
