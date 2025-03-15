using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Agentie_Imobiliara.Pages.Agents
{
    public class CreateaModel : PageModel
    {
        public AgentInfo agentInfo = new AgentInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {

        }

        public void OnPost()
        {
            agentInfo.Nume = Request.Form["Nume"];
            agentInfo.Prenume = Request.Form["Prenume"];
            agentInfo.Telefon = Request.Form["Telefon"];
            agentInfo.Email = Request.Form["Email"];
            agentInfo.Licenta= Request.Form["Licenta"];

            if (agentInfo.Nume.Length == 0 || agentInfo.Prenume.Length == 0 || agentInfo.Telefon.Length == 0 || agentInfo.Email.Length == 0 || agentInfo.Licenta.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String SQL = "INSERT INTO AgentImobiliar " +
                        "(Nume , Prenume , Telefon , Email , Licenta) VALUES" +
                        "(@Nume , @Prenume , @Telefon , @Email , @Licenta);";

                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
                        command.Parameters.AddWithValue("@Nume", agentInfo.Nume);
                        command.Parameters.AddWithValue("@Prenume", agentInfo.Prenume);
                        command.Parameters.AddWithValue("@Telefon", agentInfo.Telefon);
                        command.Parameters.AddWithValue("@Email", agentInfo.Email);
                        command.Parameters.AddWithValue("@Licenta", agentInfo.Licenta);

                        command.ExecuteNonQuery();
                    }

                }
            }

            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            agentInfo.Nume = ""; agentInfo.Prenume = " "; agentInfo.Telefon = ""; agentInfo.Email = " "; agentInfo.Licenta = " ";
            successMessage = "New Agent Added Correctly";

            Response.Redirect("/Agents/Indexa");

        }
    }
}