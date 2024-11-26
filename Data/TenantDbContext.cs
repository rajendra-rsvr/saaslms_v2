using Microsoft.EntityFrameworkCore;
using SAASLMS.SharedSchema;

namespace SAASLMS.Data
{
    public class TenantDbContext : DbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "SuperAdmin", RoleDescription = "Has full access to all resources." },
                new Role { Id = 2, RoleName = "Admin", RoleDescription = "Can manage users and roles." },
                new Role { Id = 3, RoleName = "User", RoleDescription = "Has limited access to resources." }
            );
        }
    }
}
