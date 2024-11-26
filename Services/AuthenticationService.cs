using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SAASLMS.Data;
using SAASLMS.SharedSchema;
using System.Security.Claims;

namespace SAASLMS.Services
{
    public class AuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public AuthenticationService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Checks if the current user is authenticated.
        /// </summary>
        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        /// <summary>
        /// Authenticates the user by checking the provided email and password against the database.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>Authenticated user if found, otherwise null.</returns>
        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            // NOTE: Storing passwords in plain text is NOT recommended. Use hashing in production.
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                await LoginAsync(user.Email, user.RoleId.ToString());
            }

            return user;
        }

        public async Task LoginAsync(string username, string role)
        {
            try
            {
                // Check if the response has already started
                if (_httpContextAccessor.HttpContext!.Response.HasStarted)
                {
                    Console.WriteLine("Response has already started. Skipping cookie setting.");
                    return;
                }
                Console.WriteLine("Setting authentication cookies...");
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };

                // Set cookies for authentication
                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                Console.WriteLine("Authentication successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Catch"+ ex.Message.ToString());
                throw;
            }
           
        }



        /// <summary>
        /// Logs out the current user by clearing the authentication cookie.
        /// </summary>
        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Retrieves the name of the authenticated user.
        /// </summary>
        /// <returns>The name of the user if authenticated, otherwise null.</returns>
        public string? GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }

        /// <summary>
        /// Retrieves the role of the authenticated user.
        /// </summary>
        /// <returns>The role of the user if authenticated, otherwise null.</returns>
        public string? GetUserRole()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}
