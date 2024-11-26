using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SAASLMS.Data;
using Microsoft.EntityFrameworkCore;
using SAASLMS.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<TenantService>();

builder.Services.AddScoped<SAASLMS.Services.AuthenticationService>();


// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; // Redirects to login page if not authenticated
        options.AccessDeniedPath = "/access-denied"; // Redirects for unauthorized access
    });

//builder.Services.AddAuthorizationCore();
builder.Services.AddHttpContextAccessor(); // Required for accessing HttpContext
builder.Services.AddScoped<SAASLMS.Services.AuthenticationService>(); // Custom Authentication Service

var app = builder.Build();

// Migrate Database on Startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); // Applies migrations automatically at startup
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
