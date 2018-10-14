using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyBlog
{
    public class BloggingContextFactory : IDesignTimeDbContextFactory<BlogDbContext>
    {
        public BlogDbContext CreateDbContext(string[] args)
        {
            var connStr = GetConnStr();

            var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();

            optionsBuilder.UseSqlite(connStr);

            return new BlogDbContext(optionsBuilder.Options);
        }

        private string GetConnStr()
        {
            var path = typeof(BloggingContextFactory).Assembly.Location;
            var filePath = Path.Combine(Path.GetDirectoryName(path), "appsettings.json");

            var json = File.ReadAllText(filePath, Encoding.UTF8);
            var config = JsonConvert.DeserializeObject<dynamic>(json);
            return config.appConfig.connStr;
        }
    }
}
