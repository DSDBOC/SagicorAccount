using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SagicorAccount.Class
{
    public class ExternalAccountTransaction
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string ExternalAccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionStatus { get; set; } // E.g., "Completed", "Failed"
    }

}