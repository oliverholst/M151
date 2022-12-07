using Microsoft.AspNetCore.Mvc;


namespace Garage.Controllers
{
/// <summary>
/// Die View wird der Webseite witergeleitet
/// </summary>
    public class NeuwagenController : Controller
    {
        /// <summary>
        /// Wird zur View weitergeleitet
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View("View");
        }
    }
}
