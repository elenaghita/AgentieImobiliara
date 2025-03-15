using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
namespace Agentie_Imobiliara.Pages.Agents
{
    public class Interogare1Model : PageModel
    {

        public List<MedicPacientViewModel> MedicPacientList { get; set; } = new List<MedicPacientViewModel>();
        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";
                using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                         SELECT M.Nume AS AgentNume, M.Prenume AS AgentPrenume, 
                          P.Tip_Proprietate AS TipProprietate
                         FROM AgentImobiliar M
                         INNER JOIN Proprietate P ON M.AgentImobiliarID = P.AgentIImobiliarID
                         ORDER BY M.Nume, M.Prenume";

                    using (Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
                    {
                        using (Microsoft.Data.SqlClient.SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var medicPacient = new MedicPacientViewModel
                                {
                                    AgentNume = reader["AgentNume"].ToString(),
                                    AgentPrenume = reader["AgentPrenume"].ToString(),
                                    TipProprietate = reader["TipProprietate"].ToString(),

                                };

                                MedicPacientList.Add(medicPacient);
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

    public class MedicPacientViewModel
    {
        public string AgentNume { get; set; }
        public string AgentPrenume { get; set; }
        public string TipProprietate { get; set; }

    }
}
