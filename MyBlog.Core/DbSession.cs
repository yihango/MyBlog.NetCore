using System;
using MySqlSugar;

namespace MyBlog.Core
{
    public abstract class DbSession : IDbSession
    {
        /// <summary>
        /// 数据库会话对象
        /// </summary>
        protected SqlSugarClient _client;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string _connStr;

        public DbSession(string connStr)
        {
            this._connStr = connStr;
        }

        public virtual SqlSugarClient GetSession()
        {
            // 如果未创建会话则创建数据库会话对象
            if (null == this._client)
            {
                this._client = new SqlSugarClient(this._connStr);
                // sql日志记录功能
                // 在这儿打断点,执行的查询都会进入该断点
                this._client.IsEnableLogEvent = true;
                this._client.LogEventStarting = (sql, pars) =>
                {
                    Console.WriteLine(sql);
                };
            }

            return this._client;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (null != this._client)
                this._client.Dispose();
        }

    }
}
