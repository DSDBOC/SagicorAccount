using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SagicorAccount.Class
{
    public class PaymentRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string AccountId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }

}