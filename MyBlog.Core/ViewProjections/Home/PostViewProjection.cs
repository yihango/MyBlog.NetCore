using System;
using System.Linq;
using MySqlSugar;
using MyBlog.Models;

namespace MyBlog.Core.ViewProjections.Home
{
    /// <summary>
    /// 文章详情视图投影类
    /// </summary>
    public class PostViewProjection : IViewProjection<PostBindModel, PostViewModel>
    {
        private readonly IDbSession _db;
        public PostViewProjection(IDbSession db)
        {
            this._db = db;
        }
        /// <summary>
        /// 投影数据到视图模型
        /// </summary>
        /// <param name="input">文章详情查询数据</param>
        /// <returns>文章详情视图模型</returns>
        public PostViewModel Project(PostBindModel input)
        {
            var queryPost = this._db.GetSession()
                .Queryable<post_tb>(DbTableNames.post_tb)
                .Where(p => p.post_id == input.PostId && p.post_pub_sortTime == input.PostPutSortTime)
                .FirstOrDefault();


            return new PostViewModel() { PostInfo = queryPost, PostTags = queryPost.post_tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) };
        }
    }
}
