using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace SagicorAccount.Account
{
    public partial class Login : Page
    {
        // Connection string
        private string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any previous error messages
                ErrorMessage.Visible = false;
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            string email = Email.Text.Trim();
            string password = Password.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                FailureText.Text = "Email and password are required.";
                ErrorMessage.Visible = true;
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to get hashed password and user details from your database
                    string query = "SELECT Id, FirstName, LastName, PasswordHash FROM AspNetUsers WHERE Email = @Email";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string userID = reader["Id"].ToString();
                        string firstName = reader["FirstName"].ToString();
                        string lastName = reader["LastName"].ToString();
                        string storedPasswordHash = reader["PasswordHash"].ToString();

                        // Verify the password
                        if (VerifyPassword(password, storedPasswordHash))
                        {
                            // Successful login: store user details in session
                            Session["Email"] = email;
                            Session["FirstName"] = firstName;
                            Session["LastName"] = lastName;
                            Session["UserID"] = userID;  // Store the User ID as well

                            Response.Redirect("WebForm1.aspx"); // Redirect to the user's dashboard
                        }
                        else
                        {
                            FailureText.Text = "Invalid email or password.";
                            ErrorMessage.Visible = true;
                        }
                    }
                    else
                    {
                        FailureText.Text = "Invalid email or password.";
                        ErrorMessage.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log and display error
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                FailureText.Text = "An error occurred while trying to log in. Please try again.";
                ErrorMessage.Visible = true;
            }
        }

        // Verify the hashed password
        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] enteredHashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
                string enteredHash = Convert.ToBase64String(enteredHashBytes);

                return enteredHash == storedHash;
            }
        }
    }
}
