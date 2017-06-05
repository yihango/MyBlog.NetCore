using System;
using System.Collections.Generic;
using System.Linq;
using MySqlSugar;
using MyBlog.Models;

namespace MyBlog.Core.Commands.Admin
{
    public class UpdateTagCommandInvoker : ICommandInvoker<UpdateTagCommand, CommandResult>
    {
        private readonly IDbSession _db;
        public UpdateTagCommandInvoker(IDbSession db)
        {
            this._db = db;
        }
        public CommandResult Execute(UpdateTagCommand command)
        {
             
            try
            {
                // 开始事务
                this._db.GetSession().BeginTran();

                // 统计计数
                List<tag_statistics_tb> insertAndUpdate = new List<tag_statistics_tb>();
                List<string> delete = new List<string>();


                #region 取得已存在的标签集合

                // 取得现有标签统计表中的标签
                var oldTagList = this._db.GetSession().Queryable<tag_statistics_tb>(DbTableNames.tag_statistics_tb)
                    .ToList();

                #endregion


                #region 统计博文标签

                //统计共有多少个博文标签
                var groupTags = this._db.GetSession()
                    .Queryable<post_tag_tb>(DbTableNames.post_tag_tb)
                    .GroupBy(pt => pt.tag_name)
                    .ToList();

                #endregion


                #region 检测是否存在博文标签，不存在则将删除所有已存在的标签数据

                // 如果博文标签没有数据则清空标签统计表
                if (groupTags.Count <= 0)
                {
                    foreach (var item in oldTagList)
                        delete.Add(item.tag_name);
                    goto delete;
                }

                #endregion


                #region 如果存在博文标签，统计博文标签数量

                // 统计标签的数量  
                foreach (var item in groupTags)
                {
                    // 统计数量
                    var tempCount = this._db.GetSession()
                        .Queryable<post_tag_tb>(DbTableNames.post_tag_tb)
                        .Where(pt => pt.tag_name == item.tag_name && pt.pub_state == 1)
                        .Count();
                    if (tempCount >= 1)
                        insertAndUpdate.Add(new tag_statistics_tb()
                        {
                            tag_name = item.tag_name,
                            tag_count = tempCount
                        });
                }

                #endregion


                #region 检测不存在的标签，标记为删除状态

                // 标记已经不存在的标签 为删除
                foreach (var item in oldTagList)
                    if (!groupTags.Any(pt => pt.tag_name == item.tag_name))
                        delete.Add(item.tag_name);

                #endregion


                #region 插入/更新 标签统计数据

                // 如果存在标签统计表中的数据则修改数量，如果不存在标签统计表中的数据则插入
                foreach (var item in insertAndUpdate)
                {
                    if (!(bool)this._db.GetSession().InsertOrUpdate(item))
                    {
                        this._db.GetSession().Insert<tag_statistics_tb>(item);
                    }
                }

                #endregion


                delete:
                #region 删除标签数据

                // 删除长度为0的标签
                this._db.GetSession().SqlQuery<tag_statistics_tb>($"delete from {DbTableNames.tag_statistics_tb} where tag_count=0");
                // 删除已不存在的标签
                this._db.GetSession().Delete<tag_statistics_tb, string>(delete.ToArray());

                #endregion

                // 提交事务
                this._db.GetSession().CommitTran();
                return new CommandResult();
            }
            catch (Exception e)
            {
                // 回滚事务
                this._db.GetSession().RollbackTran();
                return new CommandResult(e.ToString());
            }
        }
    }
}
