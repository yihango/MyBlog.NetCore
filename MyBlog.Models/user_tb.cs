namespace MyBlog.Models
{
    /// <summary>
    /// 用户模型
    /// </summary>
    public partial class user_tb
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string user_account { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string user_pwd { get; set; }
    }
}
