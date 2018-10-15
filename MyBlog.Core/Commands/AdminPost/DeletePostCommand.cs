namespace MyBlog.Commands.AdminPost
{
    /// <summary>
    /// 删除博文命令
    /// </summary>
    public class DeletePostCommand
    {
        /// <summary>
        /// 博文编号
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 当前应用静态文件所在目录
        /// </summary>
        public string WebRootPath { get; set; }
    }
}
