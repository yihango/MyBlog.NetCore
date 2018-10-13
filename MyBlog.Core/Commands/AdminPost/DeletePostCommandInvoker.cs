using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;



namespace MyBlog.Core.Commands.AdminPost
{
    /// <summary>
    /// 删除博文命令工作类
    /// </summary>
    public class DeletePostCommandInvoker : ICommandInvoker<DeletePostCommand, CommandResult>
    {
        private readonly BlogDbContext _context;

        public DeletePostCommandInvoker(BlogDbContext db)
        {
            this._context = db;
        }

        /// <summary>
        /// 从数据库和磁盘上删除博文
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public CommandResult Execute(DeletePostCommand command)
        {

            try
            {

               


                return new CommandResult();
            }
            catch (Exception e)
            {
               
                // 删除失败
                return new CommandResult(e.ToString());
            }
        }
    }
}
