using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Web.Common
{
    /// <summary>
    /// 缓存使用的键
    /// </summary>
    public class MemoryCacheKeys
    {
        /// <summary>
        /// 标签键
        /// </summary>
        public const string Tags = "tags";
        /// <summary>
        /// 最近博文键
        /// </summary>
        public const string RecentPost = "recentPost";
    }
}
