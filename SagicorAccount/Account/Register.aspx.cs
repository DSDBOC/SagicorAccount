using System;
using System.Configuration; // For accessing the connection string
using System.Data.SqlClient; // For database operations
using System.Security.Cryptography; // For password hashing
using System.Text; // For encoding
using System.Web.UI;

namespace SagicorAccount.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            // Validate inputs
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

            // Hash the password
            string passwordHash = HashPassword(password);

            // Retrieve the connection string
            string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Check if username or email already exists
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

                    // Insert new user
                    string insertQuery = @"
                        INSERT INTO AspNetUsers 
                        (Id, FirstName, LastName, UserName, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount) 
                        VALUES 
                        (@Id, @FirstName, @LastName, @UserName, @Email, @EmailConfirmed, @PasswordHash, @SecurityStamp, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnabled, @AccessFailedCount)";

                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                    // Generate unique values
                    string userId = Guid.NewGuid().ToString();
                    string securityStamp = Guid.NewGuid().ToString();

                    insertCommand.Parameters.AddWithValue("@Id", userId);
                    insertCommand.Parameters.AddWithValue("@FirstName", firstName);
                    insertCommand.Parameters.AddWithValue("@LastName", lastName);
                    insertCommand.Parameters.AddWithValue("@UserName", userName);
                    insertCommand.Parameters.AddWithValue("@Email", email);
                    insertCommand.Parameters.AddWithValue("@EmailConfirmed", false);
                    insertCommand.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    insertCommand.Parameters.AddWithValue("@SecurityStamp", securityStamp);
                    insertCommand.Parameters.AddWithValue("@PhoneNumberConfirmed", false);
                    insertCommand.Parameters.AddWithValue("@TwoFactorEnabled", false);
                    insertCommand.Parameters.AddWithValue("@LockoutEnabled", false);
                    insertCommand.Parameters.AddWithValue("@AccessFailedCount", 0);

                    insertCommand.ExecuteNonQuery();

                    // Redirect to login or success page
                    Response.Redirect("~/Account/Login.aspx");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ErrorMessage.Text = "An error occurred while creating the account. Please try again.";
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
        }

        // Hash the password using SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
