using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace Logic
{
    public class LoginHandler
    {
        BrightLearnContext dbContext = new BrightLearnContext();

        public LoginHandler()
        {

        }

        public bool Auth(string username, string password)
        {
            if (username == "")
            {
                throw new Exception("Username cannot be empty");
            }
            else if (password == "")
            {
                throw new Exception("Password cannot be empty");
            }
            
            //Kijken of de gebruikersnaam in de database voorkomt
            if (dbContext.Login.Where(l => l.UserName == username).Count() == 0) 
            {
                throw new Exception("Deze gebruikersnaam is niet bekend bij ons!");
            }

            isValidPassword(password);

            //Het Wachtwoord Encrypten
            string Password = EncryptPass(password, username);

            //Kijken of de gebruikersnaam en het wachtwoord overeenkomen
            if (dbContext.Login.Where(l => l.UserName == username && l.Password == Password).Count() == 0)
            {
                throw new Exception("Het wachtwoord is incorrect!");
            }
            //Terug geven dat het klopt
            else
            {
                return true;
            }
        }

        public string EncryptPass(string password, string username)
        {
            return EncryptHandler.Encrypt(password, dbContext.Login.Where(l => l.UserName == username).First().Salt);
        }

        public void RecoverPassword(string UserName, string Email)
        {
            if (dbContext.Login.Where(l => l.UserName == UserName).Count() == 0)
                throw new Exception("Er is geen account met deze gebruikersnaam bij ons bekent.");
            if (dbContext.User.Where(u => u.Email == Email).Count() == 0)
                throw new Exception("Er is geen account met deze email bij ons bekent.");
            if (dbContext.User.Where(u => u.Email == Email).First().LoginID != dbContext.Login.Where(l => l.UserName == UserName).First().ID)
                throw new Exception("De gebruikersnaam en het email adres komt niet overeen");
            
            Models.DataModels.Login login = dbContext.Login.Where(l => l.UserName == UserName).First();

            string newPassword = "BrightLearn1!";
            string encryptedNewPassword = EncryptHandler.Encrypt(newPassword, login.Salt);

            login.Password = encryptedNewPassword;
            dbContext.SaveChanges();

            EmailHandler emailHandler = new EmailHandler();

            string Message = "Beste " + UserName + "," + Environment.NewLine +
                Environment.NewLine +
                "Je hebt op onze site een niew wachtwoord aangevraagd." + Environment.NewLine +
                "Jouw niew wachtwoord is : " + newPassword + Environment.NewLine+
                Environment.NewLine +
                "Met Vriendelijke Groet" + Environment.NewLine+
                "BrightLearn";

            emailHandler.SendEmail(Email, "Wachtwoord herstel",Message);
        }

        public bool isValidPassword(string Password)
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