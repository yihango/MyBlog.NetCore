using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Core
{
    public class BloggingContextFactory : IDesignTimeDbContextFactory<BlogDbContext>
    {
        private readonly IOptions<AppConfig> _appConfig;
        public BloggingContextFactory(
            IOptions<AppConfig> appConfig
            )
        {
            _appConfig = appConfig;
        }

        public BlogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
            optionsBuilder.UseSqlite(_appConfig.Value.DbConnStr);

            return new BlogDbContext(optionsBuilder.Options);
        }
    }
}
