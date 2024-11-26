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

                // Show linked account information for the user if available
                DisplayLinkedAccounts();
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
                    // If the account exists, insert into the LinkedAccounts table
                    LinkAccountToUser(bankAccountID, flowAccountNumber, accountDetails.Balance); // Pass account balance
                    lblMessage.Text = "Account linked successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;

                    // Refresh the linked accounts info after a successful link
                    DisplayLinkedAccounts();
                }
                else
                {
                    lblMessage.Text = "FLOW account number does not exist.";
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

            // Check if the FLOW account number is already linked to the user
            if (IsFlowAccountAlreadyLinked(userId, flowAccountNumber))
            {
                lblMessage.Text = "This FLOW account is already linked to your account.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Proceed with linking the account since it's not already linked
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
                    lblMessage.Text = "Account linked successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;

                    // Refresh the linked accounts info after a successful link
                    DisplayLinkedAccounts();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "An error occurred while linking the account: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private bool IsFlowAccountAlreadyLinked(string userId, string flowAccountNumber)
        {
            // Check if the FLOW account number is already linked to the user
            string query = "SELECT COUNT(*) FROM LinkedAccounts WHERE UserID = @UserID AND FlowAccountNumber = @FlowAccountNumber";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@FlowAccountNumber", flowAccountNumber);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar(); // Execute the query and get the count

                    // If count is greater than 0, it means the FLOW account is already linked
                    return count > 0;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "An error occurred while checking for existing link: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return false;
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

        private void DisplayLinkedAccounts()
        {
            // Get the current user ID from session
            string userId = Session["UserID"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                litLinkedAccountInfo.Text = "User is not logged in.";
                return;
            }

            // Fetch the linked accounts for the user from the database
            var linkedAccounts = GetLinkedAccountsForUser(userId);

            if (linkedAccounts.Count > 0)
            {
                // Create a string to display the linked account information
                string accountInfo = "<h3>Linked Accounts:</h3><ul>";
                foreach (var account in linkedAccounts)
                {
                    accountInfo += $"<li>Bank Account ID: {account.BankAccountID}, FLOW Account Number: {account.FlowAccountNumber}</li>";
                }
                accountInfo += "</ul>";

                litLinkedAccountInfo.Text = accountInfo;
            }
            else
            {
                litLinkedAccountInfo.Text = "No linked accounts found.";
            }
        }

        private List<LinkedAccount> GetLinkedAccountsForUser(string userId)
        {
            List<LinkedAccount> linkedAccounts = new List<LinkedAccount>();

            string query = "SELECT BankAccountID, FlowAccountNumber FROM LinkedAccounts WHERE UserID = @UserID";

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
                        LinkedAccount linkedAccount = new LinkedAccount
                        {
                            BankAccountID = reader.GetInt32(0),
                            // Cast FlowAccountNumber to string explicitly
                            FlowAccountNumber = reader.GetInt64(1).ToString()  // Convert from long to string
                        };
                        linkedAccounts.Add(linkedAccount);
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "An error occurred while retrieving linked accounts: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }

            return linkedAccounts;
        }


        // LinkedAccount class to represent the data fetched from the database
        public class LinkedAccount
        {
            public int BankAccountID { get; set; }
            public string FlowAccountNumber { get; set; }
        }

        // Sample BankAccount class to simulate bank account data
        public class BankAccount
        {
            public int AccountID { get; set; }
            public long AccountNumber { get; set; }
        }
    }
}