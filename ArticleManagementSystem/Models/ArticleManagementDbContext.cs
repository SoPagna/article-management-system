using System;
using Microsoft.EntityFrameworkCore;
using ArticleManagementSystem.Models;
using JetBrains.Annotations;

namespace ArticleManagementSystem.Data
{
    public class ArticleManagementDbContext : DbContext
    {
        public ArticleManagementDbContext (DbContextOptions<ArticleManagementDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}
