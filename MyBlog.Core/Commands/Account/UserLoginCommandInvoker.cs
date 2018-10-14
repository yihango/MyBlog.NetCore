using System.Linq;
using Microsoft.Extensions.Options;
using MyExtensionsLib;

namespace MyBlog.Core.Commands.Account
{
    /// <summary>
    /// 登陆命令执行类
    /// </summary>
    public class UserLoginCommandInvoker : ICommandInvoker<UserLoginCommand, UserLoginCommandResult>
    {
        private readonly BlogDbContext _context;
        private readonly IOptions<AppConfig> _appConfig;

        public UserLoginCommandInvoker(
            BlogDbContext db,
            IOptions<AppConfig> appConfig)
        {
            this._context = db;
            _appConfig = appConfig;
        }

        /// <summary>
        /// 执行登陆命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public UserLoginCommandResult Execute(UserLoginCommand command)
        {
            if (null != command && !command.Account.IsNullOrWhitespace() && !command.Passwrd.IsNullOrWhitespace())
            {
                command.Passwrd = $"{command.Passwrd}{_appConfig.Value.PwdSalt}".GetMd5Hash();
                var queryUser = this._context.Users
                                    .Where(u => u.Account == command.Account 
                                                && u.Password == command.Passwrd);

                if (queryUser.Count() > 0)
                    return new UserLoginCommandResult() { UserInfo = queryUser.FirstOrDefault() };
            }

            return new UserLoginCommandResult("用户名或密码错误!");
        }
    }
}
