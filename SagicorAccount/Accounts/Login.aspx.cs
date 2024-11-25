using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Security.Cryptography;
using System.Text;

namespace SagicorAccount.Account
{
    public partial class Login : Page
    {
        // Connection string
        private string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                // Redirect logged-in users to the dashboard
                Response.Redirect("~/Accounts/UserDashboard.aspx");
            }
        }

        protected void LogIn(object sender, EventArgs e)
{
    string username = Email.Text.Trim();
    string password = Password.Text.Trim();

    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    {
        ShowErrorMessage("Username and Password are required.");
        return;
    }

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        // SQL query to validate credentials
        string query = "SELECT Id, FirstName, LastName FROM AspNetUsers WHERE UserName = @UserName AND PasswordHash = @PasswordHash";

        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@UserName", username);
        cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(password)); // Ensure the password is hashed

        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                // Fetch user details
                string userID = reader["Id"].ToString();
                string firstName = reader["FirstName"].ToString();
                string lastName = reader["LastName"].ToString();

                // Store in session
                Session["UserID"] = userID;
                Session["UserName"] = $"{firstName} {lastName}";

                // Redirect to dashboard
                Response.Redirect("~/Accounts/UserDashboard.aspx");
            }
            else
            {
                // Invalid credentials
                ShowErrorMessage("Invalid username or password.");
            }
        }
        catch (Exception ex)
        {
            // Handle exception
            System.Diagnostics.Debug.WriteLine("Login error: " + ex.Message);
            ShowErrorMessage("An error occurred during login. Please try again later.");
        }
    }
}

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void ShowErrorMessage(string message)
        {
            FailureText.Text = message;
            ErrorMessage.Visible = true;
        }
    }
}
