using System;
using System.Data.SqlClient;
using System.Web.Services;
using System.Configuration;  // Make sure this is included

namespace SagicorAccount.Account
{
    /// <summary>
    /// PaymentService to handle actual payments between accounts.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class PaymentService : System.Web.Services.WebService
    {
        // Fetch the actual connection string from Web.config
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        [WebMethod]
        public string ProcessPayment(string bankAccountNumber, string flowAccountNumber, decimal amount)
        {
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
                        WHERE AccountNumber = @BankAccountNumber AND Balance >= @Amount";

                    using (SqlCommand cmd = new SqlCommand(deductBankQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@BankAccountNumber", bankAccountNumber);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception("Insufficient funds or account not found.");
                        }
                    }

                    // Add amount to FlowAccount
                    string creditFlowQuery = @"
                        UPDATE FlowAccounts
                        SET Balance = Balance + @Amount
                        WHERE FlowAccountNumber = @FlowAccountNumber";

                    using (SqlCommand cmd = new SqlCommand(creditFlowQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@FlowAccountNumber", flowAccountNumber);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception("FLOW account not found.");
                        }
                    }

                    // Commit transaction
                    transaction.Commit();
                    return $"Payment of ${amount:F2} from {bankAccountNumber} to FLOW account {flowAccountNumber} was successful.";
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
