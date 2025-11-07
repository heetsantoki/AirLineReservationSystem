using Microsoft.AspNetCore.Mvc;
using AirlineReservation.ViewModels; // Adjusted to ViewModels folder
using AirlineReservation.Data;
using AirlineReservation.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Collections.Generic; // For List<Claim>

namespace AirlineReservation.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                                         .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user != null)
                {
                    // !!! IMPORTANT: Replace this with secure password hashing verification (e.g., BCrypt.Net.BCrypt.Verify)
                    if (user.PasswordHash == model.Password) // TEMPORARY: DO NOT USE IN PRODUCTION
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                            new Claim(ClaimTypes.Email, user.Email)
                            // Add other claims like role if you have them in your User model
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe,
                            ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : (DateTimeOffset?)null
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already taken.");
                    return View(model);
                }
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError(nameof(model.Email), "Email is already registered.");
                    return View(model);
                }

                // !!! IMPORTANT: Hash the password before saving it to the database!
                // For a real application, use a library like BCrypt.Net:
                // var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                var newUser = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = model.Password, // TEMPORARY: DO NOT USE IN PRODUCTION, STORE HASHED PASSWORD
                    RegistrationDate = DateTime.UtcNow
                };

                _context.Add(newUser);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}