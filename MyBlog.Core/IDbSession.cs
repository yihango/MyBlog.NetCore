using System;
using System.Collections.Generic;
using System.Text;

using MySqlSugar;

namespace MyBlog.Core
{
    /// <summary>
    /// 数据库会话接口
    /// </summary>
    public interface IDbSession:IDisposable
    {
        /// <summary>
        /// 获取数据库会话对象
        /// </summary>
        /// <returns>返回数据库会话对象</returns>
        SqlSugarClient GetSession();
    }
}
