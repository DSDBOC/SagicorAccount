using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Configuration;

namespace SagicorAccount.Account
{
    /// <summary>
    /// Summary description for AccountService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AccountService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// Checks if an account exists based on the provided account number.
        /// </summary>
        /// <param name="accountNumber">The account number to check.</param>
        /// <returns>Returns true if the account exists, otherwise false.</returns>
        [WebMethod]
        public bool CheckIfAccountExists(string accountNumber)
        {
            try
            {
                // Use the connection string from web.config
                string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

                // Define the SQL query to check if the account exists
                string query = "SELECT COUNT(*) FROM LinkedAccounts WHERE FlowAccountNumber = @FlowAccountNumber";

                // Initialize a connection to the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Create the SQL command and add the necessary parameter
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FlowAccountNumber", accountNumber);

                        // Open the database connection
                        conn.Open();

                        // Execute the command and return true if the account exists (count > 0)
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional) or throw an application exception
                // Handle exception, perhaps log the error to a file or monitoring system
                throw new ApplicationException("Error checking account existence", ex);
            }
        }
    }
}
