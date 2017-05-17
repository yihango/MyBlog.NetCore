namespace MyBlog.Core.ViewProjections.AdminPost
{
    /// <summary>
    /// 所有博文数据绑定模型
    /// </summary>
    public class AllBlogPostBindModel
    {
        /// <summary>
        /// 初始化视图模型
        /// </summary>
        public AllBlogPostBindModel()
        {
            this.PageNum = 1;
            this.Take = 10;
        }

        /// <summary>
        /// 每页取的数量
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { get; set; }

    }
}
