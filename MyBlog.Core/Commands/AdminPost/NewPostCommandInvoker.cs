using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using MyExtensionsLib;

using MySqlSugar;
using MyBlog.Models;
using MyBlog.Core.Commands.Admin;

namespace MyBlog.Core.Commands.AdminPost
{
    public class NewPostCommandInvoker : ICommandInvoker<NewPostCommand, CommandResult>
    {
        private readonly IDbSession _db;
        public NewPostCommandInvoker(IDbSession db)
        {
            this._db = db;
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

                #region 过滤标题中无法存储为文件名的字符并转换为拼音

                // 处理标签中的多余字符串生成已处理过的字符串
                string[] chars = { @"\", "/", ":", "*", "?", "#", "<", ">", "|", "\"", "&", " " };
                var tempTitle = command.Title;
                foreach (var item in chars)
                    tempTitle = tempTitle.Replace(item, "");

                // 转换为拼音
                tempTitle = tempTitle.ConvertChineseToPY();

                #endregion


                #region 创建时间戳和短时间标记

                // 当前时间
                DateTime dt = DateTime.Now;
                // 创建时间戳
                var timeStamp = dt.ToStamp();
                // 创建短时间标记 ( 年_月_日 )
                var sortTime = dt.ToString("yyyy_MM_dd");
                // 创建博文编号
                var postId = $"{tempTitle}_{timeStamp}";

                #endregion


                #region 保存文件到目录

                // 保存文章的文件夹
                var tempPath = command.PostRelativeSavePath.Replace("{time}", sortTime).WinLinuxPathReplace(command.WebRootPath);

                var dirPath = Path.Combine(command.WebRootPath, tempPath);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                // 保存文章的全路径
                while (true)
                {
                    savePath = Path.Combine(dirPath, $"{postId}.html").WinLinuxPathReplace(command.WebRootPath);
                    if (File.Exists(savePath))
                        postId = $"{tempTitle}_{DateTime.Now.ToStamp()}";
                    else
                        break;
                }

                // 写入到文件
                using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    var buffer = Encoding.UTF8.GetBytes(command.PostContent);
                    fs.Write(buffer, 0, buffer.Length);
                }

                #endregion


                #region 处理文章标签

                List<post_tag_tb> insert = new List<post_tag_tb>();
                StringBuilder sbTags = null;
                if (!command.Tags.IsNullOrWhitespace())
                {
                    sbTags = new StringBuilder();

                    // 裁剪标签遍历
                    foreach (var tag in command.Tags.ToLower().Replace("#", " sharp").Replace("，", ",")
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Distinct())
                    {
                        // 如果不存在则标记为插入
                        // 插入的数据包括文章 编号，标签名，发布状态
                        if (!this._db.GetSession().Queryable<post_tag_tb>(DbTableNames.post_tag_tb)
                            .Any(pt => pt.post_id == postId && pt.tag_name == tag))
                            insert.Add(new post_tag_tb() { post_id = postId, tag_name = tag, pub_state = command.PubState == "1" ? 1 : 0 });
                        sbTags.Append($"{tag},");
                    }
                }

                #endregion


                #region 博文数据存储到数据库

                // 开始事务
                this._db.GetSession().BeginTran();

                // 将内容截取为简介
                var noHtmlPostContent = command.PostContent.RemoveHtml();
                var postSummary = noHtmlPostContent.IsNullOrWhitespace() ? "" :
                    $"{noHtmlPostContent.Substring(0, noHtmlPostContent.Length > 120 ? 120 : noHtmlPostContent.Length - 1)}...";

                var savePost = new post_tb();
                savePost.post_id = postId;
                savePost.post_title = command.Title;
                savePost.post_tags = command.Tags;
                savePost.post_pub_state = int.Parse(command.PubState);
                savePost.post_pub_sortTime = sortTime;
                savePost.post_summary = postSummary;
                // 存储到数据库的是相对路径
                savePost.post_path = Path.Combine(tempPath, Path.GetFileName(savePath)).WinLinuxPathReplace(command.WebRootPath);

                savePost.post_tags = sbTags == null ? string.Empty : sbTags.ToString();
                if (savePost.post_pub_state == 1)
                {
                    savePost.post_pub_time = dt;
                }

                // 插入博文信息到数据库
                this._db.GetSession().Insert(savePost);
                // 插入标签数据到数据库
                this._db.GetSession().InsertRange(insert);


                // 提交事务
                this._db.GetSession().CommitTran();

                #endregion


                return new CommandResult();
            }
            catch (Exception e)
            {
                // 文件存在则删除此文件
                if (File.Exists(savePath)) File.Delete(savePath);
                // 回滚事务
                this._db.GetSession().RollbackTran();

                return new CommandResult(e.Message);
                throw e;
            }
        }
    }
}
