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
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection connection = new SqlConnection(connectionString);

            //creo lista prodotti di tipo Prodotto
            List<Prodotto> prodotti = new List<Prodotto>();

            try
            {
                connection.Open();
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

            return View(prodotti);
        }
    }
}
