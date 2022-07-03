using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class EditLegalFile
    {
        public int legalFileID { get; set; }
        [Display(Name = "File number")]
        [Required]
        public string fileNumber { get; set; }
        [Display(Name = "Status")]
        [Required]
        public string status { get; set; }
        [Display(Name = "Start date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime startDate { get; set; }
        [Display(Name = "Court")]
        [Required]
        public string court { get; set; }
        [Display(Name = "Bailiff")]
        [Required]
        public string bailiff { get; set; }
        [Display(Name = "Lawyer")]
        [Required]
        public string lawyer { get; set; }
        [Display(Name = "Notary")]
        [Required]
        public string notary { get; set; }
    }
}