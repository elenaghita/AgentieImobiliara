using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;


namespace Agentie_Imobiliara.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClients = new List<ClientInfo>();
        
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";

                using (Microsoft.Data.SqlClient.SqlConnection connection =  new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                { 
                    connection.Open();
                    String sql = "SELECT * FROM Utilizator";
                    using (Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
                    {
                        using (Microsoft.Data.SqlClient.SqlDataReader reader = command.ExecuteReader())
                        { 
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.UtilizatorID = "" + reader.GetInt32(0);
                                clientInfo.Nume = reader.GetString(1);
                                clientInfo.Prenume = reader.GetString(2);
                                clientInfo.Email = reader.GetString(3);
                                clientInfo.Telefon = reader.GetString(4);
                                clientInfo.Parola = reader.GetString(5);

                                listClients.Add(clientInfo);
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
    public class ClientInfo
    {
        public String UtilizatorID;
        public String Nume;
        public String Prenume;
        public String Email;
        public String Telefon;
        public String Parola;
    }
}
