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
        if (string.IsNullOrWhiteSpace(requestModel.Name) || string.IsNullOrWhiteSpace(requestModel.Email) || string.IsNullOrWhiteSpace(requestModel.Password))
        {
            ViewBag.Error = "All fields are required!";
            return View("Register", requestModel);
        }

       
        var result = await _registerService.Execute(requestModel);

        if (result.IsSuccess)
        {
            return RedirectToAction("Index", "Login");
        }
        else
        {
            ViewBag.Error = "Registration failed. Please try again.";
            return View("Register", requestModel);
        }
    }
}
