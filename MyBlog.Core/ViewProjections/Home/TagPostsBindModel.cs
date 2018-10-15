namespace MyBlog.ViewProjections.Home
{
    /// <summary>
    /// 标签所有文章查询模型
    /// </summary>
    public class TagPostsBindModel
    {
        public TagPostsBindModel()
        {
            this.Take = 10;
            this.PageNum = 1;
        }
        /// <summary>
        /// 每页数据
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }
    }
}
