using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SagicorAccount.Class
{
    public class WebServiceLog
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string RequestDetails { get; set; }
        public string ResponseDetails { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; } // E.g., "Success", "Failed"
    }

}