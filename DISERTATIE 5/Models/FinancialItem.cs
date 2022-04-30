using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class FinancialItem
    {
        public decimal case_id { get; set; }
        public string item_name { get; set; }
        public DateTime item_date { get; set; }
        public DateTime booking_date { get; set; }
        public float amount { get; set; }
        public string amount_currency { get; set; }
        public float amount_not_booked { get; set; }
        public string amount_not_booked_currency { get; set; }
    }
}