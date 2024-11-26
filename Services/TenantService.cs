using SAASLMS.Model;
using Microsoft.EntityFrameworkCore;
using SAASLMS.Data;
using SAASLMS.SharedSchema;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace SAASLMS.Services
{
    public class TenantService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public TenantService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Create
        public async Task<Tenant> CreateTenantAsync(Tenant tenant)
        {
            try
            {
                string defaultSiteURL = _configuration.GetConnectionString("DefaultURL");
                tenant.SiteUrl = tenant.Domain + defaultSiteURL;
                tenant.SiteUrl = tenant.SiteUrl.ToLower();
                tenant.CreateDate = DateTime.UtcNow; // Set creation date
                _context.Tenants.Add(tenant);
                await _context.SaveChangesAsync();

                // Retrieve the base connection string from appsettings.json
                string baseConnectionString = _configuration.GetConnectionString("DefaultConnection");
                string databaseName = GetDatabaseName(baseConnectionString);
                // Replace the placeholder with the actual database name
                string connectionString = baseConnectionString.Replace($"{databaseName}", $"{tenant.TenantName}_rsvrtech");

                // Create a new DbContext for the tenant
                var tenantDbContext = new TenantDbContext(new DbContextOptionsBuilder<TenantDbContext>()
                    .UseSqlServer(connectionString)
                    .Options);

                // Create the shared tables
                await tenantDbContext.Database.EnsureCreatedAsync();

                // Fetch the RoleId for the "SuperAdmin" role
                var superAdminRole = await tenantDbContext.Roles
                    .FirstOrDefaultAsync(r => r.RoleName == "SuperAdmin");

                if (superAdminRole == null)
                {
                    throw new Exception("SuperAdmin role not found in the Roles table.");
                }

                // Create a new User with SuperAdmin role
                User user = new User
                {
                    Username = $"{tenant.FirstName} {tenant.LastName}",
                    Email = tenant.Email,
                    RoleId = superAdminRole.Id, // Set RoleId to SuperAdmin's Id
                    CreateDate = DateTime.UtcNow
                };

                tenantDbContext.Users.Add(user);
                await tenantDbContext.SaveChangesAsync();
                return tenant;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SubscriptionPlan>> GetSubscriptionPlansAsync()
        {
            return await _context.SubscriptionPlans.ToListAsync();
        }

        public async Task<List<Status>> GetStatusAsync()
        {
            return await _context.Statuses.ToListAsync();
        }


        // Read
        public async Task<List<Tenant>> GetAllTenantsAsync()
        {
            return await _context.Tenants.ToListAsync();
        }

        public async Task<Tenant> GetTenantByIdAsync(int id)
        {
            return await _context.Tenants.FindAsync(id);
        }

        // Update
        public async Task<bool> UpdateTenantAsync(Tenant tenant)
        {
            _context.Tenants.Update(tenant);
            return (await _context.SaveChangesAsync()) > 0;
        }

        // Delete
        public async Task<bool> DeleteTenantAsync(int id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null)
            {
                return false;
            }

            _context.Tenants.Remove(tenant);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public string GetDatabaseName(string baseConnectionString)
        {
            // Initialize SqlConnectionStringBuilder with the connection string
            var connectionStringBuilder = new SqlConnectionStringBuilder(baseConnectionString);

            // Retrieve the database name
            return connectionStringBuilder.InitialCatalog;
        }
    }
}
