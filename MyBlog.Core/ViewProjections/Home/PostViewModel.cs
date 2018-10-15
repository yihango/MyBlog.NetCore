

using MyBlog.Posts;

namespace MyBlog.ViewProjections.Home
{
    /// <summary>
    /// 文章详情视图模型
    /// </summary>
    public class PostViewModel
    {
        /// <summary>
        /// 文章信息
        /// </summary>
        public Post PostInfo { get; set; }


        /// <summary>
        /// 标签
        /// </summary>
        public string[] PostTags { get; set; }

    }
}
