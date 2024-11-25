using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SagicorAccount.Class
{
    public class BankTransaction
    {
        

        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string AccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Status { get; set; }
    }

}