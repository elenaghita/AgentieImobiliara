using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Agentie_Imobiliara.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Parola { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-256DFHR\\SQLEXPRESS;Initial Catalog=Anunturi_imobiliare;Integrated Security=True;Trust Server Certificate=True";
                using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    // Interogare pentru verificarea utilizatorului
                    string sql = "SELECT COUNT(*) FROM Utilizator WHERE Email = @Email AND Parola = @Parola";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Parola", Parola);

                        int userExists = (int)command.ExecuteScalar();

                        if (userExists > 0)
                        {
                            // Login reușit - Redirecționează utilizatorul către pagina principală
                            return RedirectToPage("/Index");
                        }
                        else
                        {
                            // Login eșuat - Mesaj de eroare
                            ErrorMessage = "Emailul sau parola sunt incorecte!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Gestionare erori
                ErrorMessage = "Eroare la conectarea cu baza de date: " + ex.Message;
            }

            return Page();
        }
    }
}
