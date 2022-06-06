using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class EditEmail
    {
        public decimal email_id { get; set; }
        [Required]
        [Display(Name = "Main")]
        public bool main { get; set; }
        [Display(Name = "Type")]
        [Required]
        public string type { get; set; }
        [Display(Name = "Phone number")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string email_address { get; set; }
        [Display(Name = "Source")]
        [Required]
        public string source { get; set; }
    }
}