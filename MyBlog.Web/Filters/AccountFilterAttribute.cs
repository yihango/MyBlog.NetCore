using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyBlog.Core;
using MyBlog.Core.Commands.Account;
using MyBlog.Web.Common;
using MyExtensionsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyBlog.Web.Filters
{
    /// <summary>
    /// 登陆控制器过滤器
    /// </summary>
    public class AccountFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 如果已经登陆则跳转到后台管理页面
            if (AccountLoginManager.CheckLogin(context.HttpContext))
            {
                switch (context.HttpContext.Request.Method.ToLower())
                {
                    case "get":
                        context.Result = new RedirectResult("/Admin/Index");
                        break;
                    case "post":
                        context.Result = new JsonResult(new { code = -1, msg = "Error:已登录", url = "/Admin/Index" });
                        break;
                }
            }// 如果没有登陆则清除登陆痕迹
            else
            {
                AccountLoginManager.SetLoginOut(context.HttpContext);
            }
            base.OnActionExecuting(context);
        }
    }
}
