using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SagicorAccount.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userID = Session["UserID"] as string;

                // Validate UserID
                if (string.IsNullOrEmpty(userID))
                {
                    lblError.Text = "Session expired. Please log in again.";
                    lblError.CssClass = "text-danger";
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                // Attempt to load data from the database
                LoadUserData(userID);
            }
        }

        private void LoadUserData(string userID)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Load linked accounts
                    LoadDataToGridView(conn,
                        "SELECT LinkID, BankAccountID, Balance FROM LinkedAccounts WHERE UserID = @UserID",
                        userID,
                        gvLinkedAccounts,
                        lblLinkedAccountsStatus,
                        "No linked accounts found.");

                    // Load bank accounts
                    LoadDataToGridView(conn,
                        "SELECT AccountID, AccountNumber, Balance FROM BankAccounts WHERE UserID = @UserID",
                        userID,
                        gvBankAccounts,
                        lblBankAccountsStatus,
                        "No bank accounts found.");

                    // Load recent transactions (limit to 10)
                    LoadDataToGridView(conn,
                        "SELECT TOP 10 TransactionID, Amount, TransactionType, Date, Narrative FROM BankTransactions WHERE UserID = @UserID ORDER BY Date DESC",
                        userID,
                        gvTransactions,
                        lblTransactionsStatus,
                        "No recent transactions found.");
                }
            }
            catch (Exception ex)
            {
                // Display error message and log exception
                lblError.Text = "An unexpected error occurred. Please try again later.";
                lblError.CssClass = "text-danger";
                System.Diagnostics.Debug.WriteLine("Error in LoadUserData: " + ex.Message);
            }
        }

        private void LoadDataToGridView(SqlConnection conn, string query, string userID, GridView gridView, Label statusLabel, string emptyMessage)
        {
            try
            {
                if (conn == null || gridView == null || string.IsNullOrEmpty(userID))
                {
                    throw new ArgumentException("Invalid parameters passed to LoadDataToGridView.");
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Check if data is available
                        if (dt.Rows.Count > 0)
                        {
                            gridView.DataSource = dt;
                            gridView.DataBind();
                            statusLabel.Text = ""; // Clear any previous messages
                        }
                        else
                        {
                            gridView.DataSource = null;
                            gridView.DataBind();
                            statusLabel.Text = emptyMessage; // Display friendly empty message
                            statusLabel.CssClass = "text-info";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error and show user-friendly message
                statusLabel.Text = "An error occurred while loading data. Please try again.";
                statusLabel.CssClass = "text-danger";
                System.Diagnostics.Debug.WriteLine("Error in LoadDataToGridView: " + ex.Message);
            }
        }
    }
}
