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
        [Required(ErrorMessage ="The date of payment is required!")]
        [DataType(DataType.Date)]
        [CustomDateRange(ErrorMessage = "The payment date must be between 10 years ago and today!")]
        public DateTime payment_date{ get; set; }
        [Display(Name = "Booking date")]
        [Required(ErrorMessage = "The booking date of payment is required!")]
        [DataType(DataType.Date)]
        [CustomDateRange(ErrorMessage ="The booking date must be between 10 years ago and today!")]
        public DateTime booking_date{ get; set; }
        [Display(Name = "Amount")]
        [Required(ErrorMessage = "The amount of payment is required!")]
        public float amount{ get; set; }
        [Display(Name = "Currency")]
        [Required(ErrorMessage = "The currency of payment is required!")]
        public string currency { get; set; }
    }
}