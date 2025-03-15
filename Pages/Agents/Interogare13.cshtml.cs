using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Agentie_Imobiliara.Pages.Agents
{
    public class Interogare13Model : PageModel
    {
        public List<InteroInfo> listInterogare = new List<InteroInfo>();
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    
                    String sql = @"
                        SELECT 
                            U.UtilizatorID,
                            CONCAT(U.Nume, ' ', U.Prenume) AS NumeUtilizator,
                            (SELECT COUNT(DISTINCT P.Localizare) 
                             FROM Programare_vizita PV 
                             INNER JOIN Proprietate P ON PV.ProprietateID = P.ProprietateID 
                             WHERE PV.UtilizatorID = U.UtilizatorID) AS LocatiiVizitate,
                            (SELECT TOP 1 P.Localizare 
                             FROM Programare_vizita PV 
                             INNER JOIN Proprietate P ON PV.ProprietateID = P.ProprietateID 
                             WHERE PV.UtilizatorID = U.UtilizatorID 
                             ORDER BY P.Pret DESC) AS LocatiePretMaxim,
                            (SELECT TOP 1 P.Pret 
                             FROM Programare_vizita PV 
                             INNER JOIN Proprietate P ON PV.ProprietateID = P.ProprietateID 
                             WHERE PV.UtilizatorID = U.UtilizatorID 
                             ORDER BY P.Pret DESC) AS PretMaxim
                        FROM 
                            Utilizator U
                        ORDER BY 
                            LocatiiVizitate DESC, PretMaxim DESC, NumeUtilizator ASC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                InteroInfo interogareInfo = new InteroInfo
                                {
                                    UtilizatorID = reader["UtilizatorID"].ToString(),
                                    NumeUtilizator = reader["NumeUtilizator"].ToString(),
                                    LocatiiVizitate = reader["LocatiiVizitate"].ToString(),
                                    LocatiePretMaxim = reader["LocatiePretMaxim"].ToString(),
                                    PretMaxim = reader["PretMaxim"].ToString()
                                };

                                listInterogare.Add(interogareInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Eroare: " + ex.Message;
            }
        }
    }

    public class InteroInfo
    {
        public string UtilizatorID { get; set; }
        public string NumeUtilizator { get; set; }
        public string LocatiiVizitate { get; set; }
        public string LocatiePretMaxim { get; set; }
        public string PretMaxim { get; set; }
    }
}
