using ArticleManagement.Web.Models.Auth;
using ArticleManagement.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagement.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.HideMenu = true;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)    
                return View(model);

            var result = await _authService.LoginAsync(model.Username, model.Password);

            if (result is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            // Guardar token JWT en cookie
            Response.Cookies.Append("jwt_token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = result.Expiration
            });
            // Guardar token o info de usuario en la sesión si lo necesitas
            //HttpContext.Session.SetString("Token", result.Value.Token);
            //HttpContext.Session.SetString("Username", result.Value.Username);

            // ✅ Redirigir a la vista de artículos
            return RedirectToAction("Index", "Articles");

        }

        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.RegisterAsync(model.NewUsername, model.Email, model.NewPassword);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Registration failed.");
                TempData["SuccessMessage"] = "Registration failed. Please try again.";
            }

            TempData["SuccessMessage"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_token");
            return RedirectToAction("Login");
        }
    }
}