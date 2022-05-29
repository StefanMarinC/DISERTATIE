using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class EditSubscriberCase
    {
        [Display(Name="Main")]
        public Boolean main { get; set; }
        [Display(Name = "Customer type")]
        public string customer_type { get; set; }
        [Display(Name = "SSN")]
        public string ssn { get; set; }
        [Display(Name = "First name")]
        public string first_name { get; set; }
        [Display(Name = "Last name")]
        public string last_name { get; set; }
        [Display(Name = "Gender")]
        public string gender { get; set; }
        [Display(Name = "Birth date")]
        public DateTime birth_date { get; set; }
        [Display(Name = "Birth place")]
        public string birth_place { get; set; }
        [Display(Name = "Subscriber type")]
        public string subscriber_type { get; set; }
    }
}