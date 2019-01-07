using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using Models.Models;

namespace Logic
{
    public class UserHandler
    {
        BrightLearnContext dbContext = new BrightLearnContext();

        LoginHandler _loginHandler;

        public UserHandler(bool test = false)
        {
            if (test)
            {
                _loginHandler = new LoginHandler();
            }
            else
            {
                _loginHandler = new LoginHandler();
            }
        }

        //Create


        //Read

        public User GetUser(string userName, string Password)
        {

            if (userName == "")
                throw new Exception("There was no username given!");

            if (Password == "")
                throw new Exception("There was no password given!");

            if (dbContext.Login.Where(l=>l.UserName == userName && l.Password == Password).Count() == 0)
                throw new Exception("There is no record of that user!");
            Models.DataModels.Login U_L = dbContext.Login.Where(l => l.UserName == userName && l.Password == Password).First();
            int loginID = U_L.ID;
            Models.DataModels.User U = dbContext.User.Where(u => u.LoginID == loginID).First();

            return new User()
            {
                FirstName       = U.FirstName,
                MiddleName      = U.MiddleName,
                LastName        = U.LastName,
                ID              = U.ID,
                Email           = U.Email,
                UserName        = userName,
                AccountType     = U.Type,
            };
        }

        public User GetUser(string firstName, string middleName, string lastName)
        {
            if (firstName == "" || lastName == "")
                throw new Exception("You must fill in the firstname and lastname!");

            if (dbContext.User.Where(u=> u.FirstName == firstName && u.MiddleName == middleName && u.LastName == lastName).Count() == 0)
                throw new Exception("There is no record of that user!");

            Models.DataModels.User U = dbContext.User.Where(u => u.FirstName == firstName && u.MiddleName == middleName && u.LastName == lastName).Last();
            string UserName = dbContext.Login.Where(l => l.ID == U.LoginID).Last().UserName;

            return new User()
            {
                FirstName = U.FirstName,
                MiddleName = U.MiddleName,
                LastName = U.LastName,
                ID = U.ID,
                Email = U.Email,
                UserName = UserName,
                AccountType = U.Type,
            };
        }

        public User GetUser(string email)
        {
            if (email == "")
                throw new Exception("Email must be filled in!");

            if (dbContext.User.Where(u => u.Email == email).Count() == 0)
                throw new Exception("There is no record of that user!");

            Models.DataModels.User U = dbContext.User.Where(u => u.Email == email).Last();
            string UserName = dbContext.Login.Where(l => l.ID == U.LoginID).Last().UserName;

            return new User()
            {
                FirstName = U.FirstName,
                MiddleName = U.MiddleName,
                LastName = U.LastName,
                ID = U.ID,
                Email = U.Email,
                UserName = UserName,
                AccountType = U.Type,
            };
        }

        public User GetUser(int ID)
        {
            if (dbContext.User.Where(u=>u.ID == ID).Count() == 0)
                throw new Exception("There is no record of that user!");


            Models.DataModels.User U = dbContext.User.Where(u => u.ID == ID).ToList().Last();
            string UserName = dbContext.Login.Where(l => l.ID == U.LoginID).ToList().Last().UserName;

            return new User()
            {
                FirstName = U.FirstName,
                MiddleName = U.MiddleName,
                LastName = U.LastName,
                ID = U.ID,
                Email = U.Email,
                UserName = UserName,
                AccountType = U.Type,
            };
        }

        public User GetCurrentUser()
        {
            return GetUser((int)HttpContext.Current.Session["UserID"]);
        }


        //Update

        public bool UpdateUser(User user)
        {
            if (user.FirstName == "" || user.LastName == "" || user.Email == "" || user.AccountType == "" || user.UserName == "")
                throw new Exception("All the values must be filled in!");
            if (dbContext.Login.Where(l=>l.UserName == user.UserName).Count() > 0)
                throw new Exception("This username already exists!");

            if (dbContext.User.Where(u=>u.ID == user.ID).Count() == 0)
                throw new Exception("There is no record of that user!");

            Models.DataModels.User U = dbContext.User.Where(u => u.ID == user.ID).First();
            Models.DataModels.Login L = dbContext.Login.Where(l => l.ID == U.LoginID).First();

            U.Email = user.Email;
            U.FirstName = user.FirstName;
            U.LastName = user.LastName;
            U.MiddleName = user.MiddleName;
            U.Type = user.AccountType;

            L.UserName = user.UserName;

            dbContext.SaveChanges();
            
            return true;
        }

        public bool UpdateUser(string firstName, string middleName, string lastName, string Email, string AccountType, string userName, int ID)
        {
            return UpdateUser(new User { FirstName = firstName, MiddleName = middleName, LastName = lastName, AccountType = AccountType, Email = Email, ID = ID, UserName = userName });
        }

        public bool UpdatePassword(string currentPassword, string newPassword, string repeatNewPassword, int userID)
        {
            if (currentPassword == "" || newPassword == "" || repeatNewPassword == "" || currentPassword == null || newPassword == null || repeatNewPassword == null)
                throw new Exception("All fields must be filled in!");

            _loginHandler.isValidPassword(currentPassword);
            _loginHandler.isValidPassword(newPassword);

            if (newPassword != repeatNewPassword)
                throw new Exception("The Passwords do not match");

            if (dbContext.Login.Where(l=>l.ID == userID).Count() == 0)
                throw new Exception("There is no record of that user!");

            string UserName = dbContext.Login.Where(l => l.ID == userID).First().UserName;
            if (!_loginHandler.Auth(UserName, currentPassword))
                throw new Exception("Het wachtwoord is incorrect!");

            Models.DataModels.Login L = dbContext.Login.Where(l => l.ID == userID).First();

            string encryptedPassword = _loginHandler.EncryptPass(newPassword, UserName);
            L.Password = encryptedPassword;
            L.UserName = UserName;

            dbContext.SaveChanges();

            return true;
        }

        //Delete


    }
}