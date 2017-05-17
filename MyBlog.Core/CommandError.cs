namespace MyBlog.Core
{
    /// <summary>
    /// 命令错误
    /// </summary>
    public class CommandError
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// 初始化命令错误对象
        /// </summary>
        /// <param name="errorMsg">错误信息</param>
        public CommandError(string errorMsg)
        {
            this.Message = errorMsg;
        }
    }
}
