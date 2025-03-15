using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;


namespace Agentie_Imobiliara.Pages.Agents
{
    public class Interogare10Model : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Localizare { get; set; }
        public List<AgentImobiliarData> AgentImobiliarList { get; set; } = new List<AgentImobiliarData>();

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(Localizare))
            {
                string connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";

                try
                {
                    using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = @"
                            SELECT 
                                A.AgentImobiliarID,
                                CONCAT(A.Nume, ' ', A.Prenume) AS NumeAgent,
                                A.Telefon,
                                A.Email,
                                COUNT(P.ProprietateID) AS NumarProprietati
                            FROM 
                                AgentImobiliar A
                            INNER JOIN 
                                Proprietate P ON A.AgentImobiliarID = P.AgentIImobiliarID
                            WHERE 
                                P.Localizare = @Localizare
                            GROUP BY 
                                A.AgentImobiliarID, A.Nume, A.Prenume, A.Telefon, A.Email
                            ORDER BY 
                                NumarProprietati DESC";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Localizare", Localizare);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    AgentImobiliarList.Add(new AgentImobiliarData
                                    {
                                        AgentImobiliarID = reader.GetInt32(0),
                                        NumeAgent = reader.GetString(1),
                                        Telefon = reader.GetString(2),
                                        Email = reader.GetString(3),
                                        NumarProprietati = reader.GetInt32(4)
                                    });
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Log the error or display a user-friendly message
                }
            }
        }

        public class AgentImobiliarData
        {
            public int AgentImobiliarID { get; set; }
            public string NumeAgent { get; set; }
            public string Telefon { get; set; }
            public string Email { get; set; }
            public int NumarProprietati { get; set; }
        }
    }
}
