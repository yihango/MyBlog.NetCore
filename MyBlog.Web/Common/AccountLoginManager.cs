using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using MyBlog.Commands.Account;
using Microsoft.AspNetCore.Authentication.Cookies;

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
            if (null == httpContext.User.FindFirst(ClaimTypes.Sid) ||
                null == httpContext.Session.GetString(AppConsts._session_server) ||
                !httpContext.Session.GetString(AppConsts._session_server)
                .Equals(httpContext.User.FindFirst(ClaimTypes.Sid).Value))
                return false;

            return true;
        }

        /// <summary>
        /// 设置登录标记
        /// </summary>
        /// <param name="commandResult"></param>
        public static void SetLogin(HttpContext httpContext, UserLoginCommandResult commandResult)
        {
            httpContext.Session.SetString(AppConsts._session_server, commandResult.UserInfo.Account);
            // 指定身份认证类型
            var identity = new ClaimsIdentity("Forms");
            // 用户名称
            var tempC = new Claim(ClaimTypes.Sid, commandResult.UserInfo.Account);
            identity.AddClaim(tempC);
            var principal = new ClaimsPrincipal(identity);
            // 登陆
            httpContext.SignInAsync(AppConsts._auth, principal, new AuthenticationProperties
            {
                IsPersistent = true
            });
        }


        /// <summary>
        /// 清除登陆痕迹
        /// </summary>
        public static void SetLoginOut(HttpContext httpContext)
        {
            httpContext.Session.Remove(AppConsts._session_server);
            httpContext.SignOutAsync(AppConsts._auth);
        }
    }
}
