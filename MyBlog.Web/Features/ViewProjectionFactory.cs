using System;
using Microsoft.Extensions.DependencyInjection;
using MyBlog;

namespace MyBlog.Web.Features
{
    /// <summary>
    /// 视图投影工厂
    /// </summary>
    public class ViewProjectionFactory : IViewProjectionFactory
    {
        private readonly IServiceProvider _container;

        /// <summary>
        /// 初始化投影工厂
        /// </summary>
        /// <param name="container">注入的容器</param>
        public ViewProjectionFactory(IServiceProvider container)
        {
            this._container = container;
        }

        /// <summary>
        /// 获取视图
        /// </summary>
        /// <typeparam name="TIn">视图查询数据 类</typeparam>
        /// <typeparam name="TOut">查询视图结果 类</typeparam>
        /// <param name="input">视图查询数据</param>
        /// <returns>查询视图结果</returns>
        public TOut GetViewProjection<TIn, TOut>(TIn input)
        {
            var loader = _container.GetService<IViewProjection<TIn, TOut>>();
            return loader.Project(input);
        }
    }
}
