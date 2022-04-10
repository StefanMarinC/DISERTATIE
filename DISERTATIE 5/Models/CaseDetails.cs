using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class CaseDetails
    {
        public long case_id { get; set; }
        public string zone { get; set; }
        public string client_name { get; set; }
        public float balance { get; set; }
        public string pa_status { get; set; }
        public decimal pa_made { get; set; }
        public decimal pa_broken { get; set; }
        public decimal pa_kept { get; set; }
        public decimal pa_cancelled { get; set; }
    }
}