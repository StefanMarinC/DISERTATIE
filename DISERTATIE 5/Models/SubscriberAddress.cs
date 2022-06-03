using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class SubscriberAddress
    {
        public long subscriber_id { get; set; }
        public string address_type { get; set; }
        public decimal main_address { get; set; }
        public string street { get; set; }
        public string street_number { get; set; }
        public string building { get; set; }
        public string stair { get; set; }
        public string floor { get; set; }
        public string apartment { get; set; }
        public string city { get; set; }
        public string distrinct { get; set; }
        public string country { get; set; }
        public string source_type { get; set; }
        public string created_by { get; set; }
        public DateTime creation_date { get; set; }
        public decimal address_id { get; set; }
    }
}