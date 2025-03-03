using Microsoft.AspNetCore.Mvc;
using TravelAgency.Shared;
using TravelAgency.Domain.Features.Login;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using TravelAgencyMVC.Filters;
using TravelAgency.Domain.Features.UserLists;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgencyMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        private readonly UserListService _userListService;

        public LoginController(LoginService loginService, UserListService userListService)
        {
            _loginService = loginService;
            _userListService = userListService;
        }

        [HttpGet]
        public IActionResult Index()
        {

            if (User.Identity!.IsAuthenticated)
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
                new Claim("UserId", model.UserId),
                new Claim("Name", model.Name),
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.Role, model.Role),
                new Claim("Phone", model.PhoneNumber)
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



            if (model.Role == "Admin")
            {
                return RedirectToAction("AdminDashboard", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ActionName("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(User requestModel, string id)
        {
            // Ensure the current user ID matches the ID being updated
            var currentUserId = User.FindFirst("UserId")?.Value;

            if (currentUserId != null && id == currentUserId)
            {
                // Update the user profile with the new data
                var user = await _userListService.UpdateUser(requestModel);

                if (user != null)
                {
                    // Re-create the claims based on the updated user
                    var claims = new List<Claim>
            {
                new Claim("UserId", user.Id),
                new Claim("Name", user.Name),
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.Role, user.Role), // Ensure the role is correct
                new Claim("Phone", user.Phone)
            };

                    // Create new claims identity and sign the user in again
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity)
                    );

                    // Check the user's role and redirect to the appropriate page
                    var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                    if (userRole == "admin")
                    {
                        return RedirectToAction("AdminDashboard", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            // Redirect to home if the user doesn't have permission to update
            return RedirectToAction("Index", "Home");
        }



    }
}
