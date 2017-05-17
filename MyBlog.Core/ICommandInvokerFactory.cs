using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Core
{
    /// <summary>
    /// 命令工作工厂接口
    /// </summary>
    public interface ICommandInvokerFactory
    {
        /// <summary>
        /// 命令处理
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        TOut Handle<TIn, TOut>(TIn input);
    }
}