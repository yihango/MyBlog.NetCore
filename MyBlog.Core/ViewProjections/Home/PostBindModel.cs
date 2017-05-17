namespace MyBlog.Core.ViewProjections.Home
{
    /// <summary>
    /// 文章详情数据查询数据模型
    /// </summary>
    public class PostBindModel
    {
        /// <summary>
        /// 博文id
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 博文所属的时间
        /// </summary>
        public string PostPutSortTime { get; set; }
    }
}
