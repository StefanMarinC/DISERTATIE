using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class SubscriberEmail
    {
        public long subscriber_id { get; set; }
        public string email_type { get; set; }
        public short main_email { get; set; }
        public string email { get; set; }
        public string source_type { get; set; }
        public string created_by { get; set; }
        public DateTime creation_date { get; set; }
        public decimal subs_email_id { get; set; }
    }
}