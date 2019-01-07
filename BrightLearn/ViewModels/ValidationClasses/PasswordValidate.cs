using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrightLearn.ViewModels.ValidationClasses
{
    public class PasswordValidate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            string val = value.ToString();

            string Message = string.Empty;

            if (!val.Any(char.IsUpper))
            {
                Message = "Het wachtwoord moet minstens 1 hoofd letter bevatten!";
                return new ValidationResult(Message);
            }
            else if (!val.Any(char.IsLower))
            {
                Message = "Het wachtwoord moet minstens 1 kleine letter bevatten!";
                return new ValidationResult(Message);
            }
            else if (!val.Any(char.IsNumber))
            {
                Message = "Het wachtwoord moet minstens 1 cijfer bevatten!";
                return new ValidationResult(Message);
            }
            else if (!val.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                Message = "Het wachtwoord moet minstens 1 speciaal teken bevatten!";
                return new ValidationResult(Message);
            }
            else if (val.Length < 8)
            {
                Message = "Het wachtwoord moet minimaal 8 tekens zijn!";
                return new ValidationResult(Message);
            }

            return ValidationResult.Success;

        }
    }
}