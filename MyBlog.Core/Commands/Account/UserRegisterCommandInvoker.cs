using System;
using MyBlog.Core.Users;
using MyExtensionsLib;


namespace MyBlog.Core.Commands.Account
{
    /// <summary>
    /// 用户注册命令执行
    /// </summary>
    public class UserRegisterCommandInvoker : ICommandInvoker<UserRegisterCommand, UserLoginCommandResult>
    {
        private readonly BlogDbContext _context;

        public UserRegisterCommandInvoker(BlogDbContext db)
        {
            this._context = db;
        }

        /// <summary>
        /// 执行注册命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public UserLoginCommandResult Execute(UserRegisterCommand command)
        {

            try
            {


                if (command.Password != command.ConfirmPassword)
                    return new UserLoginCommandResult("两次输入密码不一致");

                User user = null;
                using (var transaction = _context.Database.BeginTransaction())
                {
                    user = new User();
                    user.Account = command.UserAccount;
                    user.Password = command.Password.GetMd5Hash();

                    this._context.Users.Add(user);

                    this._context.SaveChanges();
                    // 提交事务
                    transaction.Commit();
                }
                return new UserLoginCommandResult() { UserInfo = user };
            }
            catch (Exception e)
            {
                return new UserLoginCommandResult(e.ToString());
            }
        }
    }
}
