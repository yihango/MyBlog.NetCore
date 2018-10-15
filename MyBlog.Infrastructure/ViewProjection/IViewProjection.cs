namespace MyBlog
{
    /// <summary>
    /// 查询数据，将数据投影到视图 接口
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IViewProjection<TIn, TOut>
    {
        /// <summary>
        /// 将数据投影到视图
        /// </summary>
        /// <param name="input">查询数据</param>
        /// <returns>视图模型</returns>
        TOut Project(TIn input);
    }
}