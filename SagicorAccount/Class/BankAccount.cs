using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SagicorAccount.Class
{
    public class BankAccount
    {
        [Key] // Define AccountID as the primary key
        public string AccountID { get; set; }
        public string UserID { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; internal set; }

        public void GenerateAccountData()
        {
            var random = new Random();

            // Generate random GUID for AccountID
            AccountID = Guid.NewGuid().ToString();

            // Generate random 9-digit AccountNumber
            AccountNumber = random.Next(100000000, 999999999).ToString();

            
            UserID = "user-id-here";  
            AccountType = "Savings";   
            Balance = 1000.00m;        
        }
    }

}