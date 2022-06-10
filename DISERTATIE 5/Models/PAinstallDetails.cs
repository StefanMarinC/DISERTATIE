using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class PAinstallDetails
    {
        public decimal number { get; set; }
        public DateTime due_date { get; set; }
        public float sold { get; set; }
        public float paid { get; set; }
        public string status { get; set; }
    }
}