using Microsoft.EntityFrameworkCore;
using MyBlog.Core.Posts;
using MyBlog.Core.PostTags;
using MyBlog.Core.Tags;
using MyBlog.Core.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Core
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostTag> PostTags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
