namespace MyBlog.Core.Commands.Admin
{
    /// <summary>
    /// 异常文章数据检查命令执行结果
    /// </summary>
    public class CheckPostsCommandResult : CommandResult
    {
        public CheckPostsCommandResult()
        {

        }

        public CheckPostsCommandResult(string errorMsg)
            : base(errorMsg)
        {

        }

        /// <summary>
        /// 一共清理了多少条数据
        /// </summary>
        public int ClearCount { get; set; } = 0;
    }
}
