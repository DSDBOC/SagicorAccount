using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SagicorAccount.Account
{
    public partial class LinkingAcc : Page
    {
        // Create an instance of the FlowService class
        FlowService FlowService = new FlowService();

        // Connection string for the database (replace with your actual connection string)
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Populate the bank account dropdown list when the page loads
                PopulateBankAccounts();
            }
        }

        private void PopulateBankAccounts()
        {
            // Get the current user ID from session or authentication
            string userId = Session["UserID"]?.ToString(); // Ensure the user ID is correctly stored in session

            if (string.IsNullOrEmpty(userId))
            {
                lblMessage.Text = "User is not logged in.";
                return;
            }

            // Fetch the list of bank accounts associated with the user
            var bankAccounts = GetBankAccountsForUser(userId); // This method will query your database

            // Populate the dropdown with bank accounts
            ddlBankAccounts.Items.Clear();
            ddlBankAccounts.Items.Add(new ListItem("Select Bank Account", ""));
            foreach (var account in bankAccounts)
            {
                ddlBankAccounts.Items.Add(new ListItem(account.AccountNumber.ToString(), account.AccountID.ToString()));
            }
        }

        protected void btnLinkAccount_Click(object sender, EventArgs e)
        {
            // Get selected bank account ID and FLOW account number from user input
            string bankAccountID = ddlBankAccounts.SelectedValue;
            string flowAccountNumber = txtFlowAccountNumber.Text.Trim();

            // Validate inputs
            if (string.IsNullOrEmpty(bankAccountID) || string.IsNullOrEmpty(flowAccountNumber))
            {
                lblMessage.Text = "Please select a bank account and enter a FLOW number.";
                lblMessage.ForeColor = System.Drawing.Color.Red; // Ensure proper color
                return;
            }

            try
            {
                // Call the FlowService Web Service to verify if the FLOW account exists
                var accountDetails = FlowService.VerifyAccountExists(flowAccountNumber); // Pass only the FLOW account number

                if (accountDetails != null)
                {
                    // If the account exists, insert into the LinkedAccounts table (you can implement this method)
                    LinkAccountToUser(bankAccountID, flowAccountNumber, accountDetails.Balance); // Pass account balance
                    lblMessage.Text = "Account linked successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblMessage.Text = "FLOW accounts number does not exist.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                lblMessage.Text = "An error occurred while linking the account: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }


        private void LinkAccountToUser(string bankAccountID, string flowAccountNumber, decimal balance)
        {
            // Get user ID from session
            string userId = Session["UserID"]?.ToString(); // Assuming userID is stored in the session

            if (string.IsNullOrEmpty(userId))
            {
                lblMessage.Text = "User not found.";
                return;
            }

            string query = "INSERT INTO LinkedAccounts (UserID, BankAccountID, FlowAccountNumber, Balance) " +
                           "VALUES (@UserID, @BankAccountID, @FlowAccountNumber, @Balance)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@BankAccountID", bankAccountID);
                command.Parameters.AddWithValue("@FlowAccountNumber", flowAccountNumber);
                command.Parameters.AddWithValue("@Balance", balance);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery(); // Executes the insert command
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "An error occurred while linking the account: " + ex.Message;
                }
            }
        }

        // This method retrieves bank accounts for the logged-in user
        private List<BankAccount> GetBankAccountsForUser(string userId)
        {
            List<BankAccount> bankAccounts = new List<BankAccount>();
            string query = "SELECT AccountID, AccountNumber FROM BankAccounts WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        BankAccount account = new BankAccount
                        {
                            AccountID = reader.GetInt32(0),
                            AccountNumber = reader.GetInt64(1)
                        };
                        bankAccounts.Add(account);
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "An error occurred while retrieving bank accounts: " + ex.Message;
                }
            }

            return bankAccounts;
        }

        // This method fetches the list of already linked accounts for the user
        private List<string> GetLinkedAccounts()
        {
            List<string> linkedAccounts = new List<string>();

            string userId = Session["UserID"]?.ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                string query = "SELECT FlowAccountNumber FROM LinkedAccounts WHERE UserID = @UserID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserID", userId);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            linkedAccounts.Add(reader.GetString(0)); // Assuming FlowAccountNumber is a string
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "An error occurred while retrieving linked accounts: " + ex.Message;
                    }
                }
            }

            return linkedAccounts;
        }

        // Sample BankAccount class to simulate bank account data
        public class BankAccount
        {
            public int AccountID { get; set; }
            public long AccountNumber { get; set; }
        }
    }
}
