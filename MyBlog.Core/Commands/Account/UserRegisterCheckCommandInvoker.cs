using System.Linq;


namespace MyBlog.Commands.Account
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRegisterCheckCommandInvoker : ICommandInvoker<UserRegisterCheckCommand, CommandResult>
    {
        private readonly BlogDbContext _context;
        public UserRegisterCheckCommandInvoker(BlogDbContext db)
        {
            this._context = db;
        }
        public CommandResult Execute(UserRegisterCheckCommand command)
        {
            var queryUserCount = this._context.Users.Count();
            if (queryUserCount >= 1)
            {
                return new CommandResult();
            }
            return new CommandResult("管理员不存在，可以注册");
        }
    }
}
