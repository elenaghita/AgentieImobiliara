using Agentie_Imobiliara.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Agentie_Imobiliara.Pages.Agents
{
    public class EditaModel : PageModel
    {
        public AgentInfo AgentInfo = new AgentInfo();
        public String errorMessage = " ";
        public String successMessage = " ";
        public void OnGet()
        {
            Console.WriteLine("OnGet method called");
            String AgentImobiliarID = Request.Query["AgentImobiliarID"];

            if (string.IsNullOrEmpty(AgentImobiliarID))
            {
                errorMessage = "AgentImobiliarID is missing in the query string.";
                return;
            }


            try
            {
                String connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";

                using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM AgentImobiliar WHERE AgentImobiliarID=@AgentImobiliarID";
                    using (Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@AgentImobiliarID", AgentImobiliarID);
                        using (Microsoft.Data.SqlClient.SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                AgentInfo.AgentImobiliarID = "" + reader.GetInt32(0);
                                AgentInfo.Nume = reader.GetString(1);
                                AgentInfo.Prenume = reader.GetString(2);
                                AgentInfo.Telefon = reader.GetString(3);
                                AgentInfo.Email = reader.GetString(4);
                                AgentInfo.Licenta = reader.GetString(5);
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
            AgentInfo.AgentImobiliarID = Request.Form["AgentImobiliarID"];
            AgentInfo.Nume = Request.Form["Nume"];
            AgentInfo.Prenume = Request.Form["Prenume"];
            AgentInfo.Telefon = Request.Form["Telefon"];
            AgentInfo.Email = Request.Form["Email"];
            AgentInfo.Licenta = Request.Form["Licenta"];

            if (AgentInfo.AgentImobiliarID.Length == 0 || AgentInfo.Nume.Length == 0 || AgentInfo.Prenume.Length == 0 || AgentInfo.Telefon.Length == 0 || AgentInfo.Email.Length == 0 || AgentInfo.Licenta.Length == 0)
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
                    String SQL = "UPDATE AgentImobiliar " +
                                 "SET Nume=@Nume, Prenume=@Prenume, Telefon=@Telefon , Email=@Email, Licenta=@Licenta " +
                                 "WHERE  AgentImobiliarID=@AgentImobiliarID";



                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
                        command.Parameters.AddWithValue("@Nume", AgentInfo.Nume);
                        command.Parameters.AddWithValue("@Prenume", AgentInfo.Prenume);
                        command.Parameters.AddWithValue("@Telefon", AgentInfo.Telefon);
                        command.Parameters.AddWithValue("@Email", AgentInfo.Email);
                        command.Parameters.AddWithValue("@Licenta", AgentInfo.Licenta);
                        command.Parameters.AddWithValue("AgentImobiliarID", AgentInfo.AgentImobiliarID);

                        command.ExecuteNonQuery();
                    }
                }
            }


            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Agents/Indexa");
        }

    }
}
