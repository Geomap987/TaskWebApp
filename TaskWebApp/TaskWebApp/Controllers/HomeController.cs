using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskWebApp.Controllers.CustomAuthAttributes;
using TaskWebApp.Models;

namespace TaskWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        [AdminAccess]
        public IActionResult AdminSection()
        {

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
