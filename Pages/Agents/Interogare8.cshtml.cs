using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;


namespace Agentie_Imobiliara.Pages.Agents
{
    public class Interogare8Model : PageModel
    {
        public List<AgentImobiliarInfo> ListaAgenti { get; set; } = new List<AgentImobiliarInfo>();

        public void OnGet()
        {
            string connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";

            using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = @"
                        WITH ProprietatiAgent AS (
                        SELECT 
                            A.AgentImobiliarID,
                            P.ProprietateID,
                            P.Pret
                        FROM 
                            AgentImobiliar A
                        LEFT JOIN 
                            Proprietate P ON A.AgentImobiliarID = P.AgentIImobiliarID
                    ),
                    VizionariAgent AS (
                        SELECT 
                            PA.AgentImobiliarID,
                            COUNT(PV.ProprietateID) AS TotalVizionari
                        FROM 
                            ProprietatiAgent PA
                        LEFT JOIN 
                            Programare_vizita PV ON PA.ProprietateID = PV.ProprietateID
                        GROUP BY 
                            PA.AgentImobiliarID
                    )
                    SELECT 
                        A.AgentImobiliarID,
                        CONCAT(A.Nume, ' ', A.Prenume) AS NumeAgent,
                        COUNT(DISTINCT PA.ProprietateID) AS NrProprietatiGestionate,
                        SUM(PA.Pret) AS ValoareTotalaProprietati,
                        VA.TotalVizionari,
                        CASE 
                            WHEN SUM(PA.Pret) > 30000 THEN 'Agent Premium'
                            ELSE 'Agent Standard'
                        END AS TipAgent
                    FROM 
                        AgentImobiliar A
                    LEFT JOIN 
                        ProprietatiAgent PA ON A.AgentImobiliarID = PA.AgentImobiliarID
                    LEFT JOIN 
                        VizionariAgent VA ON A.AgentImobiliarID = VA.AgentImobiliarID
                    GROUP BY 
                        A.AgentImobiliarID, A.Nume, A.Prenume, VA.TotalVizionari
                    ORDER BY 
                        ValoareTotalaProprietati DESC, NrProprietatiGestionate DESC;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AgentImobiliarInfo agent = new AgentImobiliarInfo
                            {
                          

                                AgentImobiliarID = reader.GetInt32(0),
                                NumeAgent = reader.GetString(1),
                                NrProprietatiGestionate = reader.GetInt32(2),
                                ValoareTotalaProprietati = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                                TotalVizionari = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                TipAgent = reader.GetString(5)

                            };

                            ListaAgenti.Add(agent);
                        }
                    }
                }
            }
        }

        public class AgentImobiliarInfo
        {
            public int AgentImobiliarID { get; set; }
            public string NumeAgent { get; set; }
            public int NrProprietatiGestionate { get; set; }
            public decimal ValoareTotalaProprietati { get; set; }
            public int TotalVizionari { get; set; }
            public string TipAgent { get; set; }
        }
    }
}
