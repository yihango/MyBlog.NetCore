using System.IO;
using System.Linq;
using System.Collections.Generic;
using MySqlSugar;
using MyExtensionsLib;
using MyBlog.Models;

namespace MyBlog.Core.Commands.Admin
{
    /// <summary>
    /// 检查异常博文工作类
    /// </summary>
    public class CheckPostsCommandInvoker : ICommandInvoker<CheckPostsCommand, CheckPostsCommandResult>
    {
        private readonly IDbSession _db;
        public CheckPostsCommandInvoker(IDbSession db)
        {
            this._db = db;
        }

        /// <summary>
        /// 执行检查命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public CheckPostsCommandResult Execute(CheckPostsCommand command)
        {
            try
            {
                var deletePostList = new List<post_tb>();
                List<string> dirPaths = null;
                int skip = 0, clearCount = 0;
                // 计算总页数
                var allCount = this._db.GetSession().Queryable<post_tb>(DbTableNames.post_tb).Count();
                command.AllPageNum = allCount / command.Take;
                if (allCount % command.Take != 0) command.AllPageNum += 1;

                // 是否能下一页
                while (command.HasNext || command.PageNum == 1)
                {
                    // 跳过的数据量
                    skip = (command.PageNum - 1) * command.Take;

                    var queryRes = this._db.GetSession().Queryable<post_tb>(DbTableNames.post_tb)
                        .OrderBy(p => p.post_pub_time, OrderByType.desc)
                        .Skip(skip)
                        .Take(command.Take)
                        .ToList();

                    // 遍历检测博文文件是否存在，不存在则从数据库中删除数据
                    if (queryRes != null || queryRes.Count > 0)
                    {
                        foreach (var item in queryRes)
                        {
                            // 如果文件不存在
                            var filePath = Path.Combine(command.WebRootPath, item.post_path).WinLinuxPathReplace(command.WebRootPath);
                            if (!File.Exists(filePath))
                            {
                                if (null == dirPaths) dirPaths = new List<string>();
                                // 异常的文章的目录路径
                                if (!dirPaths.Any(path => path == filePath))
                                    dirPaths.Add(Path.GetDirectoryName(filePath));
                                // 标识为要删除的项目
                                deletePostList.Add(item);
                                clearCount++;
                            }
                        }
                    }
                    command.PageNum += 1;
                }

                // 删除多余文件夹
                if (null != dirPaths)
                {
                    foreach (var item in dirPaths)
                    {
                        if (Directory.Exists(item) && Directory.GetFiles(item).Length <= 0)
                            Directory.Delete(item);
                    }
                }

                #region 删除数据

                if (deletePostList.Count > 0)
                {
                    // 开始事务
                    this._db.GetSession().BeginTran();
                    // 删除关联的标签数据
                    var sb = new System.Text.StringBuilder();
                    foreach (var item in deletePostList)
                        sb.AppendFormat("'{0}',", item.post_id);
                    var sql = sb.ToString().TrimEnd(',');
                    if (!sql.IsNullOrWhitespace())
                        this._db.GetSession().Delete<post_tag_tb>($"post_id in ({sql})");

                    // 删除博文数据
                    this._db.GetSession().Delete(deletePostList);
                    // 提交事务
                    this._db.GetSession().CommitTran();
                }

                #endregion

                return new CheckPostsCommandResult() { ClearCount = clearCount };
            }
            catch (System.Exception e)
            {
                // 回滚事务
                this._db.GetSession().RollbackTran();
                return new CheckPostsCommandResult(e.Message);
                throw e;
            }
        }
    }
}
