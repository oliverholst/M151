using Garage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Garage.Controllers
{
    /// <summary>
    /// Datenbankverbindung aufbauen und die Liste der Webseite übergeben.
    /// </summary>
    public class AutoController : Controller
    {
        private const string SORT_ORDER = "SORT_ORDER";
        private const string TREIBSTOFFFILTER = "TREIBSTOFFFILTER";
        private const string PAGE_START = "PAGE_START";
        public const int PAGE_SIZE = 10;

        /// <summary>
        /// Datenbankverbindung
        /// </summary>
        private IAuto db = new MSSqlAuto();

        /// <summary>
        /// Konstruktor für Datenbankverbindung
        /// Zwei Fälle müssen wir unterscheiden:
        /// -----------------------------------------
        /// A:Normalbetrieb wird durch die ASP.NET - Runtime automatisch
        /// ein MSSqlAuto erzeugt, weil im Startup.cs in der Methode ConfigureServices
        /// eingeführrt wurde.
        /// -----------------------------------------
        /// B:Im Testbetrieb wollen wir die Datenbank faken, darum erzeugen wir
        /// im Projekt Garage.Test jeweils ein Objekt AutoFake dieses implemeniert
        /// das Interface IAuto und dieses entspricht MSSqlAuto
        /// IAuto db = new AutoFake();
        /// var controller = new AutoController(db);
        /// -----------------------------------------
        /// </summary>
        /// <param name="mySqlAuto"></param>
        public AutoController(IAuto mySqlAuto)
        {
            db = mySqlAuto;
        }


        /// <summary>
        /// Zeigt: /Auto (zum löschen)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Loeschen(int? id)
        {
            try
            {
                db.Connect();
                var autos = db.GetAutos(id.ToString());
                var auto = autos.First();

                AutoBildAnzeigen(auto);

                return View(auto);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Fehler", new { msg = ex.Message });
            }
            finally
            {
                db.Close();
            }
        }

        private void AutoBildAnzeigen(Auto auto)
        {
            if (auto.Pic != null)
            {
                string imageBase64 = Convert.ToBase64String(auto.Pic);
                string imageSrc = string.Format("data:image/gif;base64,{0}", imageBase64);
                ViewBag.ImageSource = imageSrc;
            }
            else
            {
                ViewBag.ImageSource = "";
            }
        }

        /// <summary>
        /// Löscht: Auto
        /// </summary>
        /// <param name="auto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Loeschen(Auto auto)
        {
            MSSqlAuto db = new MSSqlAuto();
            try
            {
                db.Connect();
                db.Delete(auto.PK_Auto.ToString());
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Fehler", new { msg = ex.Message });
            }
            finally
            {
                db.Close();
            }
        }

        /// <summary>
        /// Das erzeugte Auto wird zu View weitergeleitet
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Auto());
        }

        /// <summary>
        /// Erzeugt: Auto
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(Auto auto, IFormFile file)
        {
            BildLesen(auto, file);
            try
            {
                db.Connect();
                db.Create(auto);


            }
            catch (Exception ex)
            {
                return RedirectToAction("Groesse", new { msg = ex.Message });
            }
            finally
            {
                db.Close();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Bearbeitet: Auto
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            try
            {
                db.Connect();
                List<Auto> autos = db.GetAutos(Id.ToString());
                var auto = autos.First();

                return View(auto);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Fehler", new { msg = ex.Message });
            }
            finally
            {
                db.Close();
            }
        }

        /// <summary>
        /// wird zu index weitergeleitet
        /// </summary>
        /// <param name="auto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Auto auto, IFormFile file)
        {
            BildLesen(auto, file);
            try
            {
                db.Connect();
                db.Update(auto);


            }
            catch (Exception ex)
            {
                return RedirectToAction("Groesse", new { msg = ex.Message });
            }
            finally
            {
                db.Close();
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Bild aus Formular lesen
        /// </summary>
        /// <param name="auto"></param>
        /// <param name="file"></param>
        private static void BildLesen(Auto auto, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var fs1 = file.OpenReadStream())
                using (var ms1 = new MemoryStream())
                {
                    fs1.CopyTo(ms1, (int)file.Length - 1);
                    auto.Pic = ms1.ToArray();
                }

            }
        }

        /// <summary>
        /// Anzeige wenn etwas nicht gelöscht wurde
        /// </summary>
        /// /// <returns></returns>
        public IActionResult NichtGeloescht()
        {
            return View();
        }

        /// <summary>
        /// Auto wird der View weitergeleitet
        /// </summary>
        /// <returns></returns>
        public IActionResult Loeschen()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            try
            {
                db.Connect();
                var autos = db.GetAutos(id.ToString());
                var auto = autos.First(e => e.PK_Auto == id);

                AutoBildAnzeigen(auto);

                return View(auto);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Fehler", new { msg = ex.Message });
            }
            finally
            {
                db.Close();
            }

        }

        public IActionResult BuyCar(int id)
        {
            try
            {
                db.Connect();
                var autos = db.GetAutos(id.ToString());
                var auto = autos.First(e => e.PK_Auto == id);

                AutoBildAnzeigen(auto);

                return View(auto);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Fehler", new { msg = ex.Message });
            }
            finally
            {
                db.Close();
            }

        }
        /// <summary>
        /// Fehler wird der View weitergeleitet
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IActionResult Fehler(string msg)
        {
            ViewBag.Message = msg;
            ViewBag.TelNr = "044 905 20 26";
            return View();
        }

        public IActionResult Groesse(string msg)
        {
            ViewBag.Message = msg;

            ViewBag.TelNr = "044 905 20 26";
            return View();
        }


        public IActionResult IndexCard(string sortOrder)
        {
            try
            {
                db.Connect();

                if (sortOrder == "ASC")
                {
                    var autos = db.GetAutos().OrderBy(x => x.Marke);
                    return View(autos);
                }
                else if (sortOrder == "DESC")
                {
                    var autos = db.GetAutos().OrderByDescending(x => x.Marke);
                    return View(autos);
                }
                else
                {
                    //unsortiert
                    return View(db.GetAutos());
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Fehler", new { msg = ex.Message });
            }
            finally
            {
                db.Close();
            }
        }

        /// <summary>
        ///  Zeigt eine Liste von allen Autos an
        /// </summary>
        ///<param name="sortOrder">ASC aufsteigen, DESC absteigend, null gleich unsortiert</param>
        /// <param name="sTreibstoffFilter">Benzin, Gas, etc. oder null gleich unsortiert</param> 
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            string sortOrder = "";
            string sTreibstoffFilter = "";
            int? nPageStart = 0;

            // In eine Session ablegen
            if (HttpContext.Session.IsAvailable)
            {
                sortOrder = HttpContext.Session.GetString(SORT_ORDER);
                sTreibstoffFilter = HttpContext.Session.GetString(TREIBSTOFFFILTER);
                nPageStart = HttpContext.Session.GetInt32(PAGE_START);
                nPageStart = nPageStart == null ? 0 : nPageStart;
            }

            try
            {
                db.Connect();

                if (sortOrder == "ASC")
                {
                    var autos = db.GetAutos().OrderBy(x => x.Marke);
                    return View(autos);
                }
                else if (sortOrder == "DESC")
                {
                    var autos = db.GetAutos().OrderByDescending(x => x.Marke);
                    return View(autos);
                }
                else if (!string.IsNullOrEmpty(sTreibstoffFilter))
                {
                    var autos = db.GetAutos().Where(x => x.Treibstoff == sTreibstoffFilter);
                    return View(autos);
                }
                else
                {
                    // unsortiert     
                    return View(db.GetAutos("", nPageStart.GetValueOrDefault()));
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Fehler", new { msg = ex.Message });
            }
            finally
            {
                db.Close();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string sortOrder, string sTreibstoffFilter, string sPage)
        {
            int nPageStart = 0; // Blättern
            if (HttpContext.Session.IsAvailable)
            {
                HttpContext.Session.SetString(SORT_ORDER, sortOrder != null ? sortOrder : "");
                HttpContext.Session.SetString(TREIBSTOFFFILTER, sTreibstoffFilter != null ? sTreibstoffFilter : "");

                //Bestehender Startwert lesen
                nPageStart = HttpContext.Session.GetInt32(PAGE_START).GetValueOrDefault();

                if (!string.IsNullOrEmpty(sPage))
                {
                    //NEXT oder BACK
                    nPageStart += sPage == "NEXT" ? PAGE_SIZE : -PAGE_SIZE;
                    nPageStart = nPageStart < 0 ? 0 : nPageStart;
                    HttpContext.Session.SetInt32(PAGE_START, nPageStart);
                }
            }

            try
            {
                db.Connect();

                if (sortOrder == "ASC")
                {
                    var autos = db.GetAutos().OrderBy(x => x.Marke);
                    return View(autos);
                }
                else if (sortOrder == "DESC")
                {
                    var autos = db.GetAutos().OrderByDescending(x => x.Marke);
                    return View(autos);
                }
                else if (!string.IsNullOrEmpty(sTreibstoffFilter))
                {
                    var autos = db.GetAutos().Where(x => x.Treibstoff == sTreibstoffFilter);
                    return View(autos);
                }
                else
                {
                    // unsortiert     
                    return View(db.GetAutos("", nPageStart));
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Fehler", new { msg = ex.Message });
            }
            finally
            {
                db.Close();
            }
        }

    }
}