using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ArticleManagementSystem.Models;
using ArticleManagementSystem.Repositories;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace ArticleManagementSystem.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly PasswordHasher<string> passwordHasher;
        private readonly IUserRepository userRepository;

        public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
            this.passwordHasher = new PasswordHasher<string>();
        }

        public int GetAuthUserID()
        {
            return Int32.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public bool VerifyEmail(string email)
        {
            return this.userRepository.GetUser(email) != null;
        }

        public User Register(RegisterViewModel registerViewModel)
        {
            User user = new User()
            {
                Name = registerViewModel.Name,
                Email = registerViewModel.Email,
                Password = this.passwordHasher.HashPassword(null, registerViewModel.Password)
            };
            this.userRepository.Create(user);

            return user;
        }

        public async Task<bool> Login(string email, string password, bool rememberMe)
        {
            User authUser = this.userRepository.GetUser(email);

            if (authUser != null)
            {
                if (passwordHasher.VerifyHashedPassword(null, authUser.Password, password) == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, authUser.ID.ToString()),
                            new Claim(ClaimTypes.GivenName, authUser.Name),
                            new Claim(ClaimTypes.Email, authUser.Email)
                        };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(claimsIdentity);
                    var authProperties = new AuthenticationProperties { IsPersistent = rememberMe };
                    await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                    return true;
                }
            }

            return false;
        }
    }
}
