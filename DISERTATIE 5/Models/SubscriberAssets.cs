using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class SubscriberAssets
    {
        public decimal asset_id { get; set; }
        public decimal subscriber_id { get; set; }
        public string details { get; set; }
        public string description { get; set; }
    }
}