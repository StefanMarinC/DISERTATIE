using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class AddSubscriberCase
    {
        [Display(Name = "Main")]
        public Boolean main { get; set; }
        [Display(Name = "Debtor type")]
        [Required(ErrorMessage = "The debtor type is required")]
        public string debtor_type { get; set; }
        [Display(Name = "SSN")]
        [Required(ErrorMessage = "The SSN is required")]
        public string ssn { get; set; }
        [Display(Name = "First name / company name")]
        [Required(ErrorMessage = "The name is required")]
        public string first_name { get; set; }
        [Display(Name = "Last name")]
        public string last_name { get; set; }
        [Display(Name = "Gender")]
        public string gender { get; set; }
        [Display(Name = "Birth date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime birth_date { get; set; }
        [Display(Name = "Birth place")]
        public string birth_place { get; set; }
        [Display(Name = "Type")]
        [Required(ErrorMessage = "The subscriber type is required")]
        public string subscriber_type { get; set; }
    }
}