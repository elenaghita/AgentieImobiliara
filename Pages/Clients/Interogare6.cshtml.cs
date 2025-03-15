using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Agentie_Imobiliara.Pages.Clients
{
    public class Interogare6Model : PageModel
    {
        public List<UtilizatorViewModel> UtilizatoriList { get; set; } = new List<UtilizatorViewModel>();
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
                        WITH ProprietatiScumpe AS (
                            SELECT 
                                P.ProprietateID, 
                                P.Pret
                            FROM 
                                Proprietate P
                            WHERE 
                                P.Pret > (SELECT AVG(P1.Pret) FROM Proprietate P1)
                        )
                        SELECT 
                            CONCAT(U.Nume, ' ', U.Prenume) AS Utilizator,
                            COUNT(PV.ProprietateID) AS NumarVizionariProprietatiScumpe,
                            (SELECT COUNT(*) 
                             FROM Programare_vizita PV2 
                             WHERE PV2.UtilizatorID = U.UtilizatorID) AS TotalVizionari
                        FROM 
                            Utilizator U
                        LEFT JOIN 
                            Programare_vizita PV ON U.UtilizatorID = PV.UtilizatorID
                        INNER JOIN 
                            ProprietatiScumpe PS ON PV.ProprietateID = PS.ProprietateID
                        GROUP BY 
                            U.UtilizatorID, U.Nume, U.Prenume
                        HAVING 
                            COUNT(PV.ProprietateID) > 0
                        ORDER BY 
                            NumarVizionariProprietatiScumpe DESC;
                    ";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UtilizatoriList.Add(new UtilizatorViewModel
                                {
                                    Utilizator = reader["Utilizator"].ToString(),
                                    NumarVizionariProprietatiScumpe = Convert.ToInt32(reader["NumarVizionariProprietatiScumpe"]),
                                    TotalVizionari = Convert.ToInt32(reader["TotalVizionari"])
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

    public class UtilizatorViewModel
    {
        public string Utilizator { get; set; }
        public int NumarVizionariProprietatiScumpe { get; set; }
        public int TotalVizionari { get; set; }
    }
}
