using Microsoft.EntityFrameworkCore;
using SAASLMS.Model;
using SAASLMS.SharedSchema;



namespace SAASLMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "SuperAdmin", RoleDescription = "Has full access to all resources." },
                new Role { Id = 2, RoleName = "Admin", RoleDescription = "Can manage users and roles." },
                new Role { Id = 3, RoleName = "User", RoleDescription = "Has limited access to resources." }
            );

            // Seed initial subscription plans with descriptions
            modelBuilder.Entity<SubscriptionPlan>().HasData(
                new SubscriptionPlan
                {
                    Id = 1,
                    Plan = "Silver",
                    Description = "Basic subscription with essential features, including access to standard content and limited support.",
                    MaximumUsersUpperLimit = 100
                },
                new SubscriptionPlan
                {
                    Id = 2,
                    Plan = "Gold",
                    Description = "Enhanced subscription with advanced features, priority support, and additional access to premium content.",
                    MaximumUsersUpperLimit = 1000
                },
                new SubscriptionPlan
                {
                    Id = 3,
                    Plan = "Platinum",
                    Description = "Premium subscription with all features, dedicated support, and unrestricted access to all content and resources.",
                    MaximumUsersUpperLimit = 10000
                }
            );

           modelBuilder.Entity<Status>().HasData(
           new Status { Id = 1, StatusName = "Active", Description = "Active status" },
           new Status { Id = 2, StatusName = "Inactive", Description = "Inactive status" },
           new Status { Id = 3, StatusName = "Suspendate", Description = "Suspendate status" },
           new Status { Id = 4, StatusName = "Sleep", Description = "Sleep status" }
       );
        }
    }
}
