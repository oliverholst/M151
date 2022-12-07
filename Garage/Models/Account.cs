using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Models
{
    public class Account
    {
        //Meldung
        private const string max45Zeichen = "max. 45 Zeichen";

        /// <summary>
        /// Primary Key
        /// </summary>
        public int PK_Account { get; set; }

        /// <summary>
        /// Passwort
        /// </summary>
        [Required(ErrorMessage = "Passwort wird benötigt.")]
        [StringLength(45, ErrorMessage = max45Zeichen)]
        public string Passwort { get; set; }

        /// <summary>
        /// Benutzername
        /// </summary>
        [Required(ErrorMessage = "Email wird benötigt.")]
        [StringLength(45, ErrorMessage = max45Zeichen)]
        public string Benutzername { get; set; }
    }
}
