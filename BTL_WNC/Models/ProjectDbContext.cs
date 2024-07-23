using Microsoft.EntityFrameworkCore;
namespace BTL_WNC.Models
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Project> Projects { get; set; }

        //public DbSet<UserTask> UserTasks { get; set; } // Bảng kết nối

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<UserTask>()
        //        .HasKey(ut => new { ut.UserId, ut.TaskId });

        //    modelBuilder.Entity<UserTask>()
        //        .HasOne(ut => ut.User)
        //        .WithMany(u => u.UserTasks)
        //        .HasForeignKey(ut => ut.UserId);

        //    modelBuilder.Entity<UserTask>()
        //        .HasOne(ut => ut.Task)
        //        .WithMany(t => t.UserTasks)
        //        .HasForeignKey(ut => ut.TaskId);

        //    modelBuilder.Entity<Task>()
        //        .HasOne(t => t.Project)
        //        .WithMany(p => p.Tasks)
        //        .HasForeignKey(t => t.ProjectId);
        //}

    }
}
