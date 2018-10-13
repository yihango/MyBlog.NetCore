using System.Linq;



namespace MyBlog.Core.ViewProjections.AdminPost
{
    /// <summary>
    /// 博文编辑视图投影
    /// </summary>
    public class EditPostViewProjection : IViewProjection<EditPostBindModel, EditPostViewModel>
    {
        private readonly BlogDbContext _context;

        public EditPostViewProjection(BlogDbContext db)
        {
            this._context = db;
        }

        /// <summary>
        /// 投影
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public EditPostViewModel Project(EditPostBindModel input)
        {
            // 根据博文编号查询到博文对象
            var queryPost = this._context.Posts
                .Where(p => p.Id == input.PostId)
                .FirstOrDefault();

            // 填充到视图模型
            return new EditPostViewModel()
            {
                Post = queryPost
            };
        }
    }
}
