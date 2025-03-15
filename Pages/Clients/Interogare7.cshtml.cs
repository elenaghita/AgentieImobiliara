using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Agentie_Imobiliara.Pages.Clients
{
    public class Interogare7Model : PageModel
    {
        public List<VizionariUtilizatorViewModel> VizionariList { get; set; } = new List<VizionariUtilizatorViewModel>();
        public string ErrorMessage { get; set; } = "";
        [BindProperty(SupportsGet = true)]
        public int AnulPublicarii { get; set; }

        public void OnGet()
        {
            if (AnulPublicarii == 0)
            {
                ErrorMessage = "Te rugăm să introduci un an valid.";
                return;
            }

            try
            {
                string connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        SELECT 
                            CONCAT(U.Nume, ' ', U.Prenume) AS Utilizator,
                            COUNT(VA.AnuntID) AS NumarVizionari
                        FROM 
                            Utilizator U
                        INNER JOIN 
                            Vizionare_anunt VA ON U.UtilizatorID = VA.UtilizatorID
                        INNER JOIN 
                            Anunt A ON VA.AnuntID = A.AnuntID
                        WHERE 
                            YEAR(A.Data_publicare) = @AnulPublicarii
                        GROUP BY 
                            U.UtilizatorID, U.Nume, U.Prenume
                        ORDER BY 
                            NumarVizionari DESC;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@AnulPublicarii", AnulPublicarii);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VizionariList.Add(new VizionariUtilizatorViewModel
                                {
                                    NumeUtilizator = reader["Utilizator"].ToString(),
                                    NumarVizionari = Convert.ToInt32(reader["NumarVizionari"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }

    public class VizionariUtilizatorViewModel
    {
        public string NumeUtilizator { get; set; }
        public int NumarVizionari { get; set; }
    }
}
