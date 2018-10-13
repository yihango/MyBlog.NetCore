using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Core
{
    public class BloggingContextFactory : IDesignTimeDbContextFactory<BlogDbContext>
    {
        public BlogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
            optionsBuilder.UseSqlite("Data Source=blog.db");

            return new BlogDbContext(optionsBuilder.Options);
        }
    }
}
