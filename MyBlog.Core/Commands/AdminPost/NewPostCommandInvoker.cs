using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using MyExtensionsLib;



using MyBlog.Core.Commands.Admin;

namespace MyBlog.Core.Commands.AdminPost
{
    public class NewPostCommandInvoker : ICommandInvoker<NewPostCommand, CommandResult>
    {
        private readonly BlogDbContext _context;
        public NewPostCommandInvoker(BlogDbContext db)
        {
            this._context = db;
        }



        /// <summary>
        /// 执行插入新博文命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public CommandResult Execute(NewPostCommand command)
        {
            var savePath = string.Empty;

            try
            {
              



                return new CommandResult();
            }
            catch (Exception e)
            {
               
                return new CommandResult(e.Message);
            }
        }
    }
}
