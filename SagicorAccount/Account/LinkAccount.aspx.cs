using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SagicorAccount.Account
{
    public partial class LinkAccount : System.Web.UI.Page
    {
        // Database connection string
        private string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load the bank accounts into the dropdown list
                LoadBankAccounts();
            }
        }

        private void LoadBankAccounts()
        {
            string userID = Session["UserID"] as string;

            if (string.IsNullOrEmpty(userID))
            {
                Response.Redirect("~/Account/Login");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT AccountID, AccountType FROM BankAccounts WHERE UserID = @UserID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Bind the bank accounts to the dropdown
                    ddlBankAccount.DataSource = reader;
                    ddlBankAccount.DataTextField = "AccountType";  // Display account type
                    ddlBankAccount.DataValueField = "AccountID";   // Store account ID
                    ddlBankAccount.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading bank accounts: " + ex.Message;
                lblMessage.CssClass = "text-danger";
                lblMessage.Visible = true;
            }
        }

        protected void btnLinkAccount_Click(object sender, EventArgs e)
        {
            string flowAccountNumber = txtFlowAccount.Text.Trim();
            string selectedAccountID = ddlBankAccount.SelectedValue;

            if (string.IsNullOrEmpty(flowAccountNumber) || selectedAccountID == "0")
            {
                lblMessage.Text = "Please enter a valid FLOW account number and select a bank account.";
                lblMessage.CssClass = "text-danger";
                lblMessage.Visible = true;
                return;
            }

            // Verify the FLOW account using the web service
            string verificationResult = VerifyFlowAccount(flowAccountNumber);

            if (verificationResult.Contains("Account exists"))
            {
                // Link the account in the database
                LinkAccountToUser(flowAccountNumber, selectedAccountID);

                lblMessage.Text = "FLOW account linked successfully!";
                lblMessage.CssClass = "text-success";
                lblMessage.Visible = true;
            }
            else
            {
                lblMessage.Text = "FLOW account not found. Please check the account number.";
                lblMessage.CssClass = "text-danger";
                lblMessage.Visible = true;
            }
        }

        // Method to verify FLOW account (this can be a call to the FlowService web service)
        private string VerifyFlowAccount(string accountNumber)
        {
            // Call your FlowService to verify the account
            // Simulate verification for the demo
            if (accountNumber == "FLOW123")
            {
                return "Account exists: John Doe";
            }
            else
            {
                return "Account not found.";
            }
        }

        // Method to link the account to the user's bank account
        private void LinkAccountToUser(string flowAccountNumber, string bankAccountID)
        {
            string userID = Session["UserID"] as string;

            if (string.IsNullOrEmpty(userID))
            {
                Response.Redirect("~/Account/Login");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        INSERT INTO LinkedAccounts (UserID, BankAccountID, FlowAccountNumber)
                        VALUES (@UserID, @BankAccountID, @FlowAccountNumber)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@BankAccountID", bankAccountID);
                    cmd.Parameters.AddWithValue("@FlowAccountNumber", flowAccountNumber);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error linking account: " + ex.Message;
                lblMessage.CssClass = "text-danger";
                lblMessage.Visible = true;
            }
        }
    }
}
