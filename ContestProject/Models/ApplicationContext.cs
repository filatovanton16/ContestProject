using Microsoft.EntityFrameworkCore;

namespace ContestProject.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Many-to-many connection between users and tasks
            modelBuilder.Entity<UserTask>()
                .HasKey(t => new { t.UserId, t.TaskId });

            modelBuilder.Entity<UserTask>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTasks)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<UserTask>()
                .HasOne(ut => ut.Task)
                .WithMany(t => t.UserTasks)
                .HasForeignKey(ut => ut.TaskId);

            //Indexation
            modelBuilder.Entity<User>().HasIndex(u => u.NumberOfSolved);
        }

        public DbSet<ContestTask> ContestTasks { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}