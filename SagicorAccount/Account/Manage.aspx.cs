using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.Util;

namespace SagicorAccount.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserData();
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Load Linked Accounts
                using (SqlCommand cmd = new SqlCommand("SELECT LinkID, BankAccountID, Balance FROM LinkedAccounts WHERE UserID = @UserID", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        gvLinkedAccounts.DataSource = dt;
                        gvLinkedAccounts.DataBind();
                    }
                }

                // Load Bank Accounts
                using (SqlCommand cmd = new SqlCommand("SELECT AccountID, AccountNumber, Balance FROM BankAccounts WHERE UserID = @UserID", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        gvBankAccounts.DataSource = dt;
                        gvBankAccounts.DataBind();
                    }
                }

                // Load Transactions
                using (SqlCommand cmd = new SqlCommand("SELECT TransactionID, Amount, TransactionType, Date, Narrative FROM BankTransactions WHERE UserID = @UserID", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        gvTransactions.DataSource = dt;
                        gvTransactions.DataBind();
                    }
                }
            }
        }
    }
}
