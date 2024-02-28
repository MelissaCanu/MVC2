using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using U2S2E3.Models; // AGGIUNGI QUESTA LINEAAAAAAAHHHH !!! 

namespace U2S2E3.Controllers
{
    //creato controller per gestire login utente normale o admin
    public class AccountController : Controller
    {
        private List<Utente> utenti = new List<Utente>();

        // GET: verifico se utente è admin o utente normale e apro la sessione
        public ActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                Utente admin = new Utente();
                admin.Username = username;
                admin.Password = password;
                admin.Ruolo = "Admin";

                Session["Utente"] = admin;
                return RedirectToAction("Index", "Home");
            }
            //foreach per verificare se utente è presente nella lista utenti
            foreach (var utente in utenti)
            {
                if (utente.Username == username && utente.Password == password)
                {
                    Session["Utente"] = utente;
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Register");

           //non ho bisogno di view perché reindirizzo l'utente a una nuova pagina
        }

            //creo metodo per registrazione utente e aggiungo utente alla lista utenti
            public ActionResult Register(string username, string password)
        {
            Utente nuovoUtente = new Utente();
            nuovoUtente.Username = username;
            nuovoUtente.Password = password;
            nuovoUtente.Ruolo = "Utente";

            utenti.Add(nuovoUtente);

            return RedirectToAction("Login");
        }
    }
}