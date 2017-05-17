using System;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MyBlog.Models;
using MyExtensionsLib;
using MyBlog.Web.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.Web.Controllers
{
    [LoginCheckFilter]
    public class FileController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<WebAppConfiguration> _webAppConfiguration;
        public FileController(IHostingEnvironment hostingEnvironment, IOptions<WebAppConfiguration> webAppConfiguration)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._webAppConfiguration = webAppConfiguration;
        }

        /// <summary>
        /// 上传图片文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpLoadImg(string path)
        {
            var files = HttpContext.Request.Form.Files;

            if (null == files || files.Count <= 0)
                return null;

            // 判断扩展名是否正确
            var extensionName = Path.GetExtension(files[0].FileName).ToLower();
            if (!this._webAppConfiguration.Value.settings.ImgExtensions.Any(n => n == extensionName))
                return null;

            // 保存的文件夹
            var sortTime = DateTime.Now.ToString("yyyy_MM_dd");
            var tempPath = this._webAppConfiguration.Value.settings.UpLoadImgRelativeSavePath.Replace("{time}", sortTime);
            var dirPath = $"{this._hostingEnvironment.WebRootPath}\\{tempPath}";
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            // 保存的文件全路径 
            var filePath = string.Empty;
            while (true)
            {
                filePath = Path.Combine(dirPath, $"{DateTime.Now.ToStamp()}_{files[0].FileName}");
                if (!System.IO.File.Exists(filePath))
                    break;
            }

            // 存储图片到文件
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                files[0].CopyTo(fs);
                fs.Flush();
            }
            // 返回路径
            var imgPath = Path.Combine("/Contents", tempPath, Path.GetFileName(filePath)).Replace("\\", "/");


            return Content(imgPath);
        }

        /// <summary>
        /// 常用图片扩展名数组
        /// </summary>
        private readonly string[] _extensionNames = { ".bmp", ".gif", ".png", ".jpg", ".jpeg" };
    }
}
