using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using SagicorAccount.Class;

namespace SagicorAccount.Account
{
    public partial class MakingPayments : Page
    {
        // Initialize FlowService and PaymentService
        FlowService flowService = new FlowService();
        PaymentService paymentService = new PaymentService();

        // Connection string for database
        private string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Populate dropdowns for linked accounts and bank accounts
                PopulateBankAccounts();
                PopulateLinkedAccounts();
            }
        }

        private void PopulateBankAccounts()
        {
            string userId = Session["UserID"]?.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                lblMessage.Text = "User is not logged in.";
                return;
            }

            string query = "SELECT AccountID, AccountNumber FROM BankAccounts WHERE UserID = @UserID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    ddlBankAccounts.Items.Clear();
                    ddlBankAccounts.Items.Add(new ListItem("Select Bank Account", ""));
                    while (reader.Read())
                    {
                        ddlBankAccounts.Items.Add(new ListItem(reader["AccountNumber"].ToString(), reader["AccountID"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error fetching bank accounts: " + ex.Message;
                }
            }
        }

        private void PopulateLinkedAccounts()
        {
            string userId = Session["UserID"]?.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                lblMessage.Text = "User is not logged in.";
                return;
            }

            string query = "SELECT LinkID, FlowAccountNumber FROM LinkedAccounts WHERE UserID = @UserID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    ddlLinkedAccounts.Items.Clear();
                    ddlLinkedAccounts.Items.Add(new ListItem("Select Linked Account", ""));
                    while (reader.Read())
                    {
                        ddlLinkedAccounts.Items.Add(new ListItem(reader["FlowAccountNumber"].ToString(), reader["LinkID"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error fetching linked accounts: " + ex.Message;
                }
            }
        }

        protected void btnMakePayment_Click(object sender, EventArgs e)
        {
            string userId = Session["UserID"]?.ToString();
            string bankAccountID = ddlBankAccounts.SelectedValue;
            string linkedAccountID = ddlLinkedAccounts.SelectedValue;
            decimal amount;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(bankAccountID) || string.IsNullOrEmpty(linkedAccountID) ||
                !decimal.TryParse(txtAmount.Text.Trim(), out amount) || amount <= 0)
            {
                lblMessage.Text = "Please fill out all fields correctly.";
                return;
            }

            try
            {
                // Validate sufficient funds
                if (!HasSufficientFunds(bankAccountID, amount))
                {
                    lblMessage.Text = "Insufficient funds in the selected bank account.";
                    return;
                }

                // Call the FlowService to initiate payment
                bool paymentSuccess = flowService.MakePayment(linkedAccountID, amount);

                if (paymentSuccess)
                {
                    // Log the transaction in the database
                    LogTransaction(userId, bankAccountID, amount, "Debit", $"Payment to linked account {linkedAccountID}");

                    // Update the bank account balance
                    UpdateAccountBalance(bankAccountID, -amount);

                    lblMessage.Text = "Payment successfully completed!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblMessage.Text = "Payment failed. Please try again.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred during payment: " + ex.Message;
            }
        }

        private bool HasSufficientFunds(string bankAccountID, decimal amount)
        {
            string query = "SELECT Balance FROM BankAccounts WHERE AccountID = @AccountID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AccountID", bankAccountID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        decimal balance = Convert.ToDecimal(result);
                        return balance >= amount;
                    }
                }
                catch
                {
                    // Log or handle exception
                }
            }

            return false;
        }

        private void LogTransaction(string userId, string bankAccountID, decimal amount, string transactionType, string narrative)
        {
            string query = "INSERT INTO BankTransactions (UserID, BankAccountID, Amount, TransactionType, Date, Narrative) VALUES (@UserID, @BankAccountID, @Amount, @TransactionType, @Date, @Narrative)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@BankAccountID", bankAccountID);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@TransactionType", transactionType);
                command.Parameters.AddWithValue("@Date", DateTime.Now);
                command.Parameters.AddWithValue("@Narrative", narrative);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    // Handle logging failure
                }
            }
        }

        private void UpdateAccountBalance(string bankAccountID, decimal amount)
        {
            string query = "UPDATE BankAccounts SET Balance = Balance + @Amount WHERE AccountID = @AccountID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@AccountID", bankAccountID);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    // Handle balance update failure
                }
            }
        }
    }
}
