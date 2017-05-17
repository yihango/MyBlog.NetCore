namespace MyBlog.Core
{
    /// <summary>
    /// 视图投影工厂接口
    /// </summary>
    public interface IViewProjectionFactory
    {
        /// <summary>
        /// 获取视图投影
        /// </summary>
        /// <typeparam name="TIn">查询数据绑定模型 类</typeparam>
        /// <typeparam name="TOut">视图模型 类</typeparam>
        /// <param name="input">查询数据绑定模型</param>
        /// <returns>视图模型</returns>
        TOut GetViewProjection<TIn, TOut>(TIn input);
    }
}
