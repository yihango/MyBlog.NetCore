using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using MyExtensionsLib;



using MyBlog.Core.Commands.Admin;
using MyBlog.Core.Posts;
using MyBlog.Core.Tags;
using MyBlog.Core.PostTags;

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
           
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {

                    #region 文章

                    var post = new Post();
                    post.IsPublish = command.PubState;
                    if (post.IsPublish)
                    {
                        post.PublishDate = DateTime.Now;
                        post.PublishSortDate = post.PublishDate.Value.ToString("yyyy-mm-dd");
                    }
                    post.Content = command.PostContent;
                    post.Title = command.Title;

                    _context.Posts.Add(post);
                    _context.SaveChanges();

                    #endregion



                    #region 校验标签

                    var tagIdList = new List<long>();
                    var newTagList = new List<Tag>();

                    if (!command.Tags.IsNullOrWhitespace())
                    {
                        var tags = command.Tags.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);

                        // 所有已存在的标签
                        var eAllTags = _context.Tags.Where(o => tags.Contains(o.Value)).ToList();
                        // 
                        foreach (var item in tags)
                        {
                            var tmpTag = eAllTags.Find(o => o.Value == item);
                            // 把已存在标签添加到集合
                            if (tmpTag != null)
                            {
                                tagIdList.Add(tmpTag.Id);
                            }
                            else
                            {
                                newTagList.Add(new Tag()
                                {
                                    Value = item
                                });
                            }
                        }

                        // 创建新的标签
                        this._context.AddRange(newTagList);
                        _context.SaveChanges();
                    }

                    #endregion



                    #region 关联标签

                    var postTags = new List<PostTag>();

                    foreach (var tagId in tagIdList)
                    {
                        if (postTags.Exists(o => o.PostId == post.Id && o.TagId == tagId))
                        {
                            continue;
                        }
                        postTags.Add(new PostTag()
                        {
                            PostId = post.Id,
                            TagId = tagId
                        });
                    }

                    foreach (var tag in newTagList)
                    {
                        if (postTags.Exists(o => o.PostId == post.Id && o.TagId == tag.Id))
                        {
                            continue;
                        }
                        postTags.Add(new PostTag()
                        {
                            PostId = post.Id,
                            TagId = tag.Id
                        });
                    }

                    _context.PostTags.AddRange(postTags);

                    _context.SaveChanges(); 

                    #endregion



                    transaction.Commit();
                }

                return new CommandResult();
            }
            catch (Exception e)
            {

                return new CommandResult(e.Message);
            }
        }
    }
}
