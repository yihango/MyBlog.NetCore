using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using MySqlSugar;
using MyBlog.Models;

namespace MyBlog.Core.Commands.AdminPost
{
    /// <summary>
    /// 删除博文命令工作类
    /// </summary>
    public class DeletePostCommandInvoker : ICommandInvoker<DeletePostCommand, CommandResult>
    {
        private readonly IDbSession _db;

        public DeletePostCommandInvoker(IDbSession db)
        {
            this._db = db;
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

                #region 查询博文原信息

                // 查询原本的博文信息
                var tempPost = this._db.GetSession().Queryable<post_tb>(DbTableNames.post_tb)
                               .Where(p => p.post_id == command.PostId)
                               .FirstOrDefault();

                // 若不存在数据则表示删除成功
                if (null == tempPost)
                    return new CommandResult();

                #endregion



                #region 从数据库中删除博文信息，删除失败抛出异常

                // 删除数据库中的博文信息
                if (!this._db.GetSession().Delete<post_tb>(p => p.post_id == command.PostId))
                    throw new Exception("从数据库中删除博文失败");

                #endregion



                #region 删除/更新标签

                List<tag_statistics_tb> updateTagList = new List<tag_statistics_tb>();
                List<tag_statistics_tb> deleteTagList = new List<tag_statistics_tb>();
                // 删除/更新 标签
                foreach (var tag in tempPost.post_tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct())
                {
                    var temp = this._db.GetSession().Queryable<tag_statistics_tb>(DbTableNames.tag_statistics_tb).Where(t => t.tag_name == tag);
                    if (temp.Count() >= 1)
                    {
                        var tempTag = temp.FirstOrDefault();
                        tempTag.tag_count -= 1;
                        if (tempTag.tag_count >= 1)
                            updateTagList.Add(tempTag);
                        else
                            deleteTagList.Add(tempTag);
                    }
                }

                this._db.GetSession().SqlBulkReplace(updateTagList);
                this._db.GetSession().Delete(deleteTagList);

                #endregion



                #region 从磁盘上删除文件

                // 文件存在则删除文件
                var filePath = Path.Combine(command.WebRootPath, tempPost.post_path);
                if (File.Exists(filePath))
                    File.Delete(filePath);

                #endregion



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
