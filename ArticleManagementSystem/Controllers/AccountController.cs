using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ArticleManagementSystem.Models;
using ArticleManagementSystem.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ArticleManagementSystem.Services;

namespace ArticleManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private ArticleManagementDbContext _context;
        private PasswordHasher<string> passwordHasher;
        private readonly UserService userService;

        public AccountController(ArticleManagementDbContext context, UserService userService)
        {
            this._context = context;
            this.passwordHasher = new PasswordHasher<string>();
            this.userService = userService;
        }

        [HttpGet]
        public ViewResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid && this.userService.Register(registerViewModel) != null)
            {
                if (await this.userService.Login(registerViewModel.Email, registerViewModel.Password, false))
                {
                    return RedirectToAction("Index", "Article");
                }

                return RedirectToAction(nameof(Login));
            }

            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await this.userService.Login(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe))
                {
                    return RedirectToAction("Index", "Article");
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
