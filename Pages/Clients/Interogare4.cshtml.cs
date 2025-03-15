using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Agentie_Imobiliara.Pages.Clients
{
    public class Interogare4Model : PageModel
    {
        public List<ProgramareComplexaViewModel> ProgramariList { get; set; } = new List<ProgramareComplexaViewModel>();
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
     CONCAT(U.Nume, ' ', U.Prenume) AS Utilizator,
     P.Descriere AS Proprietate,
     P.Localizare,
     P.Numar_camere AS [NrCamere],
     P.Suprafata,
     P.Pret AS [Pret],
     CONCAT(A.Nume, ' ', A.Prenume) AS [AgentImobiliar],
     PV.Data_vizita AS [DataVizitei],
     PV.Ora_vizita AS [OraVizitei]
 FROM 
     Programare_vizita PV
 INNER JOIN 
     Utilizator U ON PV.UtilizatorID = U.UtilizatorID
 INNER JOIN 
     Proprietate P ON PV.ProprietateID = P.ProprietateID
 INNER JOIN 
     AgentImobiliar A ON P.AgentIImobiliarID = A.AgentImobiliarID
 ORDER BY 
     PV.Data_vizita DESC, PV.Ora_vizita DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProgramariList.Add(new ProgramareComplexaViewModel
                                {
                                    Utilizator = reader["Utilizator"].ToString(),
                                    Proprietate = reader["Proprietate"].ToString(),
                                    Localizare = reader["Localizare"].ToString(),
                                    NrCamere = Convert.ToInt32(reader["NrCamere"]),
                                    
                                    Suprafata = Convert.ToDecimal(reader["Suprafata"].ToString().Replace(" mp", "").Trim()),
                                    Pret = Convert.ToDecimal(reader["Pret"]),
                                    AgentImobiliar = reader["AgentImobiliar"].ToString(),
                                    DataVizitei = Convert.ToDateTime(reader["DataVizitei"]),
                                    OraVizitei = reader["OraVizitei"].ToString()
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

    public class ProgramareComplexaViewModel
    {
        public string Utilizator { get; set; }
        public string Proprietate { get; set; }
        public string Localizare { get; set; }
        public int NrCamere { get; set; }
        public decimal Suprafata { get; set; }
        public decimal Pret { get; set; }
        public string AgentImobiliar { get; set; }
        public DateTime DataVizitei { get; set; }
        public string OraVizitei { get; set; }
    }
}
