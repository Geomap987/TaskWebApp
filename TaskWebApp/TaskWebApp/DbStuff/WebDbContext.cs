using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Numerics;
using TaskWebApp.DbStuff.Models;

namespace TaskWebApp.DbStuff
{
    public class WebDbContext : DbContext
    {
        public WebDbContext(DbContextOptions<WebDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskInfo> TaskInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(user => user.OwnedTasks)
                .WithOne(taskInfo => taskInfo.Owner)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<User>()
                .HasMany(user => user.AssignedTasks)
                .WithMany(taskInfo => taskInfo.Assignees);
                  
        }
    }
}
