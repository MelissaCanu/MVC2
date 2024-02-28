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
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Ruolo { get; set; }
    }
}