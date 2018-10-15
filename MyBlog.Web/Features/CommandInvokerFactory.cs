using System;
using Microsoft.Extensions.DependencyInjection;
using MyBlog;

namespace MyBlog.Web.Features
{
    /// <summary>
    /// 命令处理单元工厂
    /// </summary>
    public class CommandInvokerFactory : ICommandInvokerFactory
    {
        private readonly IServiceProvider _container;

        /// <summary>
        /// 初始化命令处理单元工厂
        /// </summary>
        /// <param name="container">依赖注入容器</param>
        public CommandInvokerFactory(IServiceProvider container)
        {
            this._container = container;
        }

        /// <summary>
        /// 命令处理单元工厂开始处理
        /// </summary>
        /// <typeparam name="TIn">执行的命令 类</typeparam>
        /// <typeparam name="TOut">输出的命令执行结果 类</typeparam>
        /// <param name="input">执行的命令</param>
        /// <returns>命令执行结果</returns>
        public TOut Handle<TIn, TOut>(TIn input)
        {
            var loader = _container.GetService<ICommandInvoker<TIn, TOut>>();
            return loader.Execute(input);
        }
    }
}
