using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Agentie_Imobiliara.Pages.Clients
{
    public class Interogare3Model : PageModel
    {
        public List<ProgramareViewModel> ProgramareList { get; set; } = new List<ProgramareViewModel>();
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
                               P.Localizare, PV.Data_vizita, PV.Ora_vizita, PV.Status_programare
                        FROM Programare_vizita PV
                        JOIN Utilizator U ON PV.UtilizatorID = U.UtilizatorID
                        JOIN Proprietate P ON PV.ProprietateID = P.ProprietateID
                        WHERE PV.Data_vizita >= GETDATE()
                        ORDER BY PV.Data_vizita, PV.Ora_vizita";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProgramareList.Add(new ProgramareViewModel
                                {
                                    UtilizatorNume = reader["UtilizatorNume"].ToString(),
                                    UtilizatorPrenume = reader["UtilizatorPrenume"].ToString(),
                                    Localizare = reader["Localizare"].ToString(),
                                    DataVizita = reader["Data_vizita"].ToString(),
                                    OraVizita = reader["Ora_vizita"].ToString(),
                                    StatusProgramare = reader["Status_programare"].ToString(),
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

    public class ProgramareViewModel
    {
        public string UtilizatorNume { get; set; }
        public string UtilizatorPrenume { get; set; }
        public string Localizare { get; set; }
        public string DataVizita { get; set; }
        public string OraVizita { get; set; }
        public string StatusProgramare { get; set; }
    }
}
