using System;
using MySqlSugar;

using MyExtensionsLib;
using MyBlog.Models;

namespace MyBlog.Core.Commands.Admin
{
    /// <summary>
    /// 修改用户密码的命令工作类
    /// </summary>
    public class ChangePasswordCommandInvoker : ICommandInvoker<ChangePasswordCommand, CommandResult>
    {
        private readonly IDbSession _db;
        public ChangePasswordCommandInvoker(IDbSession db)
        {
            this._db = db;
        }

        /// <summary>
        /// 执行修改密码操作
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public CommandResult Execute(ChangePasswordCommand command)
        {
            try
            {
                var queryUserInfo = this._db.GetSession().Queryable<user_tb>(DbTableNames.user_tb)
                     .Where(u => u.user_account == command.UserAccount ).FirstOrDefault();

                if (null == queryUserInfo)
                    return new CommandResult("用户不存在！");
                else if(!queryUserInfo.user_pwd.Equals(command.OldPassword.GetMd5Hash()))
                    return new CommandResult("旧密码错误！");

                queryUserInfo.user_pwd = command.NewPassword.GetMd5Hash();

                this._db.GetSession().Update(queryUserInfo);

                return new CommandResult();
            }
            catch (Exception e)
            {
                return new CommandResult(e.ToString());
            }
        }
    }
}
