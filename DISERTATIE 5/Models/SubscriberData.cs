using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class SubscriberData
    {
        public long case_id { get; set; }
        public long subscriber_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string subscriber_type { get; set; }
        public decimal main { get; set; }
        public string SSN { get; set; }
    }
}