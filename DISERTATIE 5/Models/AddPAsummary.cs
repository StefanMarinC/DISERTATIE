using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class AddPAsummary
    {
        [Required]
        [Display(Name = "Total balance")]
        public float total_balance { get; set; }
        [Required]
        [Display(Name = "Person")]
        public string person { get; set; }
        [Required]
        [Display(Name = "Amount")]
        public double amount { get; set; }
        [Required]
        [Display(Name = "Amount to be paid")]
        public float amount_to_be_paid { get; set; }
        [Required]
        [Display(Name = "Periods")]
        public int periods { get; set; }
        [Required]
        [Display(Name = "Installment amount")]
        public float installment { get; set; }
        [Required]
        [Display(Name = "Installment type")]
        public string installment_type { get; set; }
        [Required]
        [Display(Name = "Start date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime start_date { get; set; }
        [Required]
        [Display(Name = "End date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime end_date { get; set; }
    }
}