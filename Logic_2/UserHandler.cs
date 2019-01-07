using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using Models;

namespace Logic
{
    public class UserHandler
    {
        BrightLearnContext _dbContext = new BrightLearnContext();

        LoginHandler _loginHandler;

        public UserHandler(bool Test = false)
        {
            if (Test)
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

        public User GetUser(string UserName, string Password)
        {

            if (UserName == "")
                throw new Exception("There was no username given!");

            if (Password == "")
                throw new Exception("There was no password given!");

            if (_dbContext.Login.Where(l=>l.UserName == UserName && l.Password == Password).Count() == 0)
                throw new Exception("There is no record of that user!");
            Models.DataModels.Login U_L = _dbContext.Login.Where(l => l.UserName == UserName && l.Password == Password).First();
            int loginID = U_L.ID;
            Models.DataModels.User U = _dbContext.User.Where(u => u.LoginID == loginID).First();

            return new User()
            {
                FirstName       = U.FirstName,
                MiddleName      = U.MiddleName,
                LastName        = U.LastName,
                ID              = U.ID,
                Email           = U.Email,
                UserName        = UserName,
                AccountType     = U.Type,
            };
        }

        public User GetUser(string FirstName, string MiddleName, string LastName)
        {
            if (FirstName == "" || LastName == "")
                throw new Exception("You must fill in the firstname and lastname!");

            if (_dbContext.User.Where(u=> u.FirstName == FirstName && u.MiddleName == MiddleName && u.LastName == LastName).Count() == 0)
                throw new Exception("There is no record of that user!");

            Models.DataModels.User User = _dbContext.User.Where(u => u.FirstName == FirstName && u.MiddleName == MiddleName && u.LastName == LastName).Last();
            string UserName = _dbContext.Login.Where(l => l.ID == User.LoginID).Last().UserName;

            return new User()
            {
                FirstName = User.FirstName,
                MiddleName = User.MiddleName,
                LastName = User.LastName,
                ID = User.ID,
                Email = User.Email,
                UserName = UserName,
                AccountType = User.Type,
            };
        }

        public User GetUser(string Email)
        {
            if (Email == "")
                throw new Exception("Email must be filled in!");

            if (_dbContext.User.Where(u => u.Email == Email).Count() == 0)
                throw new Exception("There is no record of that user!");

            Models.DataModels.User User = _dbContext.User.Where(u => u.Email == Email).Last();
            string UserName = _dbContext.Login.Where(l => l.ID == User.LoginID).Last().UserName;

            return new User()
            {
                FirstName = User.FirstName,
                MiddleName = User.MiddleName,
                LastName = User.LastName,
                ID = User.ID,
                Email = User.Email,
                UserName = UserName,
                AccountType = User.Type,
            };
        }

        public User GetUser(int ID)
        {
            if (_dbContext.User.Where(u=>u.ID == ID).Count() == 0)
                throw new Exception("There is no record of that user!");


            Models.DataModels.User User = _dbContext.User.Where(u => u.ID == ID).ToList().Last();
            string UserName = _dbContext.Login.Where(l => l.ID == User.LoginID).ToList().Last().UserName;

            return new User()
            {
                FirstName = User.FirstName,
                MiddleName = User.MiddleName,
                LastName = User.LastName,
                ID = User.ID,
                Email = User.Email,
                UserName = UserName,
                AccountType = User.Type,
            };
        }

        public User GetCurrentUser()
        {
            return GetUser((int)HttpContext.Current.Session["UserID"]);
        }

        public List<User> GetAllUsers()
        {
            List<User> Users = new List<User>();
            List<int> UserIDS = _dbContext.User.Select(u => u.ID).ToList();
            for (int i = 0; i < UserIDS.Count; i++)
            {
                Users.Add(GetUser(UserIDS[i]));
            }
            return Users;
        }


        //Update

        public bool UpdateUser(User User)
        {
            if (User.FirstName == "" || User.LastName == "" || User.Email == "" || User.AccountType == "" || User.UserName == "")
                throw new Exception("All the values must be filled in!");
            if (_dbContext.Login.Where(l=>l.UserName == User.UserName).Count() > 0)
                throw new Exception("This username already exists!");

            if (_dbContext.User.Where(u=>u.ID == User.ID).Count() == 0)
                throw new Exception("There is no record of that user!");

            Models.DataModels.User NewUser = _dbContext.User.Where(u => u.ID == User.ID).First();
            Models.DataModels.Login Login = _dbContext.Login.Where(l => l.ID == NewUser.LoginID).First();

            NewUser.Email = User.Email;
            NewUser.FirstName = User.FirstName;
            NewUser.LastName = User.LastName;
            NewUser.MiddleName = User.MiddleName;
            NewUser.Type = User.AccountType;

            Login.UserName = User.UserName;

            _dbContext.SaveChanges();
            
            return true;
        }

        public bool UpdateUser(string FirstName, string MiddleName, string LastName, string Email, string AccountType, string UserName, int ID)
        {
            return UpdateUser(new User { FirstName = FirstName, MiddleName = MiddleName, LastName = LastName, AccountType = AccountType, Email = Email, ID = ID, UserName = UserName });
        }

        public bool UpdatePassword(string CurrentPassword, string NewPassword, string RepeatNewPassword, int UserID)
        {
            if (CurrentPassword == "" || NewPassword == "" || RepeatNewPassword == "" || CurrentPassword == null || NewPassword == null || RepeatNewPassword == null)
                throw new Exception("All fields must be filled in!");

            _loginHandler.IsValidPassword(CurrentPassword);
            _loginHandler.IsValidPassword(NewPassword);

            if (NewPassword != RepeatNewPassword)
                throw new Exception("The Passwords do not match");

            if (_dbContext.Login.Where(l=>l.ID == UserID).Count() == 0)
                throw new Exception("There is no record of that user!");

            string UserName = _dbContext.Login.Where(l => l.ID == UserID).First().UserName;
            if (!_loginHandler.Auth(UserName, CurrentPassword))
                throw new Exception("Het wachtwoord is incorrect!");

            Models.DataModels.Login L = _dbContext.Login.Where(l => l.ID == UserID).First();

            string EncryptedPassword = _loginHandler.EncryptPass(NewPassword, UserName);
            L.Password = EncryptedPassword;
            L.UserName = UserName;

            _dbContext.SaveChanges();

            return true;
        }

        //Delete


    }
}