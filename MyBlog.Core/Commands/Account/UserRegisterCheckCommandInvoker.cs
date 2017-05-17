using System.Linq;
using MySqlSugar;
using MyBlog.Models;

namespace MyBlog.Core.Commands.Account
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRegisterCheckCommandInvoker : ICommandInvoker<UserRegisterCheckCommand, CommandResult>
    {
        private readonly IDbSession _db;
        public UserRegisterCheckCommandInvoker(IDbSession db)
        {
            this._db = db;
        }
        public CommandResult Execute(UserRegisterCheckCommand command)
        {
            var queryUserCount = this._db.GetSession().Queryable<user_tb>(DbTableNames.user_tb).Count();
            if (queryUserCount >= 1)
            {
                return new CommandResult();
            }
            return new CommandResult("管理员不存在，可以注册");
        }
    }
}
