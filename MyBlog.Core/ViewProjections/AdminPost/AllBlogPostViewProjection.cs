using System.Linq;
using MySqlSugar;
using MyBlog.Models;

namespace MyBlog.Core.ViewProjections.AdminPost
{
    /// <summary>
    /// 所有博客视图投影工厂
    /// </summary>
    public class AllBlogPostViewProjection : IViewProjection<AllBlogPostBindModel, AllBlogPostViewModel>
    {
        private readonly IDbSession _db;
        public AllBlogPostViewProjection(IDbSession db)
        {
            this._db = db;
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
            var allCount = this._db.GetSession().Queryable<post_tb>(DbTableNames.post_tb)
                            .Select(p => p.post_id)
                            .Count();

            allPageNum = allCount / input.Take;
            if (allCount % input.Take != 0)
            {
                allPageNum += 1;
            }


            // 查询到的结果集合
            var queryPostList = this._db.GetSession()
                           .Queryable<post_tb>(DbTableNames.post_tb)
                           .OrderBy(p => p.post_pub_time, OrderByType.desc)
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
