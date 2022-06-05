using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class AddPhone
    {
        [Required]
        [Display(Name = "Main")]
        public bool main { get; set; }
        [Display(Name = "Type")]
        [Required]
        public string type { get; set; }
        [Display(Name = "Phone number")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string phone_number { get; set; }
        [Display(Name = "Source")]
        [Required]
        public string source { get; set; }
    }
}