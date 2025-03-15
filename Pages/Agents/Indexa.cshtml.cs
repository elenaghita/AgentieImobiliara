using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;


namespace Agentie_Imobiliara.Pages.Agents
{
    public class IndexaModel : PageModel
    {
        public List<AgentInfo> listAgents = new List<AgentInfo>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";

                using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM AgentImobiliar";
                    using (Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
                    {
                        using (Microsoft.Data.SqlClient.SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AgentInfo agentInfo = new AgentInfo();
                                agentInfo.AgentImobiliarID = "" + reader.GetInt32(0);
                                agentInfo.Nume = reader.GetString(1);
                                agentInfo.Prenume = reader.GetString(2);
                                agentInfo.Telefon = reader.GetString(3);
                                agentInfo.Email = reader.GetString(4);
                                agentInfo.Licenta = reader.GetString(5);

                                listAgents.Add(agentInfo);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

        }
    }
    public class AgentInfo
    {
        public String AgentImobiliarID;
        public String Nume;
        public String Prenume;
        public String Telefon;
        public String Email;
        public String Licenta;
    }
}
