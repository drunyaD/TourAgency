using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TourAgency.WEB.Validation
{
    public class DateTimeRangeAttribute:ValidationAttribute
    {
        public DateTime MinTime { get; private set; }
        public string MaxTimePropertyName { get; private set; }
        public DateTimeRangeAttribute(string maxTimePropertyName)
        {
            MaxTimePropertyName = maxTimePropertyName;
            MinTime = DateTime.Now;
        }   

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var containerType = validationContext.ObjectInstance.GetType();
            var maxTimeField = containerType.GetProperty(MaxTimePropertyName);

            if (maxTimeField != null)
            {
                var maxTimeObj = maxTimeField.GetValue(validationContext.ObjectInstance, null);
                DateTime maxTime;
                DateTime valueTime;
                if (maxTimeObj is DateTime)
                {
                    maxTime = (DateTime)maxTimeObj;
                }
                else
                {
                    return new ValidationResult("maxTime must be DateTime", new[] { validationContext.MemberName, MaxTimePropertyName });
                }

                if (value is DateTime)
                {
                    valueTime = (DateTime)value;
                }
                else
                {
                    return new ValidationResult("Value type must be DateTime", new[] { validationContext.MemberName });
                }

                if (MinTime < valueTime && valueTime < maxTime)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Incorrect time gap haha ou lose", new[] { validationContext.MemberName, MaxTimePropertyName });
                }

            }
            else
            {
                return new ValidationResult("cant find maxTime", new[] { validationContext.MemberName, MaxTimePropertyName });
            }
        }
    }
}