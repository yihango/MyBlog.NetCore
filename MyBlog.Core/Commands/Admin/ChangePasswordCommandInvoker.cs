using System;
using System.Linq;
using Microsoft.Extensions.Options;
using MyExtensionsLib;


namespace MyBlog.Core.Commands.Admin
{
    /// <summary>
    /// 修改用户密码的命令工作类
    /// </summary>
    public class ChangePasswordCommandInvoker : ICommandInvoker<ChangePasswordCommand, CommandResult>
    {
        private readonly BlogDbContext _context;
        private readonly IOptions<AppConfig> _appConfig;

        public ChangePasswordCommandInvoker(
            BlogDbContext db,
            IOptions<AppConfig> appConfig)
        {
            this._context = db;
            _appConfig = appConfig;
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
                var user = this._context.Users.Where(u => u.Account == command.UserAccount).FirstOrDefault();

                if (null == user)
                    return new CommandResult("用户不存在！");
                else if (!user.Password.Equals(command.OldPassword.GetMd5Hash()))
                    return new CommandResult("旧密码错误！");

                using (var transaction = _context.Database.BeginTransaction())
                {
                    user.Password = $"{command.NewPassword}{_appConfig.Value.PwdSalt}".GetMd5Hash();

                    this._context.Users.Update(user);

                    this._context.SaveChanges();
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
