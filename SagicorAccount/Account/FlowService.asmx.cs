using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

namespace SagicorAccount.Account
{
    /// <summary>
    /// Summary description for FlowService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class FlowService : System.Web.Services.WebService
    {
        // Mock database for FLOW accounts
        private static Dictionary<string, string> flowAccounts = new Dictionary<string, string>
        {
            { "FLOW123", "John Doe" },
            { "FLOW456", "Jane Smith" },
            { "FLOW789", "Michael Johnson" }
        };

        /// <summary>
        /// A method to check if a FLOW account exists.
        /// </summary>
        /// <param name="accountNumber">The FLOW account number to verify.</param>
        /// <returns>A string message indicating the result.</returns>
        [WebMethod]
        public string VerifyFlowAccount(string accountNumber)
        {
            // Check if the account number exists in the mock FLOW accounts dictionary
            if (flowAccounts.ContainsKey(accountNumber))
            {
                return "Account exists: " + flowAccounts[accountNumber];
            }
            else
            {
                return "Account not found.";
            }
        }

        /// <summary>
        /// A method to simulate a payment to a FLOW account.
        /// </summary>
        /// <param name="accountNumber">The FLOW account number to pay to.</param>
        /// <param name="amount">The amount to be paid.</param>
        /// <returns>A string message indicating the result of the payment.</returns>
        [WebMethod]
        public string MakePayment(string accountNumber, decimal amount)
        {
            // Check if the account exists in the mock database
            if (flowAccounts.ContainsKey(accountNumber))
            {
                // Simulate payment processing (for mock purposes)
                return $"Payment of {amount:C} to account {accountNumber} by {flowAccounts[accountNumber]} was successful.";
            }
            else
            {
                return "Account not found.";
            }
        }
    }
}
