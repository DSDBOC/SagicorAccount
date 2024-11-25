using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace SagicorAccount.Account
{
    public partial class UserDashboard : Page
    {
        // Updated connection string
        private string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load the user's bank accounts
                LoadBankAccounts();
            }
        }

        private void LoadBankAccounts()
        {
            // Ensure the user is logged in
            if (Session["UserID"] != null)
            {
                string userID = Session["UserID"].ToString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Query to fetch bank account details for the user
                    string query = "SELECT AccountType, Balance, AccountNumber FROM BankAccounts WHERE UserID = @UserID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    try
                    {
                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        // Load data into a DataTable
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        // Bind the DataTable to GridView for display
                        gvBankAccountss.DataSource = dt;
                        gvBankAccountss.DataBind();
                    }
                    catch (Exception ex)
                    {
                        // Log and handle errors gracefully
                        System.Diagnostics.Debug.WriteLine("Error loading bank accounts: " + ex.Message);
                        // Display a user-friendly message if necessary
                    }
                }
            }
            else
            {
                // Redirect to login if session is invalid
                Response.Redirect("~/Account/Login");
            }
        }
    }
}