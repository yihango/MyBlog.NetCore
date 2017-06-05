using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using MyExtensionsLib;
using MyBlog.Core;
using MyBlog.Core.Commands.Admin;
using MyBlog.Web.Filters;
using MyBlog.Web.Common;


namespace MyBlog.Web.Controllers
{
    [LoginCheckFilter]
    public class AdminController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _memoryCache;
        private readonly IViewProjectionFactory _viewProjectionFactory;
        private readonly ICommandInvokerFactory _commandInvokerFactory;

        public AdminController(IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache, IViewProjectionFactory viewProjectionFactory, ICommandInvokerFactory commandInvokerFactory)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._memoryCache = memoryCache;
            this._viewProjectionFactory = viewProjectionFactory;
            this._commandInvokerFactory = commandInvokerFactory;
        }


        #region 后台管理首页

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }

        #endregion



        #region 修改密码

        /// <summary>
        /// 修改密码页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        /// <summary>
        /// 修改密码接口
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordCommand command)
        {
            // 过滤请求数据
            if (null == command)
                return Json(new { code = -1, msg = "Error:数据不正确", url = string.Empty });
            else if (command.NewPassword.IsNullOrWhitespace() || command.ConfirmNewPassword.IsNullOrWhitespace() || command.OldPassword.IsNullOrWhitespace())
                return Json(new { code = -1, msg = "Error:数据不正确", url = string.Empty });
            else if (command.NewPassword != command.ConfirmNewPassword)
                return Json(new { code = -1, msg = "Error:两次输入密码不一致", url = string.Empty });

            // 得到当前登录的账号
            command.UserAccount = HttpContext.User.FindFirst(ClaimTypes.Sid).Value;

            // 执行修改密码命令
            var commandResult = this._commandInvokerFactory.Handle<ChangePasswordCommand, CommandResult>(command);

            // 执行发生错误
            if (!commandResult.IsSuccess)
                return Json(new { code = -1, msg = $"Error:{commandResult.GetErrors()[0]}", url = string.Empty });

            // 执行成功，清除登陆痕迹返回结果
            AccountLoginManager.SetLoginOut(HttpContext);
            return Json(new { code = 1, msg = "Success:修改成功", url = "/Account/Index" });
        }

        #endregion



        #region 更新标签/文章数据

        /// <summary>
        /// 刷新标签数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RefreshTag()
        {
            var commandResult = this._commandInvokerFactory.Handle<UpdateTagCommand, CommandResult>(new UpdateTagCommand() { });

            if (!commandResult.IsSuccess)
                return Json(new { code = -1, msg = $"Error:{commandResult.GetErrors()[0]}", url = string.Empty });

            this.ClearCache(MemoryCacheKeys.Tags);
            return Json(new { code = 1, msg = "Success:刷新标签数据成功", url = string.Empty });
        }

        /// <summary>
        /// 刷新文章数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RefreshPost()
        {
            var commandResult = this._commandInvokerFactory.Handle<CheckPostsCommand, CheckPostsCommandResult>(new CheckPostsCommand() { WebRootPath = this._hostingEnvironment.WebRootPath });

            if (!commandResult.IsSuccess)
                return Json(new { code = -1, msg = $"Error:{commandResult.GetErrors()[0]}", url = string.Empty });

            // 更新标签统计
            var commandResultB = this._commandInvokerFactory.Handle<UpdateTagCommand, CommandResult>(new UpdateTagCommand());
            var tagUpdateMsg = commandResultB.IsSuccess ? "标签更新成功" : "标签统计失败,请手动更新";

            this.ClearCache(MemoryCacheKeys.Tags);
            this.ClearCache(MemoryCacheKeys.RecentPost);
            return Json(new { code = 1, msg = $"Sussess:刷新文章数据成功,共清理 {commandResult.ClearCount} 条异常数据,{tagUpdateMsg}", url = string.Empty });
        }


        /// <summary>
        /// 清除指定的缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        private void ClearCache(string cacheKey)
        {
            this._memoryCache.Remove(cacheKey);
        }

        #endregion



        #region 退出登录

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LoginOut()
        {
            AccountLoginManager.SetLoginOut(HttpContext);
            return RedirectToAction("Index", "Account");
        }

        #endregion
    }
}
