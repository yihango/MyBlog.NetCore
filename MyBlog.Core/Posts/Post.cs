using MyBlog.Tags;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Posts
{
    /// <summary>
    /// 博客实体
    /// </summary>
    public class Post
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// 发布的短时间
        /// </summary>
        public string PublishSortDate { get; set; }

        /// <summary>
        /// 博文内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 博文内容(markdwon)
        /// </summary>
        public string ContentMD { get; set; }

        /// <summary>
        /// 是否已发布
        /// </summary>
        public bool IsPublish { get; set; }


        private List<string> Tags;

        public void SetTags (List<string> tags)
        {
            this.Tags = tags;
        }

        public List<string> GetTags()
        {
            return this.Tags??new List<string>();
        }
    }
}
