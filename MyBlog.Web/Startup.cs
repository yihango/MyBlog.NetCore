using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using MyBlog.Core;
using MyBlog.Web.Features;
using MyBlog.Web.Middlewares;
using MyBlog.Web.Common;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public ILoggerFactory LoggerFactory { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetFileProvider(env.WebRootFileProvider)
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();

        }


        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // 缓存
            services.AddMemoryCache();

            // Session
            services.AddSession();

            // MVC
            services.AddMvc();



            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseSqlite(this.Configuration["appConfig:dbConnStr"]);
            });

            #region 认证Cookie配置

            // https://github.com/aspnet/Security/issues/1310
            // https://docs.microsoft.com/en-us/aspnet/core/migration/1x-to-2x/identity-2x
            var authenticationBuilder = services.AddAuthentication(Global._auth);

            authenticationBuilder.AddCookie(Global._auth, conf =>
            {
                conf.LoginPath = new PathString("/Account/Index");
                conf.AccessDeniedPath = new PathString("/Admin");
            });

            #endregion



            #region 读取配置文件并注册

            services.Configure<AppConfig>(this.Configuration.GetSection("appConfig"));
         
            #endregion





            #region 视图工厂/命令工厂/数据库会话 注册

            RegisterViewProjection(services);

            RegisterCommandInvoker(services);

            this.RegisterDbSession(services);

            #endregion

        }


        /// <summary>
        /// 配置Http请求管道
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // 编码注册
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


            loggerFactory.AddNLog();

            #region 异常捕获，日志记录，错误页跳转 配置

            // 全局异常捕获中间件注册
            app.UseErrorHandling();

            #region 暂时不用的

            //// 配置错误页 500

            ///* 当出现底层异常时，自定义错误页面不能显示，还是一片空白
            // * 这时想到用 MVC 显示自定义错误页面的局限，如果发生的异常导致 MVC 本身不能正常工作，自定义错误页面就无法显示。
            // * 针对这个问题进行了改进，针对500错误直接用静态文件的方式进行响应 
            // * 参考博文：http://www.cnblogs.com/dudu/p/6004777.html?utm_source=tuicool&utm_medium=referral
            // */
            //app.UseExceptionHandler(errorApp =>
            //{
            //    errorApp.Run(async (context) =>
            //    {
            //        context.Response.StatusCode = 500;
            //        if (context.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            //        {
            //            context.Response.ContentType = "text/html";
            //            await context.Response.SendFileAsync($"{env.WebRootPath}/Errors/500.html");
            //        }
            //    });
            //    errorApp.Run(async context =>
            //    {
            //        context.Response.StatusCode = 500;
            //        if (context.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            //        {
            //            context.Response.ContentType = "text/html";
            //            await context.Response.SendFileAsync($"{env.WebRootPath}/Errors/500.html");
            //        }
            //    });
            //});
            //// 404错误
            //app.UseStatusCodePagesWithReExecute("/Error/{0}"); 
            #endregion


            // 启用日志记录
            LogWriter();

            #endregion



            app.UseAuthentication();


            // 配置session
            app.UseSession(new SessionOptions()
            {
                Cookie = new CookieBuilder()
                {
                    Name = Global._session_client
                }
            });


            // 配置静态文件资源
            app.UseStaticFiles();



            // 配置MVC路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default",
                            template: "{controller}/{action}/{page?}",
                            defaults: new { controller = "Home", action = "Index" });
            });

        }



        #region 视图投影，命令工作，数据库会话  注册

        /// <summary>
        /// 注册视图投影类
        /// </summary>
        /// <param name="services"></param>
        private void RegisterViewProjection(IServiceCollection services)
        {
            // 获取程序集中所有实现了IViewProjection接口的类
            var viewProjectTypes = typeof(IViewProjection<,>).GetTypeInfo().Assembly
                 .DefinedTypes
                 .Select(t => new
                 {
                     Type = t.AsType(),
                     Interface = t.ImplementedInterfaces
                     .FirstOrDefault(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IViewProjection<,>)
                     )
                 }).Where(t => null != t.Interface)
                .ToArray();

            // 注册
            foreach (var item in viewProjectTypes)
            {
                services.AddTransient(item.Interface, item.Type);
            }

            // 
            services.AddTransient<IViewProjectionFactory>(serviceProvider =>
            {
                return new ViewProjectionFactory(serviceProvider);
            });
        }

        /// <summary>
        /// 注册命令执行类
        /// </summary>
        /// <param name="services"></param>
        private void RegisterCommandInvoker(IServiceCollection services)
        {

            // 获取程序集中所有实现了IViewProjection接口的类
            var commandInvokerTypes = typeof(ICommandInvoker<,>).GetTypeInfo().Assembly
                 .DefinedTypes
                 .Select(t => new
                 {
                     Type = t.AsType(),
                     Interface = t.ImplementedInterfaces.FirstOrDefault(
                         i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(ICommandInvoker<,>)
                         )
                 }).Where(t => null != t.Interface)
                .ToArray();

            // 注册
            foreach (var item in commandInvokerTypes)
            {
                services.AddTransient(item.Interface, item.Type);
            }
            // 
            services.AddTransient<ICommandInvokerFactory>(serviceProvider =>
            {
                return new CommandInvokerFactory(serviceProvider);
            });
        }

        /// <summary>
        /// 注册数据库会话
        /// </summary>
        /// <param name="services"></param>
        private void RegisterDbSession(IServiceCollection services)
        {
            services.AddScoped(typeof(DbContext), typeof(BlogDbContext));
        }

        #endregion



        #region 记录错误日志的函数

        /// <summary>
        /// 记录错误日志
        /// </summary>
        private void LogWriter()
        {
            // 开启一个线程，扫描异常信息队列
            System.Threading.ThreadPool.QueueUserWorkItem(a =>
            {
                while (true)
                {
                    // 检查队列中是否存在数据
                    if (ErrorHandlingMiddleware.ExceptionQueue.Count > 0)
                    {
                        // 将异常对象从队列中拿出来
                        Exception exception = ErrorHandlingMiddleware.ExceptionQueue.Dequeue();
                        // 若异常对象不为空则记录日志
                        if (null != exception)
                        {
                            // 记录错误到日志文件
                            NLogHelper.Write("execption", exception.ToString());
                        }
                        else
                        {
                            // 若异常对象为空则休眠5秒钟
                            System.Threading.Thread.Sleep(5000);
                        }
                    }
                    else
                    {
                        // 若队列中没有数据则休眠5秒钟
                        System.Threading.Thread.Sleep(5000);
                    }
                }
            });
        }

        #endregion

    }
}
