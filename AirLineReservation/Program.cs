using Microsoft.EntityFrameworkCore;
using AirlineReservation.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System; // For TimeSpan

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database Configuration for SQL Server LocalDB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Authentication Configuration
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Redirect to this path if not authenticated
        options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect if unauthorized
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie expiration
        options.SlidingExpiration = true; // Reset expiration on activity
    });
builder.Services.AddAuthorization(); // Required for authorization

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication middleware must be placed after UseRouting and before UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();