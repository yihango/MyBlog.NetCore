using MyBlog.Core.Posts;
using System.Collections.Generic;


namespace MyBlog.Core.ViewProjections.Home
{
    /// <summary>
    /// 标签所有文章数据视图模型
    /// </summary>
    public class TagPostsViewModel
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 所有页码
        /// </summary>
        public int AllPageNum { get; set; }

        /// <summary>
        /// 能否下一页
        /// </summary>
        public bool HasNext
        {
            get
            {
                return PageNum < AllPageNum;
            }
        }


        /// <summary>
        /// 能否上一页
        /// </summary>
        public bool HasPrev
        {
            get
            {
                return PageNum > 1;
            }
        }

        /// <summary>
        /// 博文集合
        /// </summary>
        public IEnumerable<Post> Posts { get; set; }

        /// <summary>
        /// 标签名
        /// </summary>
        public string TagName { get; set; }
    }
}
