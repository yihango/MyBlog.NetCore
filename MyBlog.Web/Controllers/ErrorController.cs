using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.Web.Controllers
{
    public class ErrorController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public ErrorController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        // GET: /<controller>/
        [Route("/Error/{statusCode}")]
        [HttpGet]
        public IActionResult Index(string statusCode)
        {
            var filePath = $"{_hostingEnvironment.WebRootPath}/Errors/{(statusCode == "404" ? 404 : 500)}.html";
            return new PhysicalFileResult(filePath, new MediaTypeHeaderValue("text/html"));
        }
    }
}
