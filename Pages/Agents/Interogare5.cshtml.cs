using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Agentie_Imobiliara.Pages.Agents
{
    public class Interogare5Model : PageModel
    {
        public List<AgentComplexViewModel> AgentiList { get; set; } = new List<AgentComplexViewModel>();
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
                        SELECT 
     CONCAT(A.Nume, ' ', A.Prenume) AS [Agent Imobiliar],
     (SELECT COUNT(*) 
      FROM Proprietate P 
      WHERE P.AgentIImobiliarID = A.AgentImobiliarID) AS [Număr Proprietăți Gestionate],
     COUNT(PV.AnuntID) AS [Număr Vizionări]
 FROM 
     AgentImobiliar A
 LEFT JOIN 
     Proprietate P ON A.AgentImobiliarID = P.AgentIImobiliarID
 LEFT JOIN 
     Vizionare_anunt PV ON P.ProprietateID = PV.AnuntID
 GROUP BY 
     A.AgentImobiliarID, A.Nume, A.Prenume
 ORDER BY 
     [Număr Vizionări] DESC, [Agent Imobiliar] ASC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AgentiList.Add(new AgentComplexViewModel
                                {
                                    AgentImobiliar = reader["Agent Imobiliar"].ToString(),
                                    NumarProprietati = Convert.ToInt32(reader["Număr Proprietăți Gestionate"]),
                                    NumarVizionari = Convert.ToInt32(reader["Număr Vizionări"])
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

    public class AgentComplexViewModel
    {
        public string AgentImobiliar { get; set; }
        public int NumarProprietati { get; set; }
        public int NumarVizionari { get; set; }
    }
}
