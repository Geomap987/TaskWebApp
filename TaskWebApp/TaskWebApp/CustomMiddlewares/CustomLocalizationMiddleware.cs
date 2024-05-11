using System.Globalization;
using TaskWebApp.Services;

namespace TaskWebApp.CustomMiddlewares
{
    public class CustomLocalizationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomLocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            CultureInfo culture;

            var authService = context.RequestServices.GetService<AuthService>();
            var localFromClaims = context.User.Claims.FirstOrDefault(x => x.Type == AuthService.LOCALE_TYPE);
            //if (authService.IsAuthenticated())
            //{
            //    culture = new CultureInfo(authService.GetCurrentUserLocale());
            //}
            if (context.Request.Cookies["langues"] != null)
            {
                var localFromCookie = context.Request.Cookies["langues"];
                culture = new CultureInfo(localFromCookie);
            }
            else
            {
                string acceptLanguage = context.Request.Headers.AcceptLanguage;
                var locale = acceptLanguage.Substring(0, 2);

                culture = new CultureInfo(locale);
            }

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            await _next.Invoke(context);
        }
    }
}
