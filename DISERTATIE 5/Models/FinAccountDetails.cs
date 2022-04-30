using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class FinAccountDetails
    {
        public decimal case_id { get; set; }
        public string client_name { get; set; }
        public string zone { get; set; }
        public string customer_id { get; set; }
        public string account_currency { get; set; }
        public DateTime account_balance_date { get; set; }
        public float amount_paid { get; set; }
        public string amount_paid_currency { get; set; }
        public float amount_to_pay { get; set; }
        public string amount_to_pay_currency { get; set; }

    }
}