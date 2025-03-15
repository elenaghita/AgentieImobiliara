using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;



namespace Agentie_Imobiliara.Pages.Agents
{
    public class Interogare12Model : PageModel
    {
        public List<InterogareInfo> InterogareResult { get; set; } = new List<InterogareInfo>();

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";

                using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = @"
                        SELECT 
                            U.UtilizatorID,
                            CONCAT(U.Nume, ' ', U.Prenume) AS NumeUtilizator,
                            (SELECT COUNT(PV.ProprietateID) 
                             FROM Programare_vizita PV 
                             INNER JOIN Proprietate P ON PV.ProprietateID = P.ProprietateID
                             WHERE PV.UtilizatorID = U.UtilizatorID AND P.Pret > 
                                   (SELECT AVG(P1.Pret) FROM Proprietate P1)) AS VizionariPesteMedia,
                            (SELECT COUNT(PV.ProprietateID) 
                             FROM Programare_vizita PV 
                             WHERE PV.UtilizatorID = U.UtilizatorID) AS VizionariTotale,
                            CASE 
                                WHEN (SELECT COUNT(PV.ProprietateID) 
                                      FROM Programare_vizita PV 
                                      INNER JOIN Proprietate P ON PV.ProprietateID = P.ProprietateID
                                      WHERE PV.UtilizatorID = U.UtilizatorID AND P.Pret > 
                                            (SELECT AVG(P1.Pret) FROM Proprietate P1)) = 0 THEN 'Fără interes'
                                WHEN (SELECT COUNT(PV.ProprietateID) 
                                      FROM Programare_vizita PV 
                                      INNER JOIN Proprietate P ON PV.ProprietateID = P.ProprietateID
                                      WHERE PV.UtilizatorID = U.UtilizatorID AND P.Pret > 
                                            (SELECT AVG(P1.Pret) FROM Proprietate P1)) BETWEEN 1 AND 3 THEN 'Interes redus'
                                ELSE 'Interes mare'
                            END AS CategorieInteres
                        FROM 
                            Utilizator U
                        ORDER BY 
                            VizionariPesteMedia DESC, NumeUtilizator ASC;
                    ";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                InterogareInfo interogareInfo = new InterogareInfo
                                {
                                    UtilizatorID = reader.GetInt32(0),
                                    NumeUtilizator = reader.GetString(1),
                                    VizionariPesteMedia = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                                    VizionariTotale = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                                    CategorieInteres = reader.GetString(4)
                                };

                                InterogareResult.Add(interogareInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }

    public class InterogareInfo
    {
        public int UtilizatorID { get; set; }
        public string NumeUtilizator { get; set; }
        public int VizionariPesteMedia { get; set; }
        public int VizionariTotale { get; set; }
        public string CategorieInteres { get; set; }
    }
}
