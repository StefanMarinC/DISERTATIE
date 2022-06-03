using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class FinAllocationItem
    {
        private FinAllocationItem finAllocationItem;

        public FinAllocationItem()
        {
        }

        public FinAllocationItem(FinAllocationItem finAllocationItem)
        {
            this.finAllocationItem = finAllocationItem;
        }

        public float amount { get; set; }
        public string amount_currency { get; set; }
        public float amount_fnc { get; set; }
        public string amount_fnc_currency { get; set; }
        public float amount_allocated { get; set; }
        public string amount_allocated_currency { get; set; }
        public float amount_allocated_fnc { get; set; }
        public string amount_allocated_fnc_currency { get; set; }
        public float amount_over { get; set; }
        public string amount_over_currency { get; set; }
        public float amount_over_fnc { get; set; }
        public string amount_over_fnc_currency { get; set; }
    }
}