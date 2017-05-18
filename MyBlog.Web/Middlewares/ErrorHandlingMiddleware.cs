using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace MyBlog.Web.Middlewares
{
    /// <summary>
    /// 异常捕获中间件
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        /// <summary>
        /// 全局的错误队列
        /// </summary>
        public static Queue<Exception> ExceptionQueue { get; set; } = new Queue<Exception>();

        // 请求委托
        private readonly RequestDelegate next;

        /// <summary>
        /// 初始化错误中间件
        /// </summary>
        /// <param name="next"></param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// 执行请求，捕获异常
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                // 捕获异常
                await HandleExceptionAsync(context, ex);
            }
        }


        /// <summary>
        /// 捕获到异常执行的操作
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // 将异常添加到异常队列中
            ExceptionQueue.Enqueue(exception);

            return Task.Run(() =>
            {
                context.Response.Redirect("/Error/{0}");
            });
        }
    }

    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
