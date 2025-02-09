using Microsoft.AspNetCore.Mvc;
using TravelAgency.Shared;
using TravelAgency.Domain.Features.Login;

namespace TravelAgencyMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", requestModel);
            }

            var response = await _loginService.Execute(requestModel);

            if (!response.Success)
            {
                ViewBag.Error = response.Message;
                return View("Index", requestModel);
            }

            LoginTokenModel model = response.Token.ToDecrypt().ToObject<LoginTokenModel>();

            // Set the token as a cookie
            Response.Cookies.Append("AuthToken", response.Token, new CookieOptions
            {
                Expires = model.ExpireTime,
                HttpOnly = true
            });

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");

            return RedirectToAction("Index");
        }
    }
}
