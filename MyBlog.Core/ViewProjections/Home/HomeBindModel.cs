namespace MyBlog.Core.ViewProjections.Home
{
    /// <summary>
    /// 首页查询数据绑定模型
    /// </summary>
    public class HomeBindModel
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        public HomeBindModel()
        {
            this.Take = 10;
            this.PageNum = 1;
        }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 每页数据数量
        /// </summary>
        public int Take { get; set; }
    }
}
