using MyBlog.Models;

namespace MyBlog.Core.ViewProjections.AdminPost
{
    /// <summary>
    /// 博文编辑视图模型
    /// </summary>
    public class EditPostViewModel
    {
        /// <summary>
        /// 博文
        /// </summary>
        public post_tb Post { get; set; }
    }
}
