﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskWebApp.DbStuff.Models;
using TaskWebApp.Models.Auth;
using TaskWebApp.DbStuff.Repositories;

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

        //public async Task LoginWithGoogle()
        //{

        //    await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
        //    {
        //        RedirectUri = Url.Action("GoogleResponse")
        //    });
        //}

        //public async Task<IActionResult> GoogleResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //    var claims = result.Principal.Identities.First().Claims.Select(claim => new Claim(claim.Type, claim.Value));

        //    var userEmail = GetInfoFromClaims(claims, CLAIMS_EMAIL_GOOGLE);

        //    var user = _userRepository.GetUserByEmail(userEmail);

        //    if (user == null)
        //    {
        //        user = new User
        //        {
        //            Email = userEmail,
        //            FirstName = GetInfoFromClaims(claims, CLAIMS_FIRSTNAME_GOOGLE),
        //            LastName = GetInfoFromClaims(claims, CLAIMS_LASTNAME_GOOGLE),
        //            AvatarUrl = GetInfoFromClaims(claims, CLAIMS_IMG_GOOGLE),
        //        };
        //        user.Id = _userRepository.Add(user);
        //    }

        //    SignInUser(user);

        //    return RedirectToAction("Index", "Home");
        //}

        [HttpGet]
        public IActionResult Login()
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
                //new Claim(AuthService.LOCALE_TYPE, user.PreferLocale),
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
