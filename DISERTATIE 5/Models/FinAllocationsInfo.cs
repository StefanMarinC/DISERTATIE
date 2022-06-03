using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class FinAllocationsInfo
    {
        public string item_type { get; set; }
        public float amount_p { get; set; }
        public string amount_currency_p { get; set; }
        public float allocated_amount_p { get; set; }
        public string allocated_amount_currency_p { get; set; }
        public float allocated_amount_m { get; set; }
        public string allocated_amount_currency_m { get; set; }
        public float allocated_amount_fnc { get; set; }
        public string allocated_amount_fnc_currency { get; set; }
    }
}