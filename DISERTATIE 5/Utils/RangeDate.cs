using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Utils
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RangeDate : ValidationAttribute
    {
        DateTime MinDate, MaxDate;
        public RangeDate (string minDate)
        {
            MinDate = DateTime.Parse(minDate);
            MaxDate = DateTime.Now;
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = $"Date must be between {minDate} and today";
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var d = (DateTime)value;
            if (d < MinDate || d > MaxDate)
                return new ValidationResult(ErrorMessage);
            else
                return ValidationResult.Success;
        }
    }
}