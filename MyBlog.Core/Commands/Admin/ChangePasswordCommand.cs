namespace MyBlog.Core.Commands.Admin
{
    /// <summary>
    /// 修改用户密码命令
    /// </summary>
    public class ChangePasswordCommand
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// 确认新密码
        /// </summary>
        public string ConfirmNewPassword { get; set; }

        /// <summary>
        /// 旧的密码
        /// </summary>
        public string OldPassword { get; set; }
    }
}
