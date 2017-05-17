using System.Linq;
using MySqlSugar;
using MyBlog.Models;

namespace MyBlog.Core.ViewProjections.AdminPost
{
    /// <summary>
    /// 博文编辑视图投影
    /// </summary>
    public class EditPostViewProjection : IViewProjection<EditPostBindModel, EditPostViewModel>
    {
        private readonly IDbSession _db;

        public EditPostViewProjection(IDbSession db)
        {
            this._db = db;
        }

        /// <summary>
        /// 投影
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public EditPostViewModel Project(EditPostBindModel input)
        {
            // 根据博文编号查询到博文对象
            var queryPost = this._db.GetSession().Queryable<post_tb>(DbTableNames.post_tb).Where(p => p.post_id == input.PostId).FirstOrDefault();

            // 填充到视图模型
            return new EditPostViewModel()
            {
                Post = queryPost
            };
        }
    }
}
