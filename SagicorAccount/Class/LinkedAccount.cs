using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SagicorAccount.Class
{
    public class LinkedAccount
    {
        public int Id { get; set; } // Assuming there's a primary key
        public string UserId { get; set; }
        public decimal Balance { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string AccountType { get; set; }
        public DateTime DateLinked { get; set; }

        // You can add more properties based on your database schema.
    }

}