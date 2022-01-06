using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class AddUser
    {
        [Display(Name = "Username")]
        [DataType(DataType.Text)]
        [Required]
        [MinLength(10, ErrorMessage = "Username must have at least 10 characters")]
        [MaxLength(50, ErrorMessage = "Username must have maximum 50 characters")]
        public string USERNAME { get; set; }

        [Display(Name = "Full name")]
        [DataType(DataType.Text)]
        [Required]
        [MinLength(6, ErrorMessage = "Name must have at list 6 characters")]
        [MaxLength(100, ErrorMessage = "Name must have maximum 100 characters")]
        [RegularExpression(@"^([A-Z]{1}[a-z]{1,}\s)([A-Z]{1}[a-z]{1,}\s?)+$", ErrorMessage = "Please enter first name and last name separated by space")]
        public string FULL_NAME { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must have at least 6 characters")]
        [MaxLength(50, ErrorMessage = "Password must have maximum 50 characters")]
        [Required]
        public string SEC_PASSWORD { get; set; }

        [Display(Name = "Admin")]
        [Required]
        public Nullable<bool> ISADMIN { get; set; }

        [Display(Name = "Start activ date")]
        [DataType(DataType.Date)]
        [Required]
        public Nullable<System.DateTime> START_ACTIVE_DATE { get; set; }

        [Display(Name = "End activ date")]
        [DataType(DataType.Date)]
        [Required]
        public Nullable<System.DateTime> END_ACTIVE_DATE { get; set; }
    }
}