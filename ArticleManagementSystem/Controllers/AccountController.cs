using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ArticleManagementSystem.Models;
using ArticleManagementSystem.Data;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace ArticleManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private ArticleManagementDbContext _context;
        private PasswordHasher<string> passwordHasher;

        public AccountController(ArticleManagementDbContext context)
        {
            this._context = context;
            this.passwordHasher = new PasswordHasher<string>();
        }

        [HttpGet]
        public ViewResult Register() => View();

        [HttpPost]
        public IActionResult Register([Bind("Name", "Email", "Password", "ConfirmPassword")] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Name = registerViewModel.Name,
                    Email = registerViewModel.Email,
                    Password = passwordHasher.HashPassword(null, registerViewModel.Password)
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction(nameof(Login));
            }

            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginData)
        {
            if (ModelState.IsValid)
            {
                User authUser = _context.Users.Where(user => user.Email.Equals(loginData.Email)).First();

                if (authUser != null)
                {
                    if (passwordHasher.VerifyHashedPassword(null, authUser.Password, loginData.Password) == PasswordVerificationResult.Success)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, authUser.ID.ToString()),
                            new Claim(ClaimTypes.GivenName, authUser.Name),
                            new Claim(ClaimTypes.Email, authUser.Email)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(claimsIdentity);
                        var authProperties = new AuthenticationProperties { IsPersistent = loginData.RememberMe };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                        return RedirectToAction("Index", "Article");
                    }
                }

                ModelState.AddModelError("AuthErrorMessage", "Email or Password is incorrect!");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return RedirectToAction(nameof(Login));
        }
    }
}
