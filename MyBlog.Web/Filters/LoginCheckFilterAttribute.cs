using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyBlog.Web.Common;

namespace MyBlog.Web.Filters
{
    /// <summary>
    /// 登陆检查过滤器
    /// </summary>
    public class LoginCheckFilterAttribute : ActionFilterAttribute
    {
        // 在Action执行之前检查是否登陆
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 判断是否已登录
            if (!AccountLoginManager.CheckLogin(context.HttpContext))
            {
                // 清除登陆痕迹
                AccountLoginManager.SetLoginOut(context.HttpContext);

                // 如果为Post请求
                if(context.HttpContext.Request.Method.Equals("post", StringComparison.CurrentCultureIgnoreCase))
                {
                    // 如果请求的控制器为AdminPostController
                    if (context.HttpContext.Request.Path.Value.StartsWith("/AdminPost"))
                        context.Result = new JsonResult(new { code = "-2", msg = "Error:登陆过期", url = "/Account/Index" });
                    // 如果请求的控制器为FileController
                    else if (context.HttpContext.Request.Path.Value.StartsWith("/File"))
                        context.Result = new ContentResult() { Content= "/Posts/UpLoadImgs/expiration.png" };
                }

                // 其他
                context.Result = new RedirectResult($"/Account/Index?ReturnUrl={context.HttpContext.Request.Path}");
            }
            base.OnActionExecuting(context);
        }
    }
}
