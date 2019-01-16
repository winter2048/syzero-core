using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SyZero.Domain.Model;
namespace SyZero.Infrastructure.EntityFramework
{
    public class SyDbContext:DbContext
    {
        public SyDbContext()
        {
        }

        public SyDbContext(DbContextOptions<SyDbContext> options) : base(options)
        {

        }
        public DbSet<Article> Article { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Messaged> Messaged { get; set; }
        public DbSet<Tool> Tool { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Categorys> Categorys { get; set; }
        public DbSet<Configure> Configure { get; set; }
        public DbSet<Seo> Seo { get; set; }
        public DbSet<TimeAxis> TimeAxis { get; set; }
        public DbSet<UserType> UserType { get; set; }
        public DbSet<FriendLink> FriendLin { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           // modelBuilder.Entity<ArticleEntity>().HasIndex(u => u.Id).IsUnique();
        }
    }
}
