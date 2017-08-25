using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using MyExtensionsLib;
using MyBlog.Core;
using MyBlog.Core.ViewProjections.AdminPost;
using MyBlog.Core.Commands.Admin;
using MyBlog.Core.Commands.AdminPost;
using MyBlog.Models;
using MyBlog.Web.Common;
using MyBlog.Web.Filters;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.Web.Controllers
{
    /// <summary>
    /// 博文管理
    /// </summary>
    [LoginCheckFilter]
    public class AdminPostController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<WebAppConfiguration> _webAppConfiguration;
        private readonly IMemoryCache _memoryCache;
        private readonly IViewProjectionFactory _viewProjectionFactory;
        private readonly ICommandInvokerFactory _commandInvokerFactory;

        public AdminPostController(IHostingEnvironment hostingEnvironment, IOptions<WebAppConfiguration> webAppConfiguration, IMemoryCache memoryCache, IViewProjectionFactory viewProjectionFactory, ICommandInvokerFactory commandInvokerFactory)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._webAppConfiguration = webAppConfiguration;
            this._memoryCache = memoryCache;
            this._viewProjectionFactory = viewProjectionFactory;
            this._commandInvokerFactory = commandInvokerFactory;
        }


        #region 博文管理首页

        /// <summary>
        /// 博文管理首页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index(string page)
        {
            var pageNum = 1;
            int.TryParse(page, out pageNum);

            var viewModel = this._viewProjectionFactory.GetViewProjection<AllBlogPostBindModel, AllBlogPostViewModel>(new AllBlogPostBindModel() { PageNum = pageNum });

            // TODO:使用 return View("Index", viewModel); 找不到视图

            return View("/Views/AdminPost/Index.cshtml", viewModel);
        }

        #endregion


        #region 新博文

        /// <summary>
        /// 新博文页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult NewPost()
        {
            int pageNum = 1;
            var page = HttpContext.Request.Query["page"];
            int.TryParse(page, out pageNum);
            ViewBag.ReturnPageNum = pageNum;
            return View("EditPost", new EditPostViewModel() { Post = null });
        }

        /// <summary>
        /// 新博文提交接口
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult NewPost(NewPostCommand command)
        {
            // 获取当前应用静态文件所在物理路径
            command.WebRootPath = this._hostingEnvironment.WebRootPath;
            // 设置博文保存的相对路径
            command.PostRelativeSavePath = this._webAppConfiguration.Value.settings.PostRelativeSavePath;
            // 执行新建博文命令
            var commandResult = this._commandInvokerFactory.Handle<NewPostCommand, CommandResult>(command);
            if (!commandResult.IsSuccess)
                return Json(new { code = -1, msg = $"Error:{commandResult.GetErrors()[0]}", url = string.Empty });

            // 更新标签统计
            var commandResultB = this._commandInvokerFactory.Handle<UpdateTagCommand, CommandResult>(new UpdateTagCommand());
            var tagUpdateMsg = commandResultB.IsSuccess ? "标签更新成功" : "标签统计失败,请手动更新";

            this.ClearCache();
            return Json(new { code = 1, msg = $"Success:提交成功,{tagUpdateMsg}", url = $"/AdminPost/Index/{command.ReturnPageNum}" });
        }

        #endregion


        #region 编辑博文

        /// <summary>
        /// 编辑博文页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EditPost()
        {
            var postId = HttpContext.Request.Query["post"];
            if (postId.ToString().IsNullOrWhitespace())
                return RedirectToAction("Index", "AdminPost");

            var viewModel = this._viewProjectionFactory.GetViewProjection<EditPostBindModel, EditPostViewModel>(new EditPostBindModel() { PostId = postId });

            int pageNum = 1;
            var page = HttpContext.Request.Query["page"];
            int.TryParse(page, out pageNum);


            // 如果查询结果不为空，那么拼接博文路径为全路径
            if (null != viewModel.Post)
                viewModel.Post.post_path = this._hostingEnvironment.WebRootPath.WinLinuxPathSwitchCombine(viewModel.Post.post_path);
            else if (null == viewModel.Post)
                return Redirect($"/AdminPost/Index/{pageNum}");


            ViewBag.ReturnPageNum = pageNum;
            return View("EditPost", viewModel);
        }

        /// <summary>
        /// 编辑博文提交接口
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditPost(EditPostCommand command)
        {
            // 设置当前应用静态文件所在目录并执行提交编辑
            command.WebRootPath = this._hostingEnvironment.WebRootPath;
            var commandResultA = this._commandInvokerFactory.Handle<EditPostCommand, CommandResult>(command);
            // 判断是否保存成功
            if (!commandResultA.IsSuccess)
                return Json(new { code = -1, msg = $"Error:{commandResultA.GetErrors()[0]}", url = string.Empty });

            // 更新标签统计
            var commandResultB = this._commandInvokerFactory.Handle<UpdateTagCommand, CommandResult>(new UpdateTagCommand());
            var tagUpdateMsg = commandResultB.IsSuccess ? "标签更新成功" : "标签统计失败,请手动更新";

            this.ClearCache();
            return Json(new { code = 1, msg = $"Success:提交成功,{tagUpdateMsg}", url = $"/AdminPost/Index/{command.ReturnPageNum}" });
        }

        #endregion


        #region 删除博文

        /// <summary>
        /// 删除博文
        /// </summary>
        [HttpGet]
        [HttpPost]
        public IActionResult DeletePost()
        {
            #region 获取请求数据进行过滤

            int pageNum = 1;
            var page = HttpContext.Request.Query["page"];
            int.TryParse(page, out pageNum);

            var postId = HttpContext.Request.Query["post"];
            if (postId.ToString().IsNullOrWhitespace())
                return Json(new { code = -1, msg = "Error:数据错误", url = string.Empty });

            #endregion

            // 执行删除命令
            var commandResult = this._commandInvokerFactory.Handle<DeletePostCommand, CommandResult>(new DeletePostCommand() { PostId = postId, WebRootPath = this._hostingEnvironment.WebRootPath });

            // 判断执行结果
            if (!commandResult.IsSuccess)
                return Json(new { code = -1, msg = $"Error:{commandResult.GetErrors()[0]}", url = string.Empty });

            // 更新标签统计
            var commandResultB = this._commandInvokerFactory.Handle<UpdateTagCommand, CommandResult>(new UpdateTagCommand());
            var tagUpdateMsg = commandResultB.IsSuccess ? "标签更新成功" : "标签统计失败,请手动更新";

            this.ClearCache();
            // Get 请求重定向
            if (HttpContext.Request.Method.Equals("get", System.StringComparison.CurrentCultureIgnoreCase))
                return Redirect($"/AdminPost/Index/{pageNum}");

            // Post 请求返回Json数据
            return Json(new { code = -1, msg = $"Success:删除成功,{tagUpdateMsg}", url = $"/AdminPost/Index/{pageNum}" });
        }

        #endregion


        /// <summary>
        /// 清空缓存
        /// </summary>
        private void ClearCache()
        {
            this._memoryCache.Remove(MemoryCacheKeys.Tags);
            this._memoryCache.Remove(MemoryCacheKeys.RecentPost);
        }
    }
}
