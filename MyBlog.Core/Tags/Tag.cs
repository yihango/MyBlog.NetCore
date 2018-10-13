using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Core.Tags
{
    /// <summary>
    /// 标签
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}
