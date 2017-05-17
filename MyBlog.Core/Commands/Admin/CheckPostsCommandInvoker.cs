using System.IO;
using System.Linq;
using System.Collections.Generic;
using MySqlSugar;
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
            IDictionary<string, int> dirPaths = null;
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
                    foreach (var item in queryRes)
                        if (!File.Exists(Path.Combine(command.WebRootPath, item.post_path)))
                        {
                            if (null == dirPaths)
                                dirPaths = new Dictionary<string, int>();
                            dirPaths.Add(Path.GetDirectoryName(item.post_path), 0);

                            this._db.GetSession().Delete(item);
                            clearCount++;
                        }
                command.PageNum += 1;
            }

            // 删除多余文件夹
            if (null != dirPaths)
                foreach (var item in dirPaths)
                    if (Directory.Exists(item.Key) && Directory.GetFiles(item.Key).Length <= 0)
                        Directory.Delete(item.Key);



            return new CheckPostsCommandResult() { ClearCount = clearCount };
        }
    }
}
