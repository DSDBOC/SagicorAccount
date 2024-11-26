using System;
using System.Data.SqlClient;
using System.Web.Services;
using System.Configuration;
using System.Data;
using System.IO;


namespace SagicorAccount.Account
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class PaymentService : System.Web.Services.WebService
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        [WebMethod]
        public string ProcessPayment(string bankAccountNumber, string flowAccountNumber, decimal amount)
        {
            // Validate the bank account number format (numeric and correct length)
            if (!long.TryParse(bankAccountNumber, out long bankAccount))
            {
                return "Invalid bank account number format.";
            }

            // Validate the flow account number format (assuming numeric, adjust if needed)
            if (!long.TryParse(flowAccountNumber, out long flowAccount))
            {
                return "Invalid FLOW account number format.";
            }

            if (amount <= 0)
            {
                return "Payment amount must be greater than zero.";
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Deduct amount from BankAccount
                    string deductBankQuery = @"
                        UPDATE BankAccounts
                        SET Balance = Balance - @Amount
                        WHERE AccountID = @BankAccountID AND Balance >= @Amount";

                    using (SqlCommand cmd = new SqlCommand(deductBankQuery, conn, transaction))
                    {
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
                        cmd.Parameters.Add("@BankAccountID", SqlDbType.Int).Value = bankAccount; // Use the BankAccountID from the BankAccounts table
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception("Insufficient funds or bank account not found.");
                        }
                    }

                    // Add amount to FlowAccount (use Balance field in LinkedAccounts table)
                    string creditFlowQuery = @"
                        UPDATE LinkedAccounts
                        SET Balance = Balance + @Amount
                        WHERE FlowAccountNumber = @FlowAccountNumber AND BankAccountID = @BankAccountID";

                    using (SqlCommand cmd = new SqlCommand(creditFlowQuery, conn, transaction))
                    {
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
                        cmd.Parameters.Add("@FlowAccountNumber", SqlDbType.BigInt).Value = flowAccount;  // Use the flow account number
                        cmd.Parameters.Add("@BankAccountID", SqlDbType.Int).Value = bankAccount;  // Use the BankAccountID
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception("FLOW account not found or no linked bank account for this flow account.");
                        }
                    }

                    // Commit transaction
                    transaction.Commit();
                    return $"Payment of ${amount:F2} from {bankAccount} to FLOW account {flowAccount} was successful.";
                }
                catch (Exception ex)
                {
                    // Log the exception
                    LogError(ex); // You can implement a method to log the error details (e.g., Log to file or database)

                    transaction.Rollback();
                    return "Payment failed. Please try again later.";
                }
            }
        }

        private void LogError(Exception ex)
        {
            string logFilePath = @"C:\Logs\PaymentServiceErrorLog.txt";  // Specify your log file path
            string errorMessage = $"{DateTime.Now}: {ex.Message}\n{ex.StackTrace}\n";
            File.AppendAllText(logFilePath, errorMessage);
        }



        [WebMethod]
        public string SubmitPayment(string bankAccountNumber, string flowAccountNumber, decimal amount)
        {
            if (!long.TryParse(bankAccountNumber, out long parsedBankAccountNumber) ||
                !long.TryParse(flowAccountNumber, out long parsedFlowAccountNumber))
            {
                return "Payment failed: Invalid account number format.";
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Check if Bank Account exists
                    string checkBankAccountQuery = "SELECT COUNT(*) FROM BankAccounts WHERE AccountNumber = @BankAccountNumber";
                    using (SqlCommand cmd = new SqlCommand(checkBankAccountQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BankAccountNumber", parsedBankAccountNumber);
                        int bankAccountCount = (int)cmd.ExecuteScalar();
                        if (bankAccountCount == 0)
                            return "Payment failed: Bank account not found.";
                    }

                    // Check if Flow Account exists
                    string checkFlowAccountQuery = "SELECT COUNT(*) FROM FlowAccounts WHERE FlowAccountNumber = @FlowAccountNumber";
                    using (SqlCommand cmd = new SqlCommand(checkFlowAccountQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@FlowAccountNumber", parsedFlowAccountNumber);
                        int flowAccountCount = (int)cmd.ExecuteScalar();
                        if (flowAccountCount == 0)
                            return "Payment failed: FLOW account not found.";
                    }

                    // Check sufficient funds in Bank Account
                    string checkBalanceQuery = "SELECT Balance FROM BankAccounts WHERE AccountNumber = @BankAccountNumber";
                    decimal bankBalance;
                    using (SqlCommand cmd = new SqlCommand(checkBalanceQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BankAccountNumber", parsedBankAccountNumber);
                        bankBalance = (decimal)cmd.ExecuteScalar();
                    }

                    if (bankBalance < amount)
                        return "Payment failed: Insufficient funds in bank account.";

                    // Deduct from Bank Account
                    string deductQuery = @"
                UPDATE BankAccounts
                SET Balance = Balance - @Amount, 
                    LastTransactionNarrative = @Narrative
                WHERE AccountNumber = @BankAccountNumber AND Balance >= @Amount";

                    using (SqlCommand cmd = new SqlCommand(deductQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Narrative", $"Payment to FLOW Account {flowAccountNumber}");
                        cmd.Parameters.AddWithValue("@BankAccountNumber", parsedBankAccountNumber);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0)
                            throw new Exception("Payment failed: Unable to deduct funds.");
                    }

                    // Credit the Flow Account
                    string creditQuery = @"
                UPDATE FlowAccounts
                SET Balance = Balance + @Amount, 
                    LastTransactionNarrative = @Narrative
                WHERE FlowAccountNumber = @FlowAccountNumber";

                    using (SqlCommand cmd = new SqlCommand(creditQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Narrative", $"Payment from Bank Account {bankAccountNumber}");
                        cmd.Parameters.AddWithValue("@FlowAccountNumber", parsedFlowAccountNumber);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0)
                            throw new Exception("Payment failed: Unable to credit Flow account.");
                    }

                    // Log Transactions
                    string logTransactionQuery = @"
                INSERT INTO Transactions (AccountNumber, Amount, Narrative)
                VALUES (@AccountNumber, @Amount, @Narrative)";

                    using (SqlCommand cmd = new SqlCommand(logTransactionQuery, conn, transaction))
                    {
                        // Log deduction
                        cmd.Parameters.AddWithValue("@AccountNumber", parsedBankAccountNumber);
                        cmd.Parameters.AddWithValue("@Amount", -amount);
                        cmd.Parameters.AddWithValue("@Narrative", $"Payment to FLOW Account {flowAccountNumber}");
                        cmd.ExecuteNonQuery();

                        // Log credit
                        cmd.Parameters["@AccountNumber"].Value = parsedFlowAccountNumber;
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