using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Configuration;  // Add this namespace to access ConfigurationManager

namespace SagicorAccount.Account
{
    /// <summary>
    /// FlowService to simulate account verification, payment processing, and return account details.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class FlowService : System.Web.Services.WebService
    {
        // Retrieve the connection string from Web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        // Mock class to simulate account details returned from FLOW/SAGICOR LIFE system
        public class AccountDetails
        {
            public string Name { get; set; } // Added property for account holder's name
            public string AccountNumber { get; set; }
            public string AccountType { get; set; }
            public decimal Balance { get; set; }
        }

        // Mock method to verify account on FLOW/SAGICOR LIFE and return account details
        [WebMethod]
        public AccountDetails VerifyAccountExists(string accountNumber)
        {
            // Simulate account validation logic
            var existingAccounts = new[]
            {
                new { Name = "John Doe", AccountNumber = "12345", AccountType = "Savings", Balance = 1000.50m },
                new { Name = "Jane Smith", AccountNumber = "67890", AccountType = "Checking", Balance = 1500.00m },
                new { Name = "Alice Johnson", AccountNumber = "11223", AccountType = "Savings", Balance = 500.00m },
                new { Name = "Bob Brown", AccountNumber = "44556", AccountType = "Checking", Balance = 2000.75m }
            };

            var account = Array.Find(existingAccounts, a => a.AccountNumber == accountNumber);

            if (account != null)
            {
                return new AccountDetails
                {
                    Name = account.Name,
                    AccountNumber = account.AccountNumber,
                    AccountType = account.AccountType,
                    Balance = account.Balance
                };
            }
            else
            {
                return null; // Return null if account does not exist
            }
        }

        // Submit a payment from the user's bank account to a FLOW account
        [WebMethod]
        public string SubmitPayment(string bankAccountNumber, string flowAccountNumber, decimal amount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Deduct from BankAccount
                    string deductQuery = @"
                        UPDATE BankAccounts
                        SET Balance = Balance - @Amount, 
                            LastTransactionNarrative = @Narrative
                        WHERE AccountNumber = @BankAccountNumber AND Balance >= @Amount";

                    using (SqlCommand cmd = new SqlCommand(deductQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Narrative", $"Payment to FLOW Account {flowAccountNumber}");
                        cmd.Parameters.AddWithValue("@BankAccountNumber", bankAccountNumber);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0) throw new Exception("Insufficient funds or account not found.");
                    }

                    // Credit to FlowAccount
                    string creditQuery = @"
                        UPDATE FlowAccounts
                        SET Balance = Balance + @Amount, 
                            LastTransactionNarrative = @Narrative
                        WHERE FlowAccountNumber = @FlowAccountNumber";

                    using (SqlCommand cmd = new SqlCommand(creditQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Narrative", $"Payment from Bank Account {bankAccountNumber}");
                        cmd.Parameters.AddWithValue("@FlowAccountNumber", flowAccountNumber);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0) throw new Exception("FLOW account not found.");
                    }

                    // Log Transactions
                    string logTransactionQuery = @"
                        INSERT INTO Transactions (AccountNumber, Amount, Narrative)
                        VALUES (@AccountNumber, @Amount, @Narrative)";

                    using (SqlCommand cmd = new SqlCommand(logTransactionQuery, conn, transaction))
                    {
                        // Log deduction
                        cmd.Parameters.AddWithValue("@AccountNumber", bankAccountNumber);
                        cmd.Parameters.AddWithValue("@Amount", -amount);
                        cmd.Parameters.AddWithValue("@Narrative", $"Payment to FLOW Account {flowAccountNumber}");
                        cmd.ExecuteNonQuery();

                        // Log credit
                        cmd.Parameters["@AccountNumber"].Value = flowAccountNumber;
                        cmd.Parameters["@Amount"].Value = amount;
                        cmd.Parameters["@Narrative"].Value = $"Payment from Bank Account {bankAccountNumber}";
                        cmd.ExecuteNonQuery();
                    }

                    // Commit transaction
                    transaction.Commit();
                    return $"Payment of ${amount:F2} successfully processed.";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return $"Payment failed: {ex.Message}";
                }
            }
        }
    }
}
