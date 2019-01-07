using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace Logic
{
    public class LoginHandler
    {
        BrightLearnContext _dbContext = new BrightLearnContext();

        public LoginHandler()
        {

        }

        public bool Auth(string UserName, string Password)
        {
            if (UserName == "")
            {
                throw new Exception("UserName cannot be empty");
            }
            else if (Password == "")
            {
                throw new Exception("Password cannot be empty");
            }
            
            //Kijken of de gebruikersnaam in de database voorkomt
            if (_dbContext.Login.Where(l => l.UserName == UserName).Count() == 0) 
            {
                throw new Exception("Deze gebruikersnaam is niet bekend bij ons!");
            }

            IsValidPassword(Password);

            //Het Wachtwoord Encrypten
            string EncryptedPassword = EncryptPass(Password, UserName);

            //Kijken of de gebruikersnaam en het wachtwoord overeenkomen
            if (_dbContext.Login.Where(l => l.UserName == UserName && l.Password == EncryptedPassword).Count() == 0)
            {
                throw new Exception("Het wachtwoord is incorrect!");
            }
            //Terug geven dat het klopt
            else
            {
                return true;
            }
        }

        public string EncryptPass(string Password, string UserName)
        {
            return EncryptHandler.Encrypt(Password, _dbContext.Login.Where(l => l.UserName == UserName).First().Salt);
        }

        public void RecoverPassword(string UserName, string Email)
        {
            if (_dbContext.Login.Where(l => l.UserName == UserName).Count() == 0)
                throw new Exception("Er is geen account met deze gebruikersnaam bij ons bekent.");
            if (_dbContext.User.Where(u => u.Email == Email).Count() == 0)
                throw new Exception("Er is geen account met deze email bij ons bekent.");
            if (_dbContext.User.Where(u => u.Email == Email).First().LoginID != _dbContext.Login.Where(l => l.UserName == UserName).First().ID)
                throw new Exception("De gebruikersnaam en het email adres komt niet overeen");
            
            Models.DataModels.Login Login = _dbContext.Login.Where(l => l.UserName == UserName).First();

            string NewPassword = "BrightLearn1!";
            string EncryptedNewPassword = EncryptHandler.Encrypt(NewPassword, Login.Salt);

            Login.Password = EncryptedNewPassword;
            _dbContext.SaveChanges();

            EmailHandler EmailHandler = new EmailHandler();

            string Message = "Beste " + UserName + "," + Environment.NewLine +
                Environment.NewLine +
                "Je hebt op onze site een niew wachtwoord aangevraagd." + Environment.NewLine +
                "Jouw niew wachtwoord is : " + NewPassword + Environment.NewLine+
                Environment.NewLine +
                "Met Vriendelijke Groet" + Environment.NewLine+
                "BrightLearn";

            EmailHandler.SendEmail(Email, "Wachtwoord herstel",Message);
        }

        public bool IsValidPassword(string Password)
        {
            if (Password == "")
            {
                throw new Exception("Het wachtwoord kan niet leeg zijn!");
            }
            else if (!Password.Any(char.IsUpper))
            {
                throw new Exception("Het wachtwoord moet minstens 1 hoofd letter bevatten!");
            }
            else if (!Password.Any(char.IsLower))
            {
                throw new Exception("Het wachtwoord moet minstens 1 kleine letter bevatten!");
            }
            else if (!Password.Any(char.IsNumber))
            {
                throw new Exception("Het wachtwoord moet minstens 1 cijfer bevatten!");
            }
            else if (!Password.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                throw new Exception("Het wachtwoord moet minstens 1 speciaal teken bevatten!");
            }
            else if (Password.Length < 8)
            {
                throw new Exception("Het wachtwoord moet minimaal 8 tekens zijn!");
            }
            
            return true;
        }
    }
}