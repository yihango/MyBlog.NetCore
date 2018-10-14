using System.Linq;



namespace MyBlog.ViewProjections.Home
{
    /// <summary>
    /// 
    /// </summary>
    public class RecentPostViewProjection : IViewProjection<RecentPostBindModel, RecentPostViewModel>
    {
        private readonly BlogDbContext _context;
        public RecentPostViewProjection(BlogDbContext db)
        {
            this._context = db;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RecentPostViewModel Project(RecentPostBindModel input)
        {
            var queryRecentPostList = this._context.Posts
                        .Where(p => p.IsPublish)
                        .OrderByDescending(o=>o.PublishDate)
                        .Take(input.Take)
                        .ToList();


            return new RecentPostViewModel()
            {
                PostList = queryRecentPostList
            };
        }
    }
}
