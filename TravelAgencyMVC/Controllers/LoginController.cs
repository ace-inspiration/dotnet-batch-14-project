using Microsoft.AspNetCore.Mvc;
using TravelAgency.Shared;
using TravelAgency.Domain.Features.Login;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using TravelAgencyMVC.Filters;

namespace TravelAgencyMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return View(requestModel);
            }

            var response = await _loginService.Execute(requestModel);

            if (!response.Success)
            {
                ViewBag.Error = response.Message;
                return View(requestModel);
            }

            LoginTokenModel model = response.Token.ToDecrypt().ToObject<LoginTokenModel>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Name),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, model.Role) 
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Create the authentication cookie
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = model.ExpireTime,
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(claimsIdentity),
                   authProperties);

            // Set the token as a cookie
            Response.Cookies.Append("AuthToken", response.Token, new CookieOptions
            {
                Expires = model.ExpireTime,
                HttpOnly = true
            });

            if (model.Role == "admin")
            {
                return RedirectToAction("AdminDashboard", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
