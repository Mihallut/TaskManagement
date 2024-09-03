using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure
{
    public class ApiDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Domain.Entities.Task> Tasks => Set<Domain.Entities.Task>();

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Id)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Domain.Entities.Task>()
                .HasIndex(t => t.Id)
                .IsUnique();

            modelBuilder.Entity<Domain.Entities.Task>()
                .HasOne(t => t.CreatedBy)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
