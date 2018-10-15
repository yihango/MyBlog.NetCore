using System.ComponentModel.DataAnnotations;

namespace MyBlog.Commands.Account
{
    /// <summary>
    /// 用户登陆命令
    /// </summary>
    public class UserLoginCommand
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required]
        public string Passwrd { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }

        /// <summary>
        /// 登陆之前访问的url地址
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
