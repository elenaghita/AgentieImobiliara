using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;



namespace Agentie_Imobiliara.Pages.Clients
{
    public class Interogare10Model : PageModel
    {
        public List<ProprietateData> ProprietatiList { get; set; } = new List<ProprietateData>();

        public void OnGet()
        {
            string connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";

            try
            {
                using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            P.ProprietateID,
                            P.Localizare,
                            P.Pret,
                            A.Nume AS NumeAgent,
                            A.Prenume AS PrenumeAgent,
                            COUNT(PV.UtilizatorID) AS NumarProgramari,
                            COALESCE(MAX(PV.Data_vizita), 'Nicio dată') AS UltimaVizita,
                            CASE 
                                WHEN COUNT(PV.UtilizatorID) = 0 THEN 'Fără vizite'
                                WHEN COUNT(PV.UtilizatorID) BETWEEN 1 AND 3 THEN 'Vizite reduse'
                                WHEN COUNT(PV.UtilizatorID) BETWEEN 4 AND 7 THEN 'Vizite moderate'
                                ELSE 'Vizite frecvente'
                            END AS CategorieVizite
                        FROM 
                            Proprietate P
                        LEFT JOIN 
                            AgentImobiliar A ON P.AgentIImobiliarID = A.AgentImobiliarID
                        LEFT JOIN 
                            Programare_vizita PV ON P.ProprietateID = PV.ProprietateID
                        WHERE 
                            P.Pret > 2000
                        GROUP BY 
                            P.ProprietateID, P.Localizare, P.Pret, A.Nume, A.Prenume
                        HAVING 
                            COUNT(PV.UtilizatorID) > 0
                        ORDER BY 
                            CategorieVizite DESC, P.Pret DESC;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProprietatiList.Add(new ProprietateData
                                {
                                    ProprietateID = reader.GetInt32(0),
                                    Localizare = reader.GetString(1),
                                    Pret = reader.GetInt32(2),
                                    NumeAgent = reader.GetString(3) + " " + reader.GetString(4),
                                    NumarProgramari = reader.GetInt32(5),
                                    UltimaVizita = reader.IsDBNull(6) ? "Nicio dată" : reader.GetDateTime(6).ToString("dd-MM-yyyy"), // Conversie DateTime în string
                                    CategorieVizite = reader.GetString(7)


                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log or display an error message
            }
        }

        public class ProprietateData
        {
            public int ProprietateID { get; set; }
            public string Localizare { get; set; }
            public decimal Pret { get; set; }
            public string NumeAgent { get; set; }
            public int NumarProgramari { get; set; }
            public string UltimaVizita { get; set; }
            public string CategorieVizite { get; set; }
        }
    }
}
