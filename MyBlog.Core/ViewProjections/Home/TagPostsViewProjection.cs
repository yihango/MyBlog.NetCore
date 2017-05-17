using System.Linq;
using System.Collections;
using System.Collections.Generic;

using MySqlSugar;
using MyBlog.Models;
using MyExtensionsLib.SqlSugar;


namespace MyBlog.Core.ViewProjections.Home
{
    /// <summary>
    /// 标签所有文章视图投影类
    /// </summary>
    public class TagPostsViewProjection : IViewProjection<TagPostsBindModel, TagPostsViewModel>
    {
        private readonly IDbSession _db;


        public TagPostsViewProjection(IDbSession db)
        {
            this._db = db;
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


            // 查询出所有匹配的数据总数并计算总页数
            var allCount = this._db.GetSession().Queryable<post_tag_tb>(DbTableNames.post_tag_tb)
                .Where(pt => pt.tag_name == input.TagName).Count();
            var allPageNum = allCount / input.Take;
            if (allCount % input.Take != 0)
                allPageNum++;

            // 跳过的数据量
            var skip = (input.PageNum - 1) * input.Take;

            // 查询出标签表中包含 查询标签 的查询
            var childrenQueryble = this._db.GetSession()
                .Queryable<post_tag_tb>(DbTableNames.post_tag_tb)
                .Where(pt => pt.tag_name == input.TagName);

            // 
            var queryTagPostList = this._db.GetSession()
                .Queryable<post_tb>(DbTableNames.post_tb)
                .WhereIn("post_id", childrenQueryble, "post_id")
                .OrderBy(p => p.post_pub_time, OrderByType.desc)
                .Skip(skip)
                .Take(input.Take)
                .ToList();

            // 若未查询到数据则删除这个标签
            if (queryTagPostList.Count() <= 0)
            {
                this._db.GetSession().Delete(new tag_statistics_tb() { tag_name = input.TagName });
            }


            return new TagPostsViewModel()
            {
                AllPageNum = allPageNum,
                PageNum = input.PageNum,
                Posts = queryTagPostList,
                TagName = input.TagName
            };

        }



        public string Code()
        {
            return null;
        }
    }
}
