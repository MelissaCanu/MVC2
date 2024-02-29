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

                //espressione "lambda" utilizzata per filtrare una lista di oggetti in C#:
                //prendo prodotto p e restituisco il valore del campo InVetrina -> se è true lo aggiungo alla lista
                prodotti = prodotti.Where(p => p.InVetrina).ToList();
                return View(prodotti);
            }
        }

   
        //GET: DETTAGLI 
        public ActionResult Dettagli(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            //dichiaro e inizializzo variabile prodotto a null
            Prodotto prodotto = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //apro connessione e seleziono tramite query prodotto con id specifico
                string query = "SELECT * FROM Prodotti WHERE IDArticolo = @IDArticolo";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    //aggiungo il parametro id alla query
                    command.Parameters.AddWithValue("@IDArticolo", id);
                    connection.Open();
                    //eseguo la query e leggo i dati 
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            prodotto = new Prodotto()
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
                        }
                    }
                }
            }
            //se il prodotto è null, reindirizzo a Index
            if (prodotto == null)
            {
                return RedirectToAction("Index");
            }
            //altrimenti restituisco la vista con il prodotto
            return View(prodotto);
        }


    //*** NEI MIEI METODI PER LA GESTIONE DEI PRODOTTI DA ADMIN ->
    // USO IL METODO POST PERCHE' MODIFICO IL DB, OVVERO LO STATO DEI PRODOTTI ***  

    // GET: TogliDaVetrina con parametro id 
    [HttpPost]
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
            TempData["Message"] = "Prodotti nascosti dalla vetrina con successo.";
            return RedirectToAction("Index");
        }

        // GET: AddToVetrina con parametro id
        [HttpPost]
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
            TempData["Message"] = "Prodotti aggiunti alla vetrina con successo.";
            return RedirectToAction("Index"); //reindirizzo alla action index che porta alla view IndexAdmin
        }




    }
}


