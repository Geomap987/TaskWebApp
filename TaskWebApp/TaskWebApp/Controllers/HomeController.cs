using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskWebApp.Controllers.CustomAuthAttributes;
using TaskWebApp.DbStuff.Repositories;
using TaskWebApp.Models;
using TaskWebApp.Services;

namespace TaskWebApp.Controllers
{
    public class HomeController : Controller
    {
        private AuthService _authService;
        private UserRepository _userRepository;

        public HomeController(AuthService authService, UserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AdminAccess]
        public IActionResult AdminSection()
        {

            return View();
        }

        [Authorize]
        public IActionResult SwitchLocale(string locale)
        {
            var userId = _authService.GetCurrentUserId().Value;
            _userRepository.SwitchLocal(userId, locale);
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
