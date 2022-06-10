using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class AddPA
    {
        [Required]
        [Display(Name ="Person")]
        public string person { get; set; }
        [Required]
        [Display(Name = "Amount")]
        public float amount { get; set; }
        [Required]
        [Display(Name = "Periods")]
        public int periods { get; set; }
        [Required]
        [Display(Name = "Installment type")]
        public string installment_type { get; set; }
        [Required]
        [Display(Name = "Start date")]
        public DateTime start_date { get; set; }
    }
}