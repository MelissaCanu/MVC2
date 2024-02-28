using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using U2S2E3.Models;

namespace U2S2E3.Controllers
{
    public class HomeController : Controller
    {
        //qua ottengo i prodotti in due viste: una per gli utenti standard l'altra per admin

        // GET: Home
        public ActionResult Index()
        {
            Utente utenteLoggato = Session["Utente"] as Utente;

            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            //creo lista prodotti di tipo Prodotto
            List<Prodotto> prodotti = new List<Prodotto>();

            try
            {
                connection.Open();

                //mostro solo i prodotti con InVetrina = 1
                string query = "SELECT * FROM Prodotti";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                //ciclo while per leggere i dati dal db
                while (reader.Read())
                {
                    Prodotto prodotto = new Prodotto()
                    {
                        IDArticolo = Convert.ToInt32(reader["IDArticolo"]),
                        Nome = reader["Nome"].ToString(),
                        Prezzo = Convert.ToDecimal(reader["Prezzo"]),
                        Descrizione = reader["Descrizione"].ToString(),
                        ImgCopertina = reader["ImgCopertina"].ToString(),
                        ImgAggiuntiva1 = reader["ImgAggiuntiva1"].ToString(),
                        ImgAggiuntiva2 = reader["ImgAggiuntiva2"].ToString(),
                        InVetrina = Convert.ToBoolean(reader["InVetrina"])
                    };
                    prodotti.Add(prodotto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei dati", ex);
            }
            finally
            {
                connection.Close();
            }

            if (utenteLoggato != null && utenteLoggato.Ruolo == "Admin")
            {
                // L'utente è un admin, quindi restituisco la vista per l'admin
                return View("IndexAdmin", prodotti);
            }
            else
            {
                // L'utente non è un admin, quindi restituisco la vista standard
                //espressione lambda utilizzata per filtrare una lista di oggetti in C#:
                //prendo prodotto p e restituisco il valore del campo InVetrina -> se è true lo aggiungo alla lista
                prodotti = prodotti.Where(p => p.InVetrina).ToList();
                return View(prodotti);
            }
        }

        // GET: TogliDaVetrina con parametro id 
        public ActionResult TogliDaVetrina(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            {                   
                // Query per settare InVetrina a 0 per l'articolo in base all'id
                string query = "UPDATE Prodotti SET InVetrina = 0 WHERE IDArticolo = @IDArticolo";
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                
                using (SqlCommand command = new SqlCommand(query, connection))
                {   
                    //aggiungo il parametro id all'update 
                    command.Parameters.AddWithValue("@IDArticolo", id);
                    
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("IndexAdmin");
        }

        // GET: AddToVetrina con parametro id
        public ActionResult AddToVetrina(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            {
                // Query per settare InVetrina a 1 per l'articolo in base all'id
                string query = "UPDATE Prodotti SET InVetrina = 1 WHERE IDArticolo = @IDArticolo";
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    //aggiungo il parametro id all'update 
                    command.Parameters.AddWithValue("@IDArticolo", id);

                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("IndexAdmin");
        }

        
       

    }
}
