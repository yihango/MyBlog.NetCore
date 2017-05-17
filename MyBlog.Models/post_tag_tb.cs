namespace MyBlog.Models
{
    /// <summary>
    /// 文章标签表
    /// </summary>
    public partial class post_tag_tb
    {
        /// <summary>
        /// 文章标签id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 文章编号(名称)
        /// </summary>
        public string post_id { get; set; }

        /// <summary>
        /// 标签编号(名称)
        /// </summary>
        public string tag_name { get; set; }

        /// <summary>
        /// 发布状态 （1为已发布，0为未发布）
        /// </summary>
        public int pub_state { get; set; }
    }
}