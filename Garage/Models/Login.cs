using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage.Models
{
    public class Login
    {
        //Meldung
        private const string max45Zeichen = "max. 45 Zeichen";

        [Required(ErrorMessage = "Email oder Benutzername wird benötigt.")]
        [StringLength(45, ErrorMessage = max45Zeichen)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Passwort wird benötigt.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
