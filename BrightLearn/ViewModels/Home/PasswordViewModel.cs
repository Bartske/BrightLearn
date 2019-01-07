using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrightLearn.ViewModels.Home
{
    public class PasswordViewModel
    {
        public string ID { get; set; }

        [DisplayName("Huidig Wachtwoord")]
        [DataType(DataType.Password)]
        [ValidationClasses.PasswordValidate]
        public string currentPassword { get; set; }

        [DisplayName("Nieuw Wachtwoord")]
        [DataType(DataType.Password)]
        [ValidationClasses.PasswordValidate]
        public string newPassword { get; set; }

        [DisplayName("Herhaal nieuw Wachtwoord")]
        [DataType(DataType.Password)]
        [ValidationClasses.PasswordValidate]
        public string repeatNewPassword { get; set; }

        public string ValidationErrorMessage;
    }
}