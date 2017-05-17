using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using MyBlog.Core;
using MyBlog.Core.Commands.Account;


namespace MyBlog.Web.Common
{
    /// <summary>
    /// 账号登陆检查
    /// </summary>
    public class AccountLoginManager
    {
        /// <summary>
        /// 检查是否已登录
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>已登录返回true，未登陆返回false</returns>
        public static bool CheckLogin(HttpContext httpContext)
        {
            // 
            if (null == httpContext.User.FindFirst(ClaimTypes.Sid) || null == httpContext.Session.GetString("login") || !httpContext.Session.GetString("login").Equals(httpContext.User.FindFirst(ClaimTypes.Sid).Value))
                return false;

            return true;
        }

        /// <summary>
        /// 设置登录标记
        /// </summary>
        /// <param name="commandResult"></param>
        public static void SetLogin(HttpContext httpContext, UserLoginCommandResult commandResult)
        {
            httpContext.Session.SetString("login", commandResult.UserInfo.user_account);
            // 创建两小时过期的cookie
            // 指定身份认证类型
            var identity = new ClaimsIdentity("Forms");
            // 用户名称
            var tempC = new Claim(ClaimTypes.Sid, commandResult.UserInfo.user_account);
            identity.AddClaim(tempC);
            var principal = new ClaimsPrincipal(identity);
            httpContext.Authentication.SignInAsync("_auth", principal, new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddHours(2) });
        }


        /// <summary>
        /// 清除登陆痕迹
        /// </summary>
        public static void SetLoginOut(HttpContext httpContext)
        {
            httpContext.Session.Remove("login");
            httpContext.Authentication.SignOutAsync("_auth");
        }

    }
}
