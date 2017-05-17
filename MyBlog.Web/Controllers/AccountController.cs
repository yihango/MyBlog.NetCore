using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Authentication;


using MyCommonLib;
using MyExtensionsLib;
using MyBlog.Models;
using MyBlog.Core;
using MyBlog.Core.Commands.Account;
using System;
using MyBlog.Web.Common;
using MyBlog.Web.Filters;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.Web.Controllers
{
    [AccountFilterAttribute]
    public class AccountController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<WebAppConfiguration> _webAppConfiguration;
        private readonly IViewProjectionFactory _viewProjectionFactory;
        private readonly ICommandInvokerFactory _commandInvokerFactory;

        public AccountController(IHostingEnvironment hostingEnvironment, IOptions<WebAppConfiguration> webAppConfiguration, IViewProjectionFactory viewProjectionFactory, ICommandInvokerFactory commandInvokerFactory)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._webAppConfiguration = webAppConfiguration;
            this._viewProjectionFactory = viewProjectionFactory;
            this._commandInvokerFactory = commandInvokerFactory;
        }


        #region 登陆

        /// <summary>
        /// 登陆页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            //// 获取 WebApplication 的运行根目录
            //var contentRootPath = this._hostingEnvironment.ContentRootPath;
            //// 获取 WebApplication 的静态文件目录 (wwwroot)
            //var webRootPath = this._hostingEnvironment.WebRootPath;
            //// 获取 WebApplication 的静态文件目录 (root:wwwroot)
            //var webRootFileProvider = this._hostingEnvironment.WebRootFileProvider;

            //return Json(new { ContentRootPath = contentRootPath, webRootPath = webRootPath, WebRootFileProvider = webRootFileProvider });


            if (!this.CheckAccount(this._commandInvokerFactory))
                return RedirectToAction("Register", "Account");


            ViewBag.ReturnUrl = Request.Query["ReturnUrl"];
            return View("Login");
        }

        /// <summary>
        /// 登陆接口
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(UserLoginCommand command)
        {
            if (null == command)
                return Json(new { code = -1, msg = "Error:数据不正确", url = string.Empty });
            else if (command.Account.IsNullOrWhitespace() || command.Passwrd.IsNullOrWhitespace())
                return Json(new { code = -1, msg = "Error:数据不正确", url = string.Empty });
            else if (command.VerifyCode != HttpContext.Session.GetString("verifycode"))
            {
                HttpContext.Session.Remove("verifycode");
                return Json(new { code = -1, msg = "Error:验证码错误", url = string.Empty });
            }



            var commandResult = this._commandInvokerFactory.Handle<UserLoginCommand, UserLoginCommandResult>(command);


            if (!commandResult.IsSuccess)
                return Json(new { code = -1, msg = $"错误:{commandResult.GetErrors()[0]}", url = string.Empty });

            // 登陆验证成功
            HttpContext.Session.Remove("verifycode");
            AccountLoginManager.SetLogin(HttpContext, commandResult);


            return Json(new { code = 1, msg = "Success:登陆成功", url = command.ReturnUrl.IsNullOrWhitespace() ? "/Admin/Index" : command.ReturnUrl });
        }

        #endregion



        #region 注册

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register()
        {
            if (this.CheckAccount(this._commandInvokerFactory))
                return RedirectToAction("Index", "Account");

            return View();
        }


        /// <summary>
        /// 注册接口
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Register(UserRegisterCommand command)
        {
            if (null == command)
                return Json(new { code = -1, msg = "Error:数据不正确", url = string.Empty });
            else if (command.UserAccount.IsNullOrWhitespace())
                return Json(new { code = -1, msg = "Error:数据不正确", url = string.Empty });
            else if (command.Password.IsNullOrWhitespace())
                return Json(new { code = -1, msg = "Error:数据不正确", url = string.Empty });
            else if (command.ConfirmPassword.IsNullOrWhitespace())
                return Json(new { code = -1, msg = "Error:数据不正确", url = string.Empty });
            else if (command.Password != command.ConfirmPassword)
                return Json(new { code = -1, msg = "Error:数据不正确", url = string.Empty });
            else if (this.CheckAccount(this._commandInvokerFactory))
                return Json(new { code = 1, msg = "Error:已存在后台管理账号", url = "/Account/Index" });


            var commandResult = this._commandInvokerFactory.Handle<UserRegisterCommand, UserLoginCommandResult>(command);


            if (!commandResult.IsSuccess)
                return Json(new { code = -1, msg = $"错误:{commandResult.GetErrors()[0]}", url = string.Empty });


            AccountLoginManager.SetLogin(HttpContext, commandResult);
            return Json(new { code = 1, msg = "Success:注册成功", url = "/Admin/Index" });
        }


        #endregion



        #region 验证码

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult VerifyCode()
        {
            VerifyCode v = new VerifyCode();
            // 是否随机字体颜色
            v.SetIsRandomColor = true;
            // 随机码的旋转角度
            v.SetRandomAngle = 4;
            // 文字大小
            v.SetFontSize = 18;
            // 设置字体
            v.SetFontFamily = this._webAppConfiguration.Value.settings.FontFamily;
            var questionItem = v.GetQuestion();
            v.SetVerifyCodeText = questionItem.Key;

            // 将验证码存储到Session
            HttpContext.Session.SetString("verifycode", questionItem.Value);
            System.IO.MemoryStream imgStream = v.OutputImageStreamp();
            Response.Body.Dispose();
            return File(imgStream.ToArray(), @"image/jpeg");
        }

        #endregion



        /// <summary>
        /// 检查是否已存在后台管理账号
        /// </summary>
        /// <returns>存在返回true，不存在返回false</returns>
        private bool CheckAccount(ICommandInvokerFactory commandInvokerFactory)
        {
            var commandResult = commandInvokerFactory.Handle<UserRegisterCheckCommand, CommandResult>(new UserRegisterCheckCommand());
            return commandResult.IsSuccess;
        }

    }
}
