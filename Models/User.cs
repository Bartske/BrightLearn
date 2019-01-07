using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Models
{
    public class User
    {
        public int ID { get; set; }

        [DisplayName("Voornaam")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Gebruik alstublieft alleen letters")]
        [Required(ErrorMessage = "Dit veld moet ingevuld worden")]
        public string FirstName { get; set; }

        [DisplayName("Tussenvoegsel(s)")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Gebruik alstublieft alleen letters")]
        public string MiddleName { get; set; }

        [DisplayName("Achternaam")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Gebruik alstublieft alleen letters")]
        [Required(ErrorMessage = "Dit veld moet ingevuld worden")]
        public string LastName { get; set; }

        [DisplayName("E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Dit veld moet ingevuld worden")]
        public string Email { get; set; }

        [DisplayName("Gebruikersnaam")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Gebruik alstublieft alleen letters en cijfers")]
        [Required(ErrorMessage = "Dit veld moet ingevuld worden")]
        public string UserName { get; set; }

        public string AccountType { get; set; }

        public string Password { get; set; }
    }
}