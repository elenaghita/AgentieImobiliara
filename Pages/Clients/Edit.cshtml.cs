using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Agentie_Imobiliara.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo ClientInfo = new ClientInfo();
        public String errorMessage = " ";
        public String successMessage = " ";

        public void OnGet()
        {
            Console.WriteLine("OnGet method called");
            String UtilizatorID = Request.Query["UtilizatorID"];

            if (string.IsNullOrEmpty(UtilizatorID))
            {
                errorMessage = "UtilizatorID is missing in the query string.";
                return; 
            }


            try
            {
                String connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";

                using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Utilizator WHERE UtilizatorID=@UtilizatorID";
                    using (Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@UtilizatorID", UtilizatorID);
                        using (Microsoft.Data.SqlClient.SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                ClientInfo.UtilizatorID = "" + reader.GetInt32(0);
                                ClientInfo.Nume = reader.GetString(1);
                                ClientInfo.Prenume = reader.GetString(2);
                                ClientInfo.Email = reader.GetString(3);
                                ClientInfo.Telefon = reader.GetString(4);
                                ClientInfo.Parola = reader.GetString(5);
                            }
                        }
                    }
                }
            }


            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            ClientInfo.UtilizatorID = Request.Form["UtilizatorID"];
            ClientInfo.Nume = Request.Form["Nume"];
            ClientInfo.Prenume = Request.Form["Prenume"];
            ClientInfo.Email = Request.Form["Email"];
            ClientInfo.Telefon = Request.Form["Telefon"];
            ClientInfo.Parola = Request.Form["Parola"];

            if (ClientInfo.UtilizatorID.Length == 0 || ClientInfo.Nume.Length == 0 || ClientInfo.Prenume.Length == 0 || ClientInfo.Email.Length == 0 || ClientInfo.Telefon.Length == 0 || ClientInfo.Parola.Length == 0)
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
                    String SQL = "UPDATE Utilizator " +
                                 "SET Nume=@Nume, Prenume=@Prenume, Email=@Email, Telefon=@Telefon, Parola=@Parola " +
                                 "WHERE UtilizatorID=@UtilizatorID";



                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
                        command.Parameters.AddWithValue("@Nume", ClientInfo.Nume);
                        command.Parameters.AddWithValue("@Prenume", ClientInfo.Prenume);
                        command.Parameters.AddWithValue("@Email", ClientInfo.Email);
                        command.Parameters.AddWithValue("@Telefon", ClientInfo.Telefon);
                        command.Parameters.AddWithValue("@Parola", ClientInfo.Parola);
                        command.Parameters.AddWithValue("UtilizatorID", ClientInfo.UtilizatorID);

                        command.ExecuteNonQuery();
                    }
                }
            }


            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Clients/Index");
        }
    }


}

