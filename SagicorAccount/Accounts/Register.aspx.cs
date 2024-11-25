using System;
using System.Configuration; // For accessing the connection string
using System.Data.SqlClient; // For ADO.NET database operations
using System.Web.UI;

namespace SagicorAccount.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            // Validate form inputs
            if (!Page.IsValid)
            {
                ErrorMessage.Text = "Please correct the errors and try again.";
                return;
            }

            string firstName = FirstName.Text.Trim();
            string lastName = LastName.Text.Trim();
            string userName = UserName.Text.Trim();
            string email = Email.Text.Trim();
            string password = Password.Text.Trim();

            // Hash the password (for security)
            string passwordHash = HashPassword(password);

            // Connection string from Web.config
            string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Check if the username or email already exists
                    string checkUserQuery = "SELECT COUNT(*) FROM AspNetUsers WHERE UserName = @UserName OR Email = @Email";
                    SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection);
                    checkUserCommand.Parameters.AddWithValue("@UserName", userName);
                    checkUserCommand.Parameters.AddWithValue("@Email", email);

                    connection.Open();
                    int userExists = (int)checkUserCommand.ExecuteScalar();

                    if (userExists > 0)
                    {
                        ErrorMessage.Text = "The username or email is already taken.";
                        return;
                    }

                    // Insert new user into AspNetUsers table
                    string insertQuery = @"
                        INSERT INTO AspNetUsers 
                        (Id, FirstName, LastName, UserName, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount) 
                        VALUES 
                        (@Id, @FirstName, @LastName, @UserName, @Email, @EmailConfirmed, @PasswordHash, @SecurityStamp, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnabled, @AccessFailedCount)";

                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                    // Generate a unique ID for the user
                    string userId = Guid.NewGuid().ToString();

                    insertCommand.Parameters.AddWithValue("@Id", userId);
                    insertCommand.Parameters.AddWithValue("@FirstName", firstName);
                    insertCommand.Parameters.AddWithValue("@LastName", lastName);
                    insertCommand.Parameters.AddWithValue("@UserName", userName);
                    insertCommand.Parameters.AddWithValue("@Email", email);
                    insertCommand.Parameters.AddWithValue("@EmailConfirmed", false);
                    insertCommand.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    insertCommand.Parameters.AddWithValue("@SecurityStamp", Guid.NewGuid().ToString());
                    insertCommand.Parameters.AddWithValue("@PhoneNumberConfirmed", false);
                    insertCommand.Parameters.AddWithValue("@TwoFactorEnabled", false);
                    insertCommand.Parameters.AddWithValue("@LockoutEnabled", false);
                    insertCommand.Parameters.AddWithValue("@AccessFailedCount", 0);

                    // Execute the insert query
                    insertCommand.ExecuteNonQuery();

                    // Redirect to login page with success message
                    Response.Redirect("~/Accounts/UserDashboard.aspx");
                }
            }
            catch (Exception ex)
            {
                // Display a generic error message and log the exception for debugging
                ErrorMessage.Text = "An error occurred while creating the account. Please try again later.";
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
        }

        // Helper method to hash passwords
        private string HashPassword(string password)
        {
            // For simplicity, using SHA256 hashing (use a stronger library for production)
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }
    }
}
