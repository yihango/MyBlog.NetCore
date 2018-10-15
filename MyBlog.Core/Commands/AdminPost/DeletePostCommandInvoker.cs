using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using MyBlog.EFCore;


namespace MyBlog.Commands.AdminPost
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
                using (var transaction = _context.Database.BeginTransaction())
                {
                    // 删除博客
                    var entity = _context.Posts.Where(o => o.Id == command.PostId.Value).FirstOrDefault();
                    _context.Posts.Remove(entity);
                    // 删除关联标签
                    var postTags = _context.PostTags.Where(o => o.PostId == entity.Id);
                    _context.RemoveRange(postTags.ToArray());

                    _context.SaveChanges();
                    transaction.Commit();
                }
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
