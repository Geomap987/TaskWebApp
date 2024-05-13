using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskWebApp.DbStuff.Models;
using TaskWebApp.Models.Auth;
using TaskWebApp.DbStuff.Repositories;
using TaskWebApp.Services;

namespace TaskWebApp.Controllers
{
    public class AuthController : Controller
    {
        private UserRepository _userRepository;

        public const string AUTH_KEY = "MashaAuth";

        public AuthController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Deny()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AuthViewModel authViewModel)
        {
            var user = _userRepository.GetUserByLoginAndPassword(authViewModel.UserName, authViewModel.Password);
            if (user == null)
            {
                ModelState.AddModelError(nameof(AuthViewModel.UserName), "Wrong name or passwrod");
                return View(authViewModel);
            }

            SignInUser(user);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }

        private string GetInfoFromClaims(IEnumerable<Claim> claims, string claimType)
        {
            return claims.First(x => x.Type == claimType).Value;
        }

        private void SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("name", user.Login ?? "user"),
                new Claim("email", user.Email ?? ""),
                new Claim(AuthService.LOCALE_TYPE, user.PreferLocale),
                new Claim("role", user.Role ?? "")
            };

            var identity = new ClaimsIdentity(claims, AUTH_KEY);
            var principal = new ClaimsPrincipal(identity);
            HttpContext
                .SignInAsync(AUTH_KEY, principal)
                .Wait();

            //return RedirectToAction("Index", "Home");
        }
    }
}
