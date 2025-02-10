using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Shared;
using TravelAgency.Domain.Features.Login;

namespace TravelAgencyMVC.Filters
{
    public class LoginCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            var user = token.ToDecrypt().ToObject<LoginTokenModel>();
            if (user == null || user.ExpireTime < DateTime.Now)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            // Pass user information to the view
            context.HttpContext.Items["UserName"] = user.Name;
            context.HttpContext.Items["Role"] = user.Role;

            base.OnActionExecuting(context);
        }
    }
}
