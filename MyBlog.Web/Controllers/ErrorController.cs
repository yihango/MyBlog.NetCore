using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using MyBlog.Web.ViewModels.Error;
using System.Diagnostics;
using Microsoft.Extensions.Options;

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
        [Route("/Error/{statusCode}")]
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string statusCode)
        {
            ViewBag.Configs = this._appConfig.Value;
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Code = statusCode
            });
        }
    }
}
