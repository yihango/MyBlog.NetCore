using System;
using System.Linq;
using MySqlSugar;

namespace MyExtensionsLib.SqlSugar
{
    /// <summary>
    /// 我对 SqlSugar 的扩展类
    /// </summary>
    public static class MySqlSugarExtensions
    {
        /// <summary>
        ///  主查询 Where 字段 in (子查询)
        ///  例: select * from UserInfo where name in (select userName from Msg where userName='admin')
        /// </summary>
        /// <typeparam name="T1">主查询类型</typeparam>
        /// <typeparam name="T2">子查询类型</typeparam>
        /// <param name="queryable">主查询</param>
        /// <param name="queryWhereInField">主查询 in 子查询的字段</param>
        /// <param name="childrenQuery">子查询</param>
        /// <param name="childrenQuerySelectField">子查询要查询的列，只能是一个字段</param>
        /// <returns>返回总查询</returns>
        public static Queryable<T1> WhereIn<T1, T2>(this Queryable<T1> queryable, string queryWhereInField, Queryable<T2> childrenQuery, string childrenQuerySelectField)
        {
            var childrenQuerySql = childrenQuery.Select(childrenQuerySelectField).ToSql();
            var childrenSql = string.Empty;
            foreach (var item in childrenQuerySql.Value)
            {
                var kv = item.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                childrenSql = childrenQuerySql.Key.Replace(kv[0], $"'{kv[1]}'");
            }

            return queryable.Where($"{queryWhereInField} in ({childrenSql})");
        }
    }
}
