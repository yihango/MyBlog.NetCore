using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyBlog.Core;
using MyBlog.Web.Features;
using System.Reflection;
using MyBlog.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using MyBlog.Web.Test;
using log4net;
using log4net.Repository;
using log4net.Config;

namespace MyBlog.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public static ILoggerRepository Repository { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetFileProvider(env.WebRootFileProvider)
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();

            // Log4Net配置读取
            Repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(Repository, new FileInfo("log4net.config"));
        }



        public void ConfigureServices(IServiceCollection services)
        {
            // 缓存
            services.AddMemoryCache();

            // Session
            services.AddSession();

            // MVC
            services.AddMvc();

           


            #region 读取配置文件并注册

            services.Configure<WebAppConfiguration>(this.Configuration.GetSection("WebAppConfiguration"));

            #endregion


            #region 视图工厂/命令工厂/数据库会话 注册

            RegisterViewProjection(services);

            RegisterCommandInvoker(services);

            RegisterDbSession(services);

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // 全局异常捕获中间件注册
            app.UseErrorHandling();


            // 配置错误页 500

            /* 当出现底层异常时，自定义错误页面不能显示，还是一片空白
             * 这时想到用 MVC 显示自定义错误页面的局限，如果发生的异常导致 MVC 本身不能正常工作，自定义错误页面就无法显示。
             * 针对这个问题进行了改进，针对500错误直接用静态文件的方式进行响应 
             * 参考博文：http://www.cnblogs.com/dudu/p/6004777.html?utm_source=tuicool&utm_medium=referral
             */
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    if (context.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                    {
                        context.Response.ContentType = "text/html";
                        await context.Response.SendFileAsync($"{env.WebRootPath}/Errors/500.html");
                    }
                });
            });
            // 404错误
            app.UseStatusCodePagesWithReExecute("/Error/{0}");


            // 启用日志记录
            LogWriter();



            // 配置session
            app.UseSession(new SessionOptions()
            {
                CookieName = "_coresessionid"// Session名称
            });


            // 配置cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "_auth",// Cookie 验证方案名称，在写cookie时会用到。
                LoginPath = new PathString("/Account/Index"),// 登录页
                AutomaticAuthenticate = true // 是否自动启用验证，如果不启用，则即便客服端传输了Cookie信息，服务端也不会主动解析。除了明确配置了 [Authorize(ActiveAuthenticationSchemes = "上面的方案名")] 属性的地方，才会解析，此功能一般用在需要在同一应用中启用多种验证方案的时候。比如分Area.
            });


            // 配置MVC路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default",
                            template: "{controller}/{action}/{page?}",
                            defaults: new { controller = "Home", action = "Index" });
            });


            // 配置静态文件资源
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
                RequestPath = new PathString("/Contents")

            });
        }

        #region 注册

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
            services.AddScoped(typeof(IDbSession), typeof(MyDbSession));
        }

        #endregion


        /// <summary>
        /// 记录错误日志
        /// </summary>
        private void LogWriter()
        {
            // 开启一个线程，扫描异常信息队列
            System.Threading.ThreadPool.QueueUserWorkItem(a =>
            {
                ILog log4Net = LogManager.GetLogger(Repository.Name, "Global Error :");
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
                            log4Net.Error(exception.ToString());
                        }
                        else
                        {
                            // 若异常对象为空则休眠三秒钟
                            System.Threading.Thread.Sleep(3000);
                        }
                    }
                    else
                    {
                        // 若队列中没有数据则休眠三秒钟
                        System.Threading.Thread.Sleep(3000);
                    }
                }
            });
        }

    }
}
