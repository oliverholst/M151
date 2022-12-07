using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Garage.Models

{/// <summary>
/// Das ist ein Auto
/// </summary>
    public class Auto
    {
        //Meldung
        private const string max45Zeichen = "max. 45 Zeichen";

        /// <summary>
        /// Primary Key
        /// </summary>
        public int PK_Auto { get; set; }

        /// <summary>
        /// Automarke, z.B. Opel
        /// </summary>
        [Required(ErrorMessage = "Automarke wird benötigt.")]
        [StringLength(45, ErrorMessage = max45Zeichen)]
        public string Marke { get; set; }

        /// <summary>
        /// Auto - Modell , z.B. 'Mokka'
        /// </summary>
        [Required(ErrorMessage = "Auto Modellbezeichnung wird benötigt.")]
        [StringLength(45, ErrorMessage = max45Zeichen)]
        public string Modell { get; set; }

        /// <summary>
        /// Farbe
        /// </summary>
        [StringLength(45, ErrorMessage = max45Zeichen)]
        public string Farbe { get; set; }

        /// <summary>
        /// Leistung in PS
        /// </summary>
        [DisplayName("PS")]
        [Range(1, 1000, ErrorMessage = "Leistung zwischen 1..1000")]
        public int Leistung { get; set; } = 1;

        /// <summary>
        /// Preis
        /// </summary>
        [DisplayName("Preis: (CHF)")]
        [DataType(DataType.Currency)]
        [Range(1, 1000000, ErrorMessage = "Preis zwischen 1 und 1 Mio.")]
        public int Preis { get; set; } = 1;

        /// <summary>
        /// Fahrzeug Inverkehrssetzungsdatum
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayName("Jahrgang: ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Jahrgang { get; set; } = DateTime.Now;

        /// <summary>
        /// Treibstoffart
        /// </summary>
        [DisplayName("Treibstoff: ")]
        public string Treibstoff { get; set; } = "Benzin";

        /// <summary>
        /// Fahrzeugbild
        /// </summary>
        [DisplayName("Bild: ")]
        public byte[] Pic { get; set; }

        /// <summary>
        /// Liste für die verschiedenen Treibstoffarten
        /// </summary>
        public IEnumerable<TreibstoffTyp> TreibstoffTypOptions = new List<TreibstoffTyp>
        {
            new TreibstoffTyp {Key = "Benzin", Value = "Benzin"},
            new TreibstoffTyp {Key = "Diesel", Value = "Diesel"},
            new TreibstoffTyp {Key = "Elektro", Value = "Elektro"},
            new TreibstoffTyp {Key = "Hybrid", Value = "Hybrid"},
            new TreibstoffTyp {Key = "Gas", Value = "Gas"}
        };
    }
}