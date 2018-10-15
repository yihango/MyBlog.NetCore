using MyBlog.TagStatistics;
using System.Collections.Generic;
using System.Linq;
using MyBlog.EFCore;


namespace MyBlog.ViewProjections.Home
{
    /// <summary>
    /// 标签查询视图投影类
    /// </summary>
    public class TagsViewProjection : IViewProjection<TagsBindModel, TagsViewModel>
    {
        private readonly BlogDbContext _context;

        public TagsViewProjection(BlogDbContext db)
        {
            this._context = db;
        }

        /// <summary>
        /// 投影数据到视图模型
        /// </summary>
        /// <param name="input">标签查询数据</param>
        /// <returns>标签视图模型</returns>
        public TagsViewModel Project(TagsBindModel input)
        {
            var resList = new List<TagStatistic>();

            var all = _context.PostTags.GroupBy(o => o.TagId).Select(o => o.Key);

            foreach (var item in all)
            {
                var tmpCount = _context.PostTags.Count(o => o.TagId == item);

                var tmpValue = _context.Tags.Where(o => o.Id == item).FirstOrDefault()
                    .Value;

                resList.Add(new TagStatistic()
                {
                    Id=item,
                    Value = _context.Tags.Where(o => o.Id == item).FirstOrDefault()
                    .Value,
                    Count= _context.PostTags.Count(o => o.TagId == item)

                });
            }

            return new TagsViewModel()
            {
                TagList = resList
            };
        }
    }
}
