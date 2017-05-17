namespace MyBlog.Core.ViewProjections.Home
{
    /// <summary>
    /// 近期博文绑定数据模型
    /// </summary>
    public class RecentPostBindModel
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public RecentPostBindModel()
        {
            this.Take = 10;
        }
        /// <summary>
        /// 取的数量
        /// </summary>
        public int Take { get; set; }
    }
}
