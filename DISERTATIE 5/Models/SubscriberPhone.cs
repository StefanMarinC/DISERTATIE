using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class SubscriberPhone
    {
        public long subscriber_id { get; set; }
        public string phone_type { get; set; }
        public string main_phone { get; set; }
        public string phone_number { get; set; }
        public string source_type { get; set; }
        public string created_by { get; set; }
        public DateTime creation_date { get; set; }
    }
}