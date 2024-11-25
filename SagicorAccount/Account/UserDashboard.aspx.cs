using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace SagicorAccount.Account
{
    public partial class UserDashboard : Page
    {
        // Connection string to the database
        private string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load the user's bank accounts if user is logged in
                LoadBankAccounts();
            }
        }

        // Method to load the user's bank accounts
        private void LoadBankAccounts()
        {
            // Ensure the user is logged in
            if (Session["UserID"] != null)
            {
                string userID = Session["UserID"].ToString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Query to fetch the bank account details for the user from the BankAccounts table
                    string query = @"
                        SELECT AccountID, AccountType, Balance, AccountNumber 
                        FROM BankAccounts 
                        WHERE UserID = @UserID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        // Load the data into a DataTable
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        // Bind the DataTable to GridView to display the data
                        gvBankAccounts.DataSource = dt;
                        gvBankAccounts.DataBind();
                    }
                    catch (Exception ex)
                    {
                        // Log the error and display a user-friendly message if necessary
                        System.Diagnostics.Debug.WriteLine("Error loading bank accounts: " + ex.Message);
                        // Optionally, show an error message in a label or alert on the page
                    }
                }
            }
            else
            {
                // If the user is not logged in, redirect to the login page
                Response.Redirect("~/Account/Login.aspx");
            }
        }
    }
}
