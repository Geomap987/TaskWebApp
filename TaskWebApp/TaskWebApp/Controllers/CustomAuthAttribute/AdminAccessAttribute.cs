using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskWebApp.Services;
using TaskWebApp.Controllers;

namespace TaskWebApp.Controllers.CustomAuthAttributes
{
    public class AdminAccessAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authService = context.HttpContext.RequestServices.GetService<AuthService>();
            if (!authService.IsAdmin())
            {
                context.Result = new ForbidResult(AuthController.AUTH_KEY);
            }
        }
    }
}