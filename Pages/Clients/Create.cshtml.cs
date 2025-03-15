using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Agentie_Imobiliara.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {

        }

        public void OnPost() 
        {
            clientInfo.Nume = Request.Form["Nume"];
            clientInfo.Prenume = Request.Form["Prenume"];
            clientInfo.Email = Request.Form["Email"];
            clientInfo.Telefon = Request.Form["Telefon"];
            clientInfo.Parola = Request.Form["Parola"];

            if (clientInfo.Nume.Length == 0 || clientInfo.Prenume.Length == 0 || clientInfo.Email.Length == 0 || clientInfo.Telefon.Length == 0 || clientInfo.Parola.Length == 0)
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
                    String SQL = "INSERT INTO Utilizator " +
                        "(Nume , Prenume , Email , Telefon , Parola) VALUES" +
                        "(@Nume , @Prenume , @Email , @Telefon , @Parola);";

                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
                        command.Parameters.AddWithValue("@Nume", clientInfo.Nume);
                        command.Parameters.AddWithValue("@Prenume", clientInfo.Prenume);
                        command.Parameters.AddWithValue("@Email", clientInfo.Email);
                        command.Parameters.AddWithValue("@Telefon", clientInfo.Telefon);
                        command.Parameters.AddWithValue("@Parola", clientInfo.Parola);

                        command.ExecuteNonQuery();
                    }

                }
            }

            catch(Exception ex)
            { 
                errorMessage = ex.Message;
                return;
            }

            clientInfo.Nume = ""; clientInfo.Prenume = " "; clientInfo.Email = ""; clientInfo.Telefon = " "; clientInfo.Parola = " ";
            successMessage = "New Client Added Correctly";

            Response.Redirect("/Clients/Index");

        }
    }
}
