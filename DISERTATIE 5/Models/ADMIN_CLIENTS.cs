//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DISERTATIE_5.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ADMIN_CLIENTS
    {
        public long CLIENT_ID { get; set; }
        public string NAME { get; set; }
        public string CUI { get; set; }
        public string ADDRESS { get; set; }
        public string EMAIL { get; set; }
        public string CONTRACT_NUMBER { get; set; }
        public Nullable<System.DateTime> CONTRACT_DATE { get; set; }
        public string MAX_PA_PERIOD { get; set; }
        public Nullable<long> BANK_ACCOUNT_ID { get; set; }
        public Nullable<decimal> PA_AFTER_DAYS { get; set; }
        public string ZIP_CODE { get; set; }
        public string CITY { get; set; }
        public string COUNTRY { get; set; }
        public string PHONE { get; set; }
        public Nullable<decimal> MAX_COUNT_OF_INST { get; set; }
        public Nullable<decimal> MIN_COUNT_OF_INST { get; set; }
    }
}
