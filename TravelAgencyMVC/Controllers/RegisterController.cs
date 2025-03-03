using Microsoft.AspNetCore.Mvc;
using TravelAgency.Domain.Features.UserRegister;

namespace TravelAgencyMVC.Controllers;

public class RegisterController : Controller
{
    private readonly UserRegisterService _registerService;

    public RegisterController(UserRegisterService registerService)
    {
        _registerService = registerService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View("Register");
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterRequestModel requestModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "All fields are required!";
            return View("Register", requestModel);
        }

        var result = await _registerService.Execute(requestModel);

        if (result.IsSuccess)
        {
            return RedirectToAction("VerifyEmail", new { email = requestModel.Email });
        }

        ViewBag.Error = "Registration failed. Please try again.";
        return View("Register", requestModel);
    }

    [HttpGet]
    public IActionResult VerifyEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return RedirectToAction("Index");
        }

        ViewBag.Email = email;
        return View("VerifyEmail");
    }

    [HttpPost]
    public async Task<IActionResult> VerifyEmail(string email, string otp)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp))
        {
            ViewBag.Error = "Email and OTP are required!";
            return View("VerifyEmail");
        }

        var result = await _registerService.VerifyEmail(email, otp);
        if (result.IsSuccess)
        {
            TempData["Success"] = "OTP verified successfully! You can now log in.";
            return RedirectToAction("Index", "Login");
        }


        TempData["Error"] = "Email verification failed. Incorrect OTP or expired code.";
        return View("VerifyEmail");
    }

}
