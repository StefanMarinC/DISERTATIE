using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class AddContact
    {
        [Required]
        [Display(Name ="Type")]
        public string type { get; set; }
        [Required]
        [Display(Name = "First name")]
        public string first_name {get;set;}
        [Display(Name = "Last name")]
        public string last_name { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "Postal code")]
        public string postal_code { get; set; }
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required]
        [Display(Name = "Source")]
        public string source { get; set; }
    }
}