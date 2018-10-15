namespace MyBlog.Commands.AdminPost
{
    /// <summary>
    /// 新博文命令
    /// </summary>
    public class NewPostCommand
    {
        /// <summary>
        /// 博文标题
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// 博文标签
        /// </summary>
        public string Tags { get; set; } = string.Empty;

        /// <summary>
        /// 博文内容
        /// </summary>
        public string PostContent { get; set; }

        public string PostContentMD { get; set; }

        /// <summary>
        /// 是否发布（两个值，true=1，false=0）
        /// </summary>
        public bool PubState { get; set; }


        /// <summary>
        /// 真实地址
        /// </summary>
        public string WebRootPath { get; set; }

        /// <summary>
        /// 博文保存的相对路径(相对于根目录)
        /// </summary>
        public string PostRelativeSavePath { get; set; }

        /// <summary>
        /// 之前的页码
        /// </summary>
        public int ReturnPageNum { get; set; }
    }
}
