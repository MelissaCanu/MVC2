using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace U2S2E3.Models
{
    public class Utente
    {
        public int IDUtente { get; set; }
        
        [Required(ErrorMessage = "Inserisci un username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Inserisci una password")]
        public string Password { get; set; }
        public string Ruolo { get; set; }
    }
}