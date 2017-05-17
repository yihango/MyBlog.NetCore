using System.Linq;
using MySqlSugar;
using MyBlog.Models;

namespace MyBlog.Core.ViewProjections.Home
{
    /// <summary>
    /// 标签查询视图投影类
    /// </summary>
    public class TagsViewProjection : IViewProjection<TagsBindModel, TagsViewModel>
    {
        private readonly IDbSession _db;

        public TagsViewProjection(IDbSession db)
        {
            this._db = db;
        }

        /// <summary>
        /// 投影数据到视图模型
        /// </summary>
        /// <param name="input">标签查询数据</param>
        /// <returns>标签视图模型</returns>
        public TagsViewModel Project(TagsBindModel input)
        {
            var queryTagList = this._db.GetSession()
                        .Queryable<tag_statistics_tb>(DbTableNames.tag_statistics_tb)
                        .ToList();

            return new TagsViewModel()
            {
                TagList = queryTagList
            };
        }
    }
}
