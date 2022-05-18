using Microsoft.AspNetCore.Mvc;
using Parental_Control.Models;
using System.Diagnostics;

namespace Parental_Control.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            if (TempData["MSG"] != null)
            {
                ViewBag.StatusMsg = TempData["MSG"].ToString();
                ViewBag.msgType = TempData["MSG_Type"].ToString();
            }
            return View();
        }

        public IActionResult Privacy()
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