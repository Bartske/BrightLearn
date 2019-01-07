using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BrightLearn.ViewModels.Login
{
    public class ForgotPasswordModel
    {
        [DisplayName("Gebruikersnaam")]
        [Required(ErrorMessage = "Dit veld moet ingevuld worden")]
        public string Username { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Dit veld moet ingevuld worden")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string LoginErrorMessage { get; set; }

    }
}