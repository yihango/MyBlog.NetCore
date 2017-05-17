using Microsoft.Extensions.Options;
using MyBlog.Core;
using MyBlog.Models;

namespace MyBlog.Web.Features
{
    public class MyDbSession : DbSession
    {
        public MyDbSession(IOptions<WebAppConfiguration> webAppConfiguration)
            : base(webAppConfiguration.Value.connectionStrings.connStr)
        {
        }
    }
}
