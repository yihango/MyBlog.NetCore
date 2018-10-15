using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using MyBlog.EFCore;
using MyBlog.Extensions;

using MyBlog.Commands.Admin;
using MyBlog.PostTags;
using MyBlog.Tags;
using MyBlog.Posts;

namespace MyBlog.Commands.AdminPost
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
                using (var transaction = _context.Database.BeginTransaction())
                {

                    #region 文章

                    var post = _context.Posts.Where(o => o.Id == command.PostId).FirstOrDefault();
                    post.Id = command.PostId;
                    post.IsPublish = command.PubState;
                    if (post.IsPublish && !command.UpdatePublishTime.IsNullOrWhitespace())
                    {
                        post.PublishDate = DateTime.Now;
                        post.PublishSortDate = post.PublishDate.Value.ToString("yyyy-MM-dd");
                    }
                    post.ContentMD = command.PostContentMD;
                    post.Content = command.PostContent;
                    post.Title = command.Title;
                    post.Summary = post.Content.RemoveHtml().Sub(300);


                    _context.Posts.Update(post);

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

                    // 原有关联文章的标签
                    var oldPostTagIdList = _context.PostTags
                        .Where(o => o.PostId == post.Id)
                        .ToList();

                    // 新的关联
                    var newPostTags = new List<PostTag>();

                    // 新的关联与旧的关联的交集
                    var deletePostTags = new List<PostTag>();

                    foreach (var tagId in tagIdList)
                    {
                        //if (oldPostTagIdList.Exists(o => o.PostId == post.Id && o.TagId == tagId))
                        //{
                        //    continue;
                        //}
                        if (newPostTags.Exists(o => o.PostId == post.Id && o.TagId == tagId))
                        {
                            continue;
                        }
                        newPostTags.Add(new PostTag()
                        {
                            PostId = post.Id,
                            TagId = tagId
                        });
                    }

                    foreach (var tag in newTagList)
                    {
                        //if (oldPostTagIdList.Exists(o => o.PostId == post.Id && o.TagId == tag.Id))
                        //{
                        //    continue;
                        //}
                        if (newPostTags.Exists(o => o.PostId == post.Id && o.TagId == tag.Id))
                        {
                            continue;
                        }
                        newPostTags.Add(new PostTag()
                        {
                            PostId = post.Id,
                            TagId = tag.Id
                        });
                    }



                    foreach (var postTag in oldPostTagIdList)
                    {
                        var tmpPostTag = newPostTags.Find(o => o.PostId == postTag.PostId && o.TagId == postTag.TagId);
                        // 如果新的关联里面不存在老的关联
                        if (tmpPostTag == null)
                        {
                            // 添加到要删除的里面
                            deletePostTags.Add(postTag);
                        }
                        else// 老的已经存在了，那么从新的里面移除掉
                        {
                            newPostTags.Remove(tmpPostTag);
                        }
                    }

                    _context.PostTags.RemoveRange(deletePostTags);
                    _context.PostTags.AddRange(newPostTags);
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
