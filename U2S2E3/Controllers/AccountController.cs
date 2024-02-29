
//********* Aggiungo [HttpPost] alle azioni per indicare che dovrebbero essere invocate
//solo in risposta a una richiesta POST.

//Aggiungo un parametro Utente utente a ciascuna azione
//per accettare un'istanza del modello Utente.


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using U2S2E3.Models; // AGGIUNGI QUESTA LINEAAAAAAAHHHH !!! 

namespace U2S2E3.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Utente utente)
        {
            // Verifica se il modello è valido
            if (ModelState.IsValid)
            {
                // Mi connetto al db con uno using per evitare di lasciare la connessione aperta
                string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Query
                    string query = "SELECT * FROM Utenti WHERE Username = @Username AND Password = @Password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", utente.Username);
                        command.Parameters.AddWithValue("@Password", utente.Password);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // HasRows è un metodo che restituisce true se il reader contiene una o più righe
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    utente.Ruolo = reader["Ruolo"].ToString();
                                }
                                Session["Utente"] = utente;
                                return RedirectToAction("Index", "Home");
                            }
                        }
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
            // Verifica se il modello è valido
            if (ModelState.IsValid)
            {
                // Mi connetto al db con uno using per evitare di lasciare la connessione aperta
                string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Query
                    string query = "SELECT * FROM Utenti WHERE Username = @Username";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", utente.Username);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // HasRows è un metodo che restituisce true se il reader contiene una o più righe
                            if (reader.HasRows)
                            {
                                ViewBag.Error = "Username già esistente. Scegli un altro username.";
                                return View(utente);
                            }
                        }
                    }

                    // Se il modello è valido, inserisco l'utente nel db
                    query = "INSERT INTO Utenti (Username, Password, Ruolo) VALUES (@Username, @Password, @Ruolo)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", utente.Username);
                        command.Parameters.AddWithValue("@Password", utente.Password);
                        command.Parameters.AddWithValue("@Ruolo", "Utente");
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Login");
            }

            // Se il modello non è valido, restituisco la vista di registrazione con il modello
            return View(utente);
        }
    }
}

