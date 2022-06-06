using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class SubscriberContact
    {
        public long subscriber_id { get; set; }
        public string contact_type { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string source_type { get; set; }
        public decimal subs_contact_id { get; set; }
    }
}