using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DISERTATIE_5.Models
{
    public class Emails
    {
        public string subject { get; set; }
        public string email_to { get; set; }
        public DateTime sent_time { get; set; }
        public decimal case_id { get; set; }
        public decimal email_id { get; set; }
        public string status { get; set; }
    }
}