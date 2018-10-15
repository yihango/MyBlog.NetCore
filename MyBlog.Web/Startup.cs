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
using MyBlog;
using MyBlog.Web.Features;
using MyBlog.Web.Common;
using Microsoft.EntityFrameworkCore;
using MyBlog.EFCore;

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
                options.UseSqlite(this.Configuration["appConfig:connStr"]);
            });

            #region 认证Cookie配置

            // https://github.com/aspnet/Security/issues/1310
            // https://docs.microsoft.com/en-us/aspnet/core/migration/1x-to-2x/identity-2x
            var authenticationBuilder = services.AddAuthentication(AppConsts._auth);

            authenticationBuilder.AddCookie(AppConsts._auth, conf =>
            {
                conf.LoginPath = new PathString("/Account/Index");
                conf.AccessDeniedPath = new PathString("/Admin");
            });

            #endregion



            // 注册配置文件
            services.Configure<AppConfig>(this.Configuration.GetSection("appConfig"));


            #region ViewProjection 、 CommandInvoker 注册

            RegisterViewProjection(services);

            RegisterCommandInvoker(services);

            #endregion

            // DbContext注入
            services.AddScoped(typeof(DbContext), typeof(BlogDbContext));
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

            // 确保数据库正确创建
            using (var serviceScop = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScop.ServiceProvider.GetRequiredService<BlogDbContext>();
                context.Database.EnsureCreated();
            }

            // 异常
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            // 启用认证
            app.UseAuthentication();

            // 配置session
            app.UseSession(new SessionOptions()
            {
                Cookie = new CookieBuilder()
                {
                    Name = AppConsts._session_client
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


            // 启用日志记录
            LogWriter();
        }



        #region 视图投影，命令工作，数据库会话  注册

        /// <summary>
        /// 注册 ViewProjection
        /// </summary>
        /// <param name="services"></param>
        private void RegisterViewProjection(IServiceCollection services)
        {
            // 获取程序集中所有实现了IViewProjection接口的类
            var viewProjectTypes = typeof(AppConfig).GetTypeInfo().Assembly
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
        /// 注册 CommandInvoker
        /// </summary>
        /// <param name="services"></param>
        private void RegisterCommandInvoker(IServiceCollection services)
        {

            // 获取程序集中所有实现了IViewProjection接口的类
            var commandInvokerTypes = typeof(AppConfig).GetTypeInfo().Assembly
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
                    if (Global.ExceptionQueue.Count > 0)
                    {
                        // 将异常对象从队列中拿出来
                        Exception exception = Global.ExceptionQueue.Dequeue();
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
