using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using MyBlog.Web.ViewModels.Error;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Diagnostics;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.Web.Controllers
{
    public class ErrorController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<AppConfig> _appConfig;

        public ErrorController(
            IHostingEnvironment hostingEnvironment,
            IOptions<AppConfig> appConfig
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _appConfig = appConfig;
        }


        // GET: /<controller>/
        [HttpGet("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.Configs = this._appConfig.Value;
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;
            // 将异常添加到异常队列中
            //Global.ExceptionQueue.Enqueue(error);
            return View("~/Views/Shared/Error.cshtml", new ErrorViewModel()
            {
                Exception = error,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            });
        }
    }
}
