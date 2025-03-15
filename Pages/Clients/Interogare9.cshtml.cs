using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;


namespace Agentie_Imobiliara.Pages.Clients
{
    public class Interogare9Model : PageModel
    {
        public List<AgentImobiliarViewModel> Agenti { get; set; } = new List<AgentImobiliarViewModel>();
        [BindProperty(SupportsGet = true)]
        public string Nume { get; set; } 

        public void OnGet()
        {
            string connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";

            using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        AgentImobiliarID,
                        CONCAT(Nume, ' ', Prenume) AS NumeAgent,
                        Telefon,
                        Email,
                        Licenta
                    FROM 
                        AgentImobiliar
                    WHERE 
                        Nume LIKE '%' + @Nume + '%'
                    ORDER BY 
                        Nume, Prenume;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nume", string.IsNullOrEmpty(Nume) ? "" : Nume);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Agenti.Add(new AgentImobiliarViewModel
                            {
                                AgentImobiliarID = reader.GetInt32(0),
                                NumeAgent = reader.GetString(1),
                                Telefon = reader.GetString(2),
                                Email = reader.GetString(3),
                                Licenta = reader.GetString(4)
                            });
                        }
                    }
                }
            }
        }
    }

    public class AgentImobiliarViewModel
    {
        public int AgentImobiliarID { get; set; }
        public string NumeAgent { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Licenta { get; set; }
    }
}
