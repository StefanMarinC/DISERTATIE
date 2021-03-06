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
    
    public partial class SUBSCRIBERS
    {
        public SUBSCRIBERS()
        {
            this.ACC_SUBS_INT = new HashSet<ACC_SUBS_INT>();
            this.SUBSCRIBER_ADDRESSES = new HashSet<SUBSCRIBER_ADDRESSES>();
        }
    
        public long SUBSCRIBER_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public Nullable<System.DateTime> BIRTH_DATE { get; set; }
        public string SSN { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATION_DATE { get; set; }
        public string LAST_UPDATED_BY { get; set; }
        public Nullable<System.DateTime> LAST_UPDATE_DATE { get; set; }
        public string CUSTOMER_TYPE { get; set; }
        public string BIRTH_PLACE { get; set; }
        public string NATIONALITY { get; set; }
        public Nullable<decimal> BATCH_NUMBER { get; set; }
        public string COMPANY_NAME { get; set; }
        public string BIRTH_NAME { get; set; }
        public string MOTHER_NAME { get; set; }
        public string GENDER { get; set; }
    
        public virtual ICollection<ACC_SUBS_INT> ACC_SUBS_INT { get; set; }
        public virtual ICollection<SUBSCRIBER_ADDRESSES> SUBSCRIBER_ADDRESSES { get; set; }
    }
}
