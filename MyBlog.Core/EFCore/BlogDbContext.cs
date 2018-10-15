using Microsoft.EntityFrameworkCore;
using MyBlog.Posts;
using MyBlog.PostTags;
using MyBlog.Tags;
using MyBlog.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.EFCore
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
