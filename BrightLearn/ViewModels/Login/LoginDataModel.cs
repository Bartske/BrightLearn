using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrightLearn.ViewModels.Login
{
    public class LoginDataModel
    {
        public int ID { get; set; }
        
        [DisplayName("Gebruikersnaam")]
        [Required(ErrorMessage = "Dit veld moet ingevuld worden")]
        public string UserName { get; set; }

        [DisplayName("Wachtwoord")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Dit veld moet ingevuld worden")]
        [ValidationClasses.PasswordValidate]
        public string Password { get; set; }

        public string LoginErrorMessage { get; set; }
    }
}