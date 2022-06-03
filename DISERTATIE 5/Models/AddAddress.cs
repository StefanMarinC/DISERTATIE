using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class AddAddress
    {
        [Required]
        [Display(Name ="Main")]
        public bool main { get; set; }
        [Display(Name = "Type")]
        [Required]
        public string type { get; set; }
        [Display(Name = "Country")]
        [Required]
        public string country { get; set; }
        [Display(Name = "City")]
        [Required]
        public string city { get; set; }
        [Display(Name = "Disctrict")]
        [Required]
        public string district { get; set; }
        [Display(Name = "Street")]
        [Required]
        public string street { get; set; }
        [Display(Name = "Street number")]
        [Required]
        public string street_number { get; set; }
        [Display(Name = "Building")]
        public string building { get; set; }
        [Display(Name = "Stair")]
        public string stair { get; set; }
        [Display(Name = "Floor")]
        public string floor { get; set; }
        [Display(Name = "Apartment")]
        public string apartment { get; set; }
        [Display(Name = "Source")]
        [Required]
        public string source { get; set; }
    }
}