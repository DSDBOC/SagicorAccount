using System;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Configuration;
using System.Data.SqlClient;

namespace SagicorAccount.Account
{
    public partial class LinkAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Ensure user is logged in
                if (Session["UserID"] == null)
                {
                    lblMessage.Text = "You must be logged in to link an account.";
                    lblMessage.CssClass = "text-danger";
                    btnLinkAccount.Enabled = false;
                }
            }
        }

        protected void btnLinkAccount_Click(object sender, EventArgs e)
        {
            string flowAccountNumber = txtFlowAccountNumber.Text.Trim();

            try
            {
                // Validate flow account number format (basic example)
                if (string.IsNullOrWhiteSpace(flowAccountNumber) || flowAccountNumber.Length != 5 || !long.TryParse(flowAccountNumber, out _))
                {
                    lblMessage.Text = "Invalid account number format.";
                    lblMessage.CssClass = "text-warning";
                    return;
                }

                // Verify the account on the FLOW/SAGICOR LIFE platform
                var accountDetails = VerifyAccountOnFlowService(flowAccountNumber);

                if (accountDetails != null)
                {
                    // Check if account is already linked
                    if (IsAccountAlreadyLinked(flowAccountNumber))
                    {
                        lblMessage.Text = "This account is already linked.";
                        lblMessage.CssClass = "text-warning";
                        return;
                    }

                    // Display account details
                    DisplayAccountDetails(accountDetails);

                    // Link the account to the user in a transaction
                    LinkAccountToUser(flowAccountNumber);

                    lblMessage.Text = "Account successfully linked!";
                    lblMessage.CssClass = "text-success";
                }
                else
                {
                    lblMessage.Text = "The account does not exist on the FLOW platform.";
                    lblMessage.CssClass = "text-danger";
                }
            }
            catch (Exception ex)
            {
                // Log the exception (for debugging and monitoring)
                System.Diagnostics.Debug.WriteLine(ex.Message);  // This is just a log; consider logging to a file or DB for production
                lblMessage.Text = "An error occurred. Please try again later. " + ex.Message;
                lblMessage.CssClass = "text-danger"; // Optional: show more detailed error in the message
            }
        }


        private FlowService.AccountDetails VerifyAccountOnFlowService(string accountNumber)
        {
            try
            {
                // Create an instance of the FlowService class and call the VerifyAccountExists web method
                FlowService flowService = new FlowService();
                return flowService.VerifyAccountExists(accountNumber);
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine("Error verifying account: " + ex.Message);
                throw new ApplicationException("There was an issue verifying the account. Please try again later.");
            }
        }

        private void DisplayAccountDetails(FlowService.AccountDetails accountDetails)
        {
            lblName.Text = "Name: " + accountDetails.Name; // Show account holder's name
            lblAccountNumber.Text = "Account Number: " + accountDetails.AccountNumber;
            lblAccountType.Text = "Account Type: " + accountDetails.AccountType;
            lblBalance.Text = "Balance: $" + accountDetails.Balance.ToString("F2");
        }

        private bool IsAccountAlreadyLinked(string flowAccountNumber)
        {
            string userID = Session["UserID"] as string;
            string query = "SELECT COUNT(*) FROM LinkedAccounts WHERE FlowAccountNumber = @FlowAccountNumber AND UserID = @UserID";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FlowAccountNumber", flowAccountNumber);
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error checking account link: " + ex.Message);
                throw new ApplicationException("There was an error checking if the account is already linked.");
            }
        }

        private void LinkAccountToUser(string flowAccountNumber)
        {
            string userID = Session["UserID"] as string;

            // Retrieve the BankAccountID for the user (you might need to fetch this depending on your logic)
            Guid bankAccountID = GetBankAccountID(userID);

            string query = "INSERT INTO LinkedAccounts (UserID, BankAccountID, FlowAccountNumber) VALUES (@UserID, @BankAccountID, @FlowAccountNumber)";

            // Use transaction to ensure data consistency
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@BankAccountID", bankAccountID);  // Pass BankAccountID
                        cmd.Parameters.AddWithValue("@FlowAccountNumber", flowAccountNumber);
                        cmd.ExecuteNonQuery();
                    }

                    // Commit the transaction
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Rollback the transaction in case of an error
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine("Error linking account: " + ex.Message);
                    throw new ApplicationException("There was an error linking the account.");
                }
            }
        }

        private Guid GetBankAccountID(string userID)
        {
            // Example logic to retrieve the BankAccountID for the user (you will need to implement this based on your system)
            string query = "SELECT AccountID FROM BankAccounts WHERE UserID = @UserID";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    conn.Open();

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return (Guid)result;
                    }
                    else
                    {
                        throw new ApplicationException("No bank account found for the user.");
                    }
                }
            }
        }
    }
}
