using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SagicorAccount.Class
{
    public class PaymentStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; } // E.g., "Pending", "Completed", etc.
        public string Description { get; set; }
    }

}