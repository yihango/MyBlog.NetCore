namespace MyBlog.Models{
    /// <summary>
    /// 标签统计类
    /// </summary>
    public partial class tag_statistics_tb
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string tag_name { get; set; }

        /// <summary>
        /// 标签数量
        /// </summary>
        public int tag_count { get; set; }
    }
}
