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
    
    public partial class ADDRESS_TYPES
    {
        public ADDRESS_TYPES()
        {
            this.SUBSCRIBER_ADDRESSES = new HashSet<SUBSCRIBER_ADDRESSES>();
        }
    
        public long ADDRESS_TYPE_ID { get; set; }
        public string NAME { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATION_DATE { get; set; }
        public string LAST_UPDATED_BY { get; set; }
        public Nullable<System.DateTime> LAST_UPDATE_DATE { get; set; }
    
        public virtual ICollection<SUBSCRIBER_ADDRESSES> SUBSCRIBER_ADDRESSES { get; set; }
    }
}
