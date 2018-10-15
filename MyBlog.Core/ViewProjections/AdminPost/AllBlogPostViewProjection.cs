using System.Linq;
using MyBlog.EFCore;


namespace MyBlog.ViewProjections.AdminPost
{
    /// <summary>
    /// 所有博客视图投影工厂
    /// </summary>
    public class AllBlogPostViewProjection : IViewProjection<AllBlogPostBindModel, AllBlogPostViewModel>
    {
        private readonly BlogDbContext _context;
        public AllBlogPostViewProjection(BlogDbContext db)
        {
            this._context = db;
        }

        /// <summary>
        /// 投影
        /// </summary>
        /// <param name="input">所有博文数据绑定模型</param>
        /// <returns></returns>
        public AllBlogPostViewModel Project(AllBlogPostBindModel input)
        {
            int allPageNum = 0;
            // 计算要跳过的数量
            var skip = (input.PageNum - 1) * input.Take;

            // 计算总页数
            var postQuery = this._context.Posts.AsQueryable();
            var allCount = postQuery.Count();



            allPageNum = allCount / input.Take;
            if (allCount % input.Take != 0)
            {
                allPageNum += 1;
            }


            // 查询到的结果集合
            var queryPostList = postQuery
                           .OrderByDescending(o => o.PublishDate)
                           .Skip(skip).Take(input.Take)
                           .ToList();


            // 填充数据到视图模型
            return new AllBlogPostViewModel()
            {
                Posts = queryPostList,
                PageNum = input.PageNum,
                AllPageNum = allPageNum
            };
        }
    }
}
