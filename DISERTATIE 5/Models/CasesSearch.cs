using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class CasesSearch
    {
        public decimal unique_id { get; set; }
        public string client_name { get; set; }
        public string zone { get; set; }
        public string zone_name { get; set; }
        public string owner { get; set; }
        public decimal owner_id { get; set; }
        public long case_id { get; set; }
        public long account_id { get; set; }
        public string customer_id { get; set; }
        public string ssn { get; set; }
        public string name { get; set; }
        public string subscriber_type { get; set; }
        public string contract_number { get; set; }
        public long subscriber_id { get; set; }

    }
}