using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Core.PostTags
{
    /// <summary>
    /// 文章和标签的关联表
    /// </summary>
    public class PostTag
    {
        public long Id { get; set; }

        /// <summary>
        /// 标签Id
        /// </summary>
        public long TagId { get; set; }

        /// <summary>
        /// 文章id
        /// </summary>
        public long PostId { get; set; }
    }
}
