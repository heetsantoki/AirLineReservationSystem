using System;
using AirLineReservation.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Antiforgery;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// DbContext (SQL Server / LocalDB)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpContextAccessor();


// Antiforgery cookie -> secure + same-site
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = ".AirLineReservation.AntiForgery";
    options.Cookie.SameSite = SameSiteMode.Lax;     // good default for normal forms
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // send only on HTTPS
});

// Auth cookies
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".AirLineReservation.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;          // prevents cross-site posting issues
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // only over HTTPS
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;

        // If you want to keep the original returnUrl behavior:
        options.ReturnUrlParameter = "returnUrl";
    });

var app = builder.Build();

// Error handling + HSTS in production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // adds Strict-Transport-Security
}

// Always redirect HTTP -> HTTPS (fixes the console warning)
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// If hosted behind a proxy/HTTPS terminator, uncomment:
// app.UseForwardedHeaders(new ForwardedHeadersOptions
// {
//     ForwardedHeaders = ForwardedHeaders.XForwardedProto
// });

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
