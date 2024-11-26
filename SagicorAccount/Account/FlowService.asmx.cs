using System;
using System.Web.Services;

namespace SagicorAccount.Account
{
    /// <summary>
    /// FlowService to simulate account verification and return account details.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class FlowService : System.Web.Services.WebService
    {
        // Mock class to simulate account details returned from FLOW/SAGICOR LIFE system
        public class AccountDetails
        {
            public string Name { get; set; }
            public string AccountNumber { get; set; }
            public string AccountType { get; set; }
            public decimal Balance { get; set; }
        }

        // Mock method to simulate account existence check
        [WebMethod]
        public AccountDetails VerifyAccountExists(string flowAccountNumber)
        {
            // Simulate an existing account list
            var existingAccounts = new[]
            {
                new { Name = "John Doe", flowAccountNumber = "12345", AccountType = "Savings", Balance = 1000.50m },
                new { Name = "Mia Pope", flowAccountNumber = "67890", AccountType = "Checking", Balance = 1500.00m },
                new { Name = "Alice Johnson", flowAccountNumber = "11223", AccountType = "Savings", Balance = 500.00m },
                new { Name = "Bob Brown", flowAccountNumber = "44556", AccountType = "Checking", Balance = 2000.75m }
            };

            // Check if the FLOW account number exists in the mock list
            var account = Array.Find(existingAccounts, a => a.flowAccountNumber == flowAccountNumber);

            if (account != null)
            {
                // If the account exists, return account details
                return new AccountDetails
                {
                    Name = account.Name,
                    AccountNumber = account.flowAccountNumber,
                    AccountType = account.AccountType,
                    Balance = account.Balance
                };
            }
            else
            {
                // If the account does not exist, return null
                return null;
            }
        }
    }
}
