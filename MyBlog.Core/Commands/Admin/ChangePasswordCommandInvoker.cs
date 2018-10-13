using System;
using System.Linq;
using MyExtensionsLib;


namespace MyBlog.Core.Commands.Admin
{
    /// <summary>
    /// 修改用户密码的命令工作类
    /// </summary>
    public class ChangePasswordCommandInvoker : ICommandInvoker<ChangePasswordCommand, CommandResult>
    {
        private readonly BlogDbContext _context;
        public ChangePasswordCommandInvoker(BlogDbContext db)
        {
            this._context = db;
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
                var queryUserInfo = this._context.Users.Where(u => u.Account == command.UserAccount).FirstOrDefault();

                if (null == queryUserInfo)
                    return new CommandResult("用户不存在！");
                else if (!queryUserInfo.Password.Equals(command.OldPassword.GetMd5Hash()))
                    return new CommandResult("旧密码错误！");

                using (var transaction = _context.Database.BeginTransaction())
                {
                    queryUserInfo.Password = command.NewPassword.GetMd5Hash();

                    this._context.Users.Update(queryUserInfo);

                    transaction.Commit();
                }
                return new CommandResult();
            }
            catch (Exception e)
            {
                return new CommandResult(e.ToString());
            }
        }
    }
}
