using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using MyExtensionsLib;

using MyBlog.Core.Commands.Admin;

namespace MyBlog.Core.Commands.AdminPost
{
    /// <summary>
    /// 编辑博文
    /// </summary>
    public class EditPostCommandInvoker : ICommandInvoker<EditPostCommand, CommandResult>
    {
        private readonly BlogDbContext _context;

        public EditPostCommandInvoker(BlogDbContext db)
        {
            this._context = db;
        }

        public CommandResult Execute(EditPostCommand command)
        {
             
            try
            {

              
                return new CommandResult("更新数据失败");
            }
            catch (Exception e)
            {
              
                return new CommandResult(e.Message);
            }
        }
    }
}
