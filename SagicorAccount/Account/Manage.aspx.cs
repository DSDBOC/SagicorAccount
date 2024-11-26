using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Activities;

namespace SagicorAccount.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userID = Session["UserID"] as string;

                if (string.IsNullOrEmpty(userID))
                {
                    lblError.Text = "Session expired. Please log in again.";
                    lblError.CssClass = "text-danger";
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                try
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        LoadDataToGridView(conn,
                            "SELECT LinkID, BankAccountID, Balance FROM LinkedAccounts WHERE UserID = @UserID",
                            userID,
                            gvLinkedAccounts,
                            lblLinkedAccountsStatus,
                            "No linked accounts found.");

                        LoadDataToGridView(conn,
                            "SELECT AccountID, AccountNumber, Balance FROM BankAccounts WHERE UserID = @UserID",
                            userID,
                            gvBankAccounts,
                            lblBankAccountsStatus,
                            "No bank accounts found.");

                        LoadDataToGridView(conn,
                            "SELECT TransactionID, Amount, TransactionType, Date, Narrative FROM BankTransactions WHERE UserID = @UserID ORDER BY Date DESC",
                            userID,
                            gvTransactions,
                            lblTransactionsStatus,
                            "No recent transactions found.");
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "An unexpected error occurred. Please try again later.";
                    lblError.CssClass = "text-danger";

                    // Log the exception details for debugging
                    System.Diagnostics.Debug.WriteLine("Error in Page_Load: " + ex.Message);
                }
            }
        }


        private void LoadUserData()
        {
            string userID = Session["UserID"] as string;

            if (string.IsNullOrEmpty(userID))
            {
                // Redirect to login page if session is null
                Response.Redirect("~/Login.aspx");
                return;
            }

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Load Linked Accounts
                    string linkedAccountsQuery = "SELECT LinkID, BankAccountID, Balance FROM LinkedAccounts WHERE UserID = @UserID";
                    LoadDataToGridView(conn, linkedAccountsQuery, userID, gvLinkedAccounts, lblLinkedAccountsStatus, "No linked accounts found.");

                    // Load Bank Accounts
                    string bankAccountsQuery = "SELECT AccountID, AccountNumber, Balance FROM BankAccounts WHERE UserID = @UserID";
                    LoadDataToGridView(conn, bankAccountsQuery, userID, gvBankAccounts, lblBankAccountsStatus, "No bank accounts found.");

                    // Load Transactions (most recent first)
                    string transactionsQuery = "SELECT TOP 10 TransactionID, Amount, TransactionType, Date, Narrative FROM BankTransactions WHERE UserID = @UserID ORDER BY Date DESC";
                    LoadDataToGridView(conn, transactionsQuery, userID, gvTransactions, lblTransactionsStatus, "No transactions found.");
                }
            }
            catch (Exception ex)
            {
                // Log exception (can use logging framework)
                lblError.Text = "An error occurred while loading your data. Please try again later.";
                lblError.CssClass = "text-danger";
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads data into a GridView and updates the status label based on the results.
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="query">SQL query</param>
        /// <param name="userID">User ID for filtering</param>
        /// <param name="gridView">GridView to bind data</param>
        /// <param name="statusLabel">Label to display status</param>
        /// <param name="emptyMessage">Message to display if no data is found</param>
        private void LoadDataToGridView(SqlConnection conn, string query, string userID, GridView gridView, Label statusLabel, string emptyMessage)
        {
            try
            {
                // Validate input parameters
                if (conn == null)
                    throw new ArgumentNullException(nameof(conn), "SqlConnection cannot be null.");
                if (gridView == null)
                    throw new ArgumentNullException(nameof(gridView), "GridView control cannot be null.");
                if (string.IsNullOrEmpty(userID))
                    throw new ArgumentNullException(nameof(userID), "UserID cannot be null or empty.");

                // Execute the query and populate the GridView
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

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
                // Log error and display user-friendly message
                statusLabel.Text = "An error occurred while loading data. Please try again.";
                statusLabel.CssClass = "text-danger";

                // Log the exception details (optional logging mechanism)
                System.Diagnostics.Debug.WriteLine("Error in LoadDataToGridView: " + ex.Message);
            }
        }

    }
}
