using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class PAdetails
    {
        public decimal id { get; set; }
        public float amount { get; set; }
        public float sold_amount { get; set; }
        public float amount_paid { get; set; }
        public DateTime begin_date { get; set; }
        public DateTime due_date { get; set; }
        public string status { get; set; }
        public DateTime creation_date { get; set; }
    }
}