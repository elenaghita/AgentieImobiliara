using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Agentie_Imobiliara.Pages.Clients
{
    public class Interogare2Model : PageModel
    {
        public List<VizionareViewModel> VizionareList { get; set; } = new List<VizionareViewModel>();
        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        SELECT U.Nume AS UtilizatorNume, U.Prenume AS UtilizatorPrenume, 
                               P.Localizare, P.Pret, V.Data_vizionare, V.Ora_vizionare
                        FROM Vizionare_anunt V
                        JOIN Utilizator U ON V.UtilizatorID = U.UtilizatorID
                        JOIN Anunt A ON V.AnuntID = A.AnuntID
                        JOIN Proprietate P ON A.ProprietateID = P.ProprietateID
                        ORDER BY U.Nume, U.Prenume";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VizionareList.Add(new VizionareViewModel
                                {
                                    UtilizatorNume = reader["UtilizatorNume"].ToString(),
                                    UtilizatorPrenume = reader["UtilizatorPrenume"].ToString(),
                                    Localizare = reader["Localizare"].ToString(),
                                    Pret = reader["Pret"].ToString(),
                                    DataVizionare = reader["Data_vizionare"].ToString(),
                                    OraVizionare = reader["Ora_vizionare"].ToString(),
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

    public class VizionareViewModel
    {
        public string UtilizatorNume { get; set; }
        public string UtilizatorPrenume { get; set; }
        public string Localizare { get; set; }
        public string Pret { get; set; }
        public string DataVizionare { get; set; }
        public string OraVizionare { get; set; }
    }
}
