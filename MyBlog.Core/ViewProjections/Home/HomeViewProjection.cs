using System.Linq;



namespace MyBlog.Core.ViewProjections.Home
{
    /// <summary>
    /// 首页视图投影类
    /// </summary>
    public class HomeViewProjection : IViewProjection<HomeBindModel, HomeViewModel>
    {
        private readonly BlogDbContext _context;
        public HomeViewProjection(BlogDbContext db)
        {
            this._context = db;
        }

        /// <summary>
        /// 投影数据到视图模型
        /// </summary>
        /// <param name="input">首页查询数据</param>
        /// <returns>首页视图模型</returns>
        public HomeViewModel Project(HomeBindModel input)
        {
            // 总页数
            var allPageNum = 0;
            // 计算要跳过的数量
            var skip = (input.PageNum - 1) * input.Take;
            // 查询总数据量
            var postQuery = this._context.Posts
                      .Where(p => p.IsPublish);
            var allCount = postQuery.Count();
            // 获取总页数
            allPageNum = allCount / input.Take;
            if (allCount % input.Take != 0)
            {
                allPageNum += 1;
            }


            // 查询博文数据
            var queryPostList = postQuery
                      .OrderByDescending(o => o.PublishDate)
                      .Skip(skip)
                      .Take(input.Take)
                      .ToList();

            foreach (var item in queryPostList)
            {
                var tagIdList = this._context.PostTags.Where(o => o.PostId == item.Id)
                    .Select(o => o.TagId)
                    .ToList();

                var tags = this._context.Tags
                    .Where(o => tagIdList.Contains(o.Id))
                    .Select(o => o.Value)
                    .ToList();

                item.SetTags(tags);
            }

            // 返回视图模型
            return new HomeViewModel()
            {
                Posts = queryPostList,
                PageNum = input.PageNum,
                AllPageNum = allPageNum
            };
        }
    }
}
