using MyBlog.Users;


namespace MyBlog.Commands.Account
{
    /// <summary>
    /// 登陆命令执行结果
    /// </summary>
    public class UserLoginCommandResult : CommandResult
    {
        /// <summary>
        /// 初始化登录命令结果(成功)
        /// </summary>
        public UserLoginCommandResult()
            : base()
        { }

        /// <summary>
        /// 初始化登录命令结果(出错)
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        public UserLoginCommandResult(string errorMessage)
            : base(errorMessage)
        { }

        /// <summary>
        /// 登录账号信息
        /// </summary>
        public User UserInfo { get; set; }
    }
}
