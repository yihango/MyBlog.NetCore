using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using MySqlSugar;
using MyExtensionsLib;
using MyBlog.Models;
using MyBlog.Core.Commands.Admin;

namespace MyBlog.Core.Commands.AdminPost
{
    /// <summary>
    /// 编辑博文
    /// </summary>
    public class EditPostCommandInvoker : ICommandInvoker<EditPostCommand, CommandResult>
    {
        private readonly IDbSession _db;

        public EditPostCommandInvoker(IDbSession db)
        {
            this._db = db;
        }

        public CommandResult Execute(EditPostCommand command)
        {
            try
            {

                #region 查询文章原来的数据

                var tempPost = this._db.GetSession().Queryable<post_tb>(DbTableNames.post_tb)
                               .Where(p => p.post_id == command.PostId)
                               .FirstOrDefault();

                if (tempPost == null)
                {
                    return new CommandResult("未找到数据");
                }

                #endregion


                #region 处理标签数据

                List<post_tag_tb> insert = new List<post_tag_tb>();
                List<post_tag_tb> update = new List<post_tag_tb>();
                List<int> delete = new List<int>();

                StringBuilder sbTags = null;
                // 要更新和插入的标签数据
                if (!command.Tags.IsNullOrWhitespace())
                {
                    sbTags = new StringBuilder();

                    // 裁剪标签遍历
                    foreach (var tag in command.Tags.ToLower().Replace("#", "sharp").Replace("，", ",")
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Distinct())
                    {
                        var tempQuery = this._db.GetSession().Queryable<post_tag_tb>(DbTableNames.post_tag_tb);
                        // 如果不存在则标记为插入
                        // 插入的数据包括文章 编号，标签名，发布状态
                        if (!tempQuery.Any(pt => pt.post_id == command.PostId && pt.tag_name == tag))
                        {
                            insert.Add(new post_tag_tb() { post_id = command.PostId, tag_name = tag, pub_state = command.PubState == "1" ? 1 : 0 });
                        }
                        else// 如果已存在则修改发布状态
                        {
                            var tempPostTag = tempQuery.Where(pt => pt.post_id == command.PostId && pt.tag_name == tag).FirstOrDefault();
                            tempPostTag.pub_state = command.PubState == "1" ? 1 : 0;
                            update.Add(tempPostTag);
                        }
                        sbTags.Append($"{tag},");
                    }
                }

                // 要删除的标签数据
                // 原来的数据标签
                var oldTagList = this._db.GetSession()
                    .Queryable<post_tag_tb>(DbTableNames.post_tag_tb)
                    .Where(pt => pt.post_id == command.PostId)
                    .ToList();
                // 要删除的标签数据
                foreach (var item in oldTagList)
                {
                    if (!insert.Any(pt => pt.tag_name == item.tag_name) && !update.Any(pt => pt.tag_name == item.tag_name))
                        delete.Add(item.id);
                }

                #endregion


                #region 写入到文件

                // 检查文件夹是否存在,不存在创建新文件夹
                var savePath = Path.Combine(command.WebRootPath, tempPost.post_path).WinLinuxPathReplace(command.WebRootPath);
                string dirPath = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                // 写入文件内容到文件
                using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    var buffer = Encoding.UTF8.GetBytes(command.PostContent);
                    fs.Write(buffer, 0, buffer.Length);
                }

                #endregion


                #region 博文数据存储到数据库

                // 将内容截取为简介
                var noHtmlPostContent = command.PostContent.RemoveHtml();
                var postSummary = $"{noHtmlPostContent.Substring(0, noHtmlPostContent.Length > 120 ? 120 : noHtmlPostContent.Length - 1)}...";

                // 填充数据
                if (tempPost.post_pub_state == 0 && int.Parse(command.PubState) == 1)
                    tempPost.post_pub_time = DateTime.Now;
                tempPost.post_pub_state = int.Parse(command.PubState);
                tempPost.post_title = command.Title;
                tempPost.post_tags = sbTags == null ? string.Empty : sbTags.ToString();
                tempPost.post_summary = postSummary;

                // 更新博文成功
                if (this._db.GetSession().Update(tempPost))
                    return new CommandResult();

                #endregion


                #region 修改标签数据

                // 插入/更新/删除 标签数据到数据库
                this._db.GetSession().SqlBulkCopy(insert);
                this._db.GetSession().SqlBulkReplace(update);
                this._db.GetSession().Delete<post_tag_tb, int>(delete.ToArray());

                #endregion


                #region 统计标签数据

                // 更新标签数据
                var invoker = new UpdateTagCommandInvoker(this._db);
                var commandResult = invoker.Execute(new UpdateTagCommand());

                if (!commandResult.IsSuccess)
                {
                    throw new Exception(commandResult.GetErrors()[0]);
                }
                #endregion


                return new CommandResult("更新数据失败");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
