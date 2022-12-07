using Garage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Garage.Controllers
{
    /// <summary>
    /// Ist die Einstiegseite für das Web (Main)
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Liefert die Standard-Webseite der Garage
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Zeigt Kontakt.cshtml an
        /// </summary>
        /// <returns></returns>
        public IActionResult Kontakt()
        {
            return View();
        }
    }
}