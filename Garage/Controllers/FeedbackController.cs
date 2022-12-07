using Garage.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Garage.Controllers
{
    /// <summary>
    /// Datenbankverbindung aufbauen und die Liste der Webseite übergeben.
    /// </summary>
    public class FeedbackController : Controller
    {
        /// <summary>
        /// Datenbankverbindung
        /// </summary>
        public ActionResult Index()
        {
            var mySQL = new MSSqlFeedback();
            mySQL.Connect();
            return View(mySQL.Select());
        }

        /// <summary>
        /// Das erzeugte Feedback wird zu View weitergeleitet
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            // In der Klasse Auto hatten Sie die ListBox für die Treibstoffe
            // erstellt. Nun folgt eine weitere Variante, wie man eine ListBox
            // mit Daten befuellen kann
            ViewBag.FahrzeugArtListe = Feedback.FahrzeugArtListe;
            return View(new Feedback());
        }

        /// <summary>
        /// Erzeugt: Feedback
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Feedback feedback)
        {
            try
            {
                var mySQL = new MSSqlFeedback();
                mySQL.Connect();
                mySQL.Insert(feedback);

                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                return View("Feedback nicht erzeugt: " + ex.Message);
            }
        }
    }
}