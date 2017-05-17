namespace MyBlog.Core
{
    /// <summary>
    /// 命令工作接口
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface ICommandInvoker<in TIn, out TOut>
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        TOut Execute(TIn command);
    }
}
