using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class AddDebt
    {
        [Display(Name = "Item date")]
        [Required(ErrorMessage = "The item date of debt is required!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime item_date { get; set; }

        [Display(Name = "Amount")]
        [Required(ErrorMessage = "The amount of debt is required!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public float amount { get; set; }

        [Display(Name = "Currency")]
        [Required(ErrorMessage = "The currency of debt is required!")]
        public string currency { get; set; }
    }
}