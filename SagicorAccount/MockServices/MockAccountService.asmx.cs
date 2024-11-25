using System;
using System.Collections.Generic;
using System.Web.Services;

namespace SagicorAccount.MockServices
{
    /// <summary>
    /// Mock service to simulate external account verification for FLOW and Sagicor accounts.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class MockAccountService : WebService
    {
        // Mock data to simulate accounts
        private static readonly Dictionary<string, string> MockAccounts = new Dictionary<string, string>
        {
            { "FLOW12345", "FLOW" },
            { "FLOW67890", "FLOW" },
            { "SAGICOR11111", "SAGICOR" },
            { "SAGICOR22222", "SAGICOR" }
        };

        /// <summary>
        /// Verifies if an account exists and matches the given type.
        /// </summary>
        /// <param name="accountNumber">The account number to verify.</param>
        /// <param name="accountType">The type of the account (e.g., FLOW or SAGICOR).</param>
        /// <returns>True if the account exists and matches the type, otherwise false.</returns>
        [WebMethod]
        public bool VerifyAccount(string accountNumber, string accountType)
        {
            if (string.IsNullOrEmpty(accountNumber) || string.IsNullOrEmpty(accountType))
                return false;

            return MockAccounts.TryGetValue(accountNumber, out var type) &&
                   type.Equals(accountType, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Retrieves account details if the account exists.
        /// </summary>
        /// <param name="accountNumber">The account number to retrieve details for.</param>
        /// <returns>A string containing account details or a "not found" message.</returns>
        [WebMethod]
        public string GetAccountDetails(string accountNumber)
        {
            if (MockAccounts.TryGetValue(accountNumber, out var accountType))
            {
                return $"Account Number: {accountNumber}, Account Type: {accountType}";
            }
            return "Account not found.";
        }

        /// <summary>
        /// Links a new account to the user's profile.
        /// </summary>
        /// <param name="userID">The user ID attempting to link the account.</param>
        /// <param name="accountNumber">The account number to link.</param>
        /// <param name="accountType">The type of the account (e.g., FLOW or SAGICOR).</param>
        /// <returns>A success or error message.</returns>
        [WebMethod]
        public string LinkAccount(string userID, string accountNumber, string accountType)
        {
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(accountNumber) || string.IsNullOrEmpty(accountType))
                return "Invalid input. All fields are required.";

            if (!VerifyAccount(accountNumber, accountType))
                return "Account verification failed. Please check the account number and type.";

            // Simulating a successful link operation
            return $"Success! Account {accountNumber} ({accountType}) has been linked to user {userID}.";
        }

        /// <summary>
        /// Unlinks an account from the user's profile.
        /// </summary>
        /// <param name="userID">The user ID attempting to unlink the account.</param>
        /// <param name="accountNumber">The account number to unlink.</param>
        /// <returns>A success or error message.</returns>
        [WebMethod]
        public string UnlinkAccount(string userID, string accountNumber)
        {
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(accountNumber))
                return "Invalid input. Both user ID and account number are required.";

            // Simulating a successful unlink operation
            return $"Success! Account {accountNumber} has been unlinked from user {userID}.";
        }
    }
}
