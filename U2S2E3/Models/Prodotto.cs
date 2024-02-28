using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace U2S2E3.Models
{
    public class Prodotto
    {
        public int IDArticolo { get; set; }
        public string Nome { get; set; }
        public decimal Prezzo { get; set; }
        public string Descrizione { get; set; }
        public string ImgCopertina { get; set; }
        public string ImgAggiuntiva1 { get; set; }
        public string ImgAggiuntiva2 { get; set; }
        public bool InVetrina { get; set; }


    }
}