using Microsoft.EntityFrameworkCore;
using JobTrackerApi.Models;

namespace JobTrackerApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Application>()
                .Property(a => a.JobType)
                .HasConversion<string>();

            modelBuilder.Entity<Application>()
                .Property(a => a.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Interview>()
                .Property(i => i.Outcome)
                .HasConversion<string>();
        }
    }
}
