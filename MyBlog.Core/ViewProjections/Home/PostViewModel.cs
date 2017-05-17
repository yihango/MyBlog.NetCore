using System.Collections.Generic;

using MyBlog.Models;

namespace MyBlog.Core.ViewProjections.Home
{
    /// <summary>
    /// 文章详情视图模型
    /// </summary>
    public class PostViewModel
    {
        /// <summary>
        /// 文章信息
        /// </summary>
        public post_tb PostInfo { get; set; }


        /// <summary>
        /// 标签
        /// </summary>
        public string[] PostTags { get; set; }

    }
}
