using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlSugar;
using MyBlog.Models;

namespace MyBlog.Core.ViewProjections.Home
{
    /// <summary>
    /// 
    /// </summary>
    public class RecentPostViewProjection : IViewProjection<RecentPostBindModel, RecentPostViewModel>
    {
        private readonly IDbSession _db;
        public RecentPostViewProjection(IDbSession db)
        {
            this._db = db;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RecentPostViewModel Project(RecentPostBindModel input)
        {
            var queryRecentPostList = this._db.GetSession()
                        .Queryable<post_tb>(DbTableNames.post_tb)
                        .OrderBy(p => p.post_pub_time, OrderByType.desc)
                        .Where(p => p.post_pub_state == 1 && p.post_pub_time != null)
                        .Take(input.Take)
                        .ToList();


            return new RecentPostViewModel()
            {
                PostList = queryRecentPostList
            };
        }
    }
}
