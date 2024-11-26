using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Configuration;

namespace SagicorAccount.Account
{
    public partial class WebForm1 : Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is logged in (i.e., session variables are set)
            if (Session["UserID"] != null)
            {
                // Retrieve user ID from session
                string userID = Session["UserID"].ToString();

                // Retrieve user details from the database
                GetUserDetails(userID);
            }
            else
            {
                // Redirect to login page if session is not available (i.e., user is not logged in)
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        private void GetUserDetails(string userID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserName, Email, FirstName, LastName FROM AspNetUsers WHERE Id = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userID);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read(); // Read the first (and only) row
                        string firstName = reader["FirstName"].ToString();
                        string lastName = reader["LastName"].ToString();
                        string userName = reader["UserName"].ToString();
                        string email = reader["Email"].ToString();

                        // Display user details on the page
                        lblUserID.Text = "" + userID;
                        lblFullName.Text = "" + firstName + " " + lastName;
                        lblUserName.Text = "" + userName;
                        lblEmail.Text = "" + email;
                    }
                    else
                    {
                        // User not found, redirect or show error message
                        lblUserID.Text = "User not found.";
                    }
                }
                catch (Exception ex)
                {
                    // Log the error or handle it as needed
                    lblUserID.Text = "Error retrieving user details: " + ex.Message;
                }
            }
        }


    }
}
