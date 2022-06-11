using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DISERTATIE_5.Models
{
    public class AddAsset
    {
        [Display(Name = "Asset category")]
        [Required]
        public string asset_category { get; set; }
        [Display(Name = "Asset type")]
        [Required]
        public string asset_type { get; set; }
        [Display(Name = "Asset subtype")]
        [Required]
        public string asset_subtype { get; set; }
        [Display(Name = "Asset status")]
        public string asset_status { get; set; }
        [Display(Name = "Building status")]
        public string building_status { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "Construction year")]
        public string construction_year { get; set; }
        [Display(Name = "Thermal rehabilitation")]
        public string thermal_rehabilitation { get; set; }
        [Display(Name = "Year of last thermal rehabilitation")]
        public string year_of_last_rehabilitation { get; set; }
        [Display(Name = "Company status")]
        public string company_status { get; set; }
        [Display(Name = "Colour")]
        public string colour { get; set; }
        [Display(Name = "Value")]
        public string value { get; set; }
        [Display(Name = "License plate")]
        public string license_plate { get; set; }
        [Display(Name = "Manufacturing year")]
        public string manufacturing_year { get; set; }
        [Display(Name = "Model")]
        public string model_type { get; set; }
        [Display(Name = "Cilindrical capacity")]
        public string cilindrical_capacity { get; set; }
    }
}