using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class SubscriberEmployer
    {
        public long subscriber_id { get; set; }
        public string employer_name { get; set; }
        public string position { get; set; }
        public string main_employer { get; set; }
        public string source_type { get; set; }
    }
}