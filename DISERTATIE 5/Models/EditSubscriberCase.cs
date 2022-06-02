using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class EditSubscriberCase
    {
        [Display(Name = "Main")]
        [Required]
        public Boolean main { get; set; }
        [Display(Name = "Debtor type")]
        [Required]
        public string customer_type { get; set; }
        [Display(Name = "SSN")]
        [Required]
        public string ssn { get; set; }
        [Display(Name = "First name / company name")]
        [Required]
        public string first_name { get; set; }
        [Display(Name = "Last name")]
        public string last_name { get; set; }
        [Display(Name = "Gender")]
        [Required]
        public string gender { get; set; }
        [Display(Name = "Birth date / date of establishment")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime birth_date { get; set; }
        [Display(Name = "Birth place")]
        public string birth_place { get; set; }
        [Display(Name = "Subscriber type")]
        [Required]
        public string subscriber_type { get; set; }
    }
}