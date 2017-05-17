using System.Linq;
using MySqlSugar;

using MyBlog.Models;

namespace MyBlog.Core.ViewProjections.Home
{
    /// <summary>
    /// 首页视图投影类
    /// </summary>
    public class HomeViewProjection : IViewProjection<HomeBindModel, HomeViewModel>
    {
        private readonly IDbSession _db;
        public HomeViewProjection(IDbSession db)
        {
            this._db = db;
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
            var allCount = this._db.GetSession()
                      .Queryable<post_tb>(DbTableNames.post_tb)
                      .Where(p => p.post_pub_state == 1 && p.post_pub_time != null)
                      .Count();
            // 获取总页数
            allPageNum = allCount / input.Take;
            if (allCount % input.Take != 0)
            {
                allPageNum += 1;
            }


            // 查询博文数据
            var queryPostList = this._db.GetSession()
                      .Queryable<post_tb>(DbTableNames.post_tb)
                      .Where(p => p.post_pub_state == 1 && p.post_pub_time != null)
                      .OrderBy(p => p.post_pub_time, OrderByType.desc)
                      .Skip(skip)
                      .Take(input.Take)
                      .ToList();


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
