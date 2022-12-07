using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Garage.Models
{
    /// <summary>
    /// Das ist ein Feedback
    /// </summary>
    public class Feedback
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int PK_Feedback { get; set; }

        /// <summary>
        /// Name, Nachname , z.B. Hans Muster
        /// </summary>
        [Required]
        [DisplayName("Vorname, Nachname")]
        public string Name { get; set; }

        /// <summary>
        /// Email, z.B. beispiel@server.com
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email: (beispiel@server.com)")]
        public string Email { get; set; }

        /// <summary>
        /// DropDown-Control für Fahrzeug Art: SUV, Cabriolet,
        /// </summary>
        [DisplayName("Fahrzeug-Art:")]
        public string FahrzeugArt { get; set; }

        /// <summary>
        /// Eine Textarea für Mitteilungen
        /// </summary>
        [Required]
        [DataType(DataType.MultilineText)]
        public string Mitteilung { get; set; }

        /// <summary>
        /// Radio Buttons für Service 4 Varianten
        /// </summary>
        [DisplayName("Sind Sie mit dem Service zufrieden? ")]
        public int Service { get; set; }

        /// <summary>
        /// Radio Buttons für Mitteilungen 4 Varianten
        /// </summary>
        [DisplayName("Sind Sie mit der Modellauswahl zufrieden? ")]
        public int Modellauswahl { get; set; }

        /// <summary>
        /// Liefert anhand einer Zahl den ensprechenden String
        /// </summary>
        /// <returns></returns>
        public string GetServiceQuality(int nMeinung)
        {
            switch (nMeinung)
            {
                case 0: return "Gut";
                case 1: return "Geht so";
                case 2: return "Schlecht";
                default: return "keine Meinung";
            }
        }

        /// <summary>
        /// Wird benötigt um eine DropDown-Liste im Formular anzuzeigen.
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public static SelectList FahrzeugArtListe { get; private set; }
            = new SelectList(new[] { "", "SUV", "Cabrio", "Kleinwagen",
                "Kombi", "Van/Kleinbus", "Geländewagen", "Limousine",
                "Sportwagen/Coupé" });
    }
}