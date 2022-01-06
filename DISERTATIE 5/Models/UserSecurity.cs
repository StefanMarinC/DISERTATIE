using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class UserSecurity
    {
        public decimal SEC_USER_ID { get; set; }
        public string USERNAME { get; set; }
        public string FULL_NAME { get; set; }
        public Nullable<decimal> ISADMIN { get; set; }
        public Nullable<decimal> ACTIVE { get; set; }
        public Nullable<decimal> BLOCKED { get; set; }
    }
}