using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SagicorAccount.Account
{
    public partial class ViewBankAccountDetails : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load existing bank accounts for the logged-in user
                LoadBankAccounts();
            }
        }

        // Method to load bank accounts
        private void LoadBankAccounts()
        {
            string userID = Session["UserID"] as string;

            if (string.IsNullOrEmpty(userID))
            {
                // Redirect to login if no user is logged in
                Response.Redirect("~/Account/Login");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT AccountID, AccountType, AccountNumber, Balance FROM BankAccounts WHERE UserID = @UserID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Check if there are any rows
                    if (reader.HasRows)
                    {
                        gvBankAccounts.DataSource = reader;
                        gvBankAccounts.DataBind();
                    }
                    else
                    {
                        lblMessage.Text = "No bank accounts found.";
                        lblMessage.CssClass = "text-info";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred while fetching your accounts: " + ex.Message;
                lblMessage.CssClass = "text-danger";
            }
        }

        // Event handler for adding new bank account
       
    }
}
