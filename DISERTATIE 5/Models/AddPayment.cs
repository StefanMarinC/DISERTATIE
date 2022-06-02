using DISERTATIE_5.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class AddPayment
    {
        [Display(Name ="Payment date")]
        [Required]
        [DataType(DataType.Date)]
        [RangeDate(minDate:"01/01/2000")]
        public DateTime payment_date{ get; set; }
        [Display(Name = "Booking date")]
        [Required]
        [DataType(DataType.Date)]
        [RangeDate(minDate: "01/01/2000")]
        public DateTime booking_date{ get; set; }
        [Display(Name = "Amount")]
        [Required]
        public float amount{ get; set; }
        [Display(Name = "Currency")]
        [Required]
        public string currency { get; set; }
    }
}