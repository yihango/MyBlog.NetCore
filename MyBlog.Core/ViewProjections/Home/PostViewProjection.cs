using System;
using System.Linq;
using MyBlog.EFCore;


namespace MyBlog.ViewProjections.Home
{
    /// <summary>
    /// 文章详情视图投影类
    /// </summary>
    public class PostViewProjection : IViewProjection<PostBindModel, PostViewModel>
    {
        private readonly BlogDbContext _context;
        public PostViewProjection(BlogDbContext db)
        {
            this._context = db;
        }
        /// <summary>
        /// 投影数据到视图模型
        /// </summary>
        /// <param name="input">文章详情查询数据</param>
        /// <returns>文章详情视图模型</returns>
        public PostViewModel Project(PostBindModel input)
        {
            // 查询博客
            var post = this._context.Posts
                .Where(p => p.Id == input.PostId && p.PublishSortDate == input.PostPutSortTime)
                .FirstOrDefault();

            // 
            var tagIdList = this._context.PostTags.Where(o => o.PostId == post.Id)
                .Select(o => o.TagId)
                .ToList();

            var tags = this._context.Tags
                .Where(o => tagIdList.Contains(o.Id))
                .Select(o=>o.Value)
                .ToArray();


            return new PostViewModel() { PostInfo = post, PostTags = tags };
        }
    }
}
