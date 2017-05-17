using System.Linq;
using MySqlSugar;

using MyExtensionsLib;
using MyBlog.Models;

namespace MyBlog.Core.Commands.Account
{
    /// <summary>
    /// 登陆命令执行类
    /// </summary>
    public class UserLoginCommandInvoker : ICommandInvoker<UserLoginCommand, UserLoginCommandResult>
    {
        private readonly IDbSession _db;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        public UserLoginCommandInvoker(IDbSession db)
        {
            this._db = db;
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
                command.Passwrd = command.Passwrd.GetMd5Hash();

                var queryUser = this._db.GetSession().Queryable<user_tb>(DbTableNames.user_tb).Where(u => u.user_account == command.Account && u.user_pwd == command.Passwrd);

                if (queryUser.Count() > 0)
                    return new UserLoginCommandResult() { UserInfo = queryUser.FirstOrDefault() };
            }

            return new UserLoginCommandResult("用户名或密码错误!");
        }
    }
}
