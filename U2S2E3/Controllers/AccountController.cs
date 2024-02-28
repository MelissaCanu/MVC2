using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using U2S2E3.Models; // AGGIUNGI QUESTA LINEAAAAAAAHHHH !!! 

//********* Aggiungo [HttpPost] alle azioni per indicare che dovrebbero essere invocate
//solo in risposta a una richiesta POST.

//Aggiungo un parametro Utente utente a ciascuna azione
//per accettare un'istanza del modello Utente.


namespace U2S2E3.Controllers
{
   
    public class AccountController : Controller
    {
        private List<Utente> utenti = new List<Utente>();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        //verifico se utente è admin o utente normale e apro la sessione
        [HttpPost]
        public ActionResult Login(Utente utente)
        {
            if (ModelState.IsValid)
            {
                if (utente.Username == "admin" && utente.Password == "admin")
                {
                    utente.Ruolo = "Admin";
                    Session["Utente"] = utente;
                    return RedirectToAction("Index", "Home");
                }

                foreach (var u in utenti)
                {
                    if (u.Username == utente.Username && u.Password == utente.Password)
                    {
                        Session["Utente"] = u;
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            // Se il modello non è valido o se non viene trovato nessun utente, reindirizza alla registrazione
            return RedirectToAction("Register");
        }

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Utente utente)
        {
            if (ModelState.IsValid)
            {
                foreach (var u in utenti)
                {
                    if (u.Username == utente.Username)
                    {
                        ViewBag.Error = "Username già esistente. Scegli un altro username.";
                        return View(utente);
                    }
                }

                utente.Ruolo = "Utente";
                utenti.Add(utente);

                return RedirectToAction("Login");
            }

            // Se il modello non è valido, restituisci la vista di registrazione con il modello
            return View(utente);
        }
    }
}