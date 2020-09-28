using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ProfileUser> ProfileUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProfileUser>()
            .HasOne(p => p.User)
            .WithMany(b => b.ProfileUser);
    }
    }
}