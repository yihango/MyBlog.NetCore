using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyBlog.Web.Test
{
    /// <summary>
    /// 异常捕获中间件
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        public static Queue<Exception> ExceptionQueue { get; set; } = new Queue<Exception>();
        //private ILog log = LogManager.GetLogger(Startup.Repository.Name, "Global Error:");


        private readonly RequestDelegate next;

        /// <summary>
        /// 初始化
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


            //// 获取状态码
            //var code = context.Response.StatusCode;

            //return context.Response.WriteAsync("1");

            return null;
            //// 根据实际需求进行具体实现
            //var msg = exception.ToString();
            //var code = HttpStatusCode.InternalServerError;


            //var result = JsonConvert.SerializeObject(new { code = (int)code, msg = msg });

            //context.Response.ContentType = "application/json";
            //context.Response.StatusCode = (int)code;
            //return context.Response.WriteAsync(result);
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
