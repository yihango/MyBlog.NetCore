using System.Linq;



namespace MyBlog.ViewProjections.Home
{
    /// <summary>
    /// 标签所有文章视图投影类
    /// </summary>
    public class TagPostsViewProjection : IViewProjection<TagPostsBindModel, TagPostsViewModel>
    {
        private readonly BlogDbContext _context;


        public TagPostsViewProjection(BlogDbContext db)
        {
            this._context = db;
        }

        /// <summary>
        /// 投影数据到视图模型
        /// </summary>
        /// <param name="input">标签所有文章查询数据</param>
        /// <returns>标签所有文章详情视图模型</returns>
        public TagPostsViewModel Project(TagPostsBindModel input)
        {
            #region 检查数据

            if (null == input || null == input.TagName)
                return new TagPostsViewModel()
                {
                    AllPageNum = -1,
                    PageNum = input.PageNum,
                    Posts = null,
                    TagName = input.TagName
                };

            #endregion


            // 查询的标签
            var tmpTag = _context.Tags.Where(o => o.Value == input.TagName).FirstOrDefault();

            // 关联的博文数据总数量
            var all = this._context.PostTags.Where(o => o.TagId == tmpTag.Id)
                .Select(o => o.PostId)
                .ToList();

            // 查询出标签表中包含 查询标签 的查询
            var postQuery = this._context.Posts
                .Where(o => o.IsPublish && all.Contains(o.Id));

            // 总数量
            var allPostCount = postQuery.Count();

            // 
            var allPageNum = allPostCount / input.Take;
            if (allPostCount % input.Take != 0)
                allPageNum++;

            // 跳过的数据量
            var skip = (input.PageNum - 1) * input.Take;

            // 
            var resList = postQuery.OrderByDescending(o => o.PublishDate)
                .Skip(skip)
                .Take(input.Take)
                .ToList();
            foreach (var item in resList)
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


            return new TagPostsViewModel()
            {
                AllPageNum = allPageNum,
                PageNum = input.PageNum,
                Posts = resList,
                TagName = input.TagName
            };

        }



        public string Code()
        {
            return null;
        }
    }
}
