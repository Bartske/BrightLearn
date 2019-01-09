using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logic
{
    public class ManageHandler
    {
        BrightLearnContext _dbContext = new BrightLearnContext();
        LoginHandler _loginHandler;
        GameHandler _gameHandler;
        UserHandler _userHandler;

        public ManageHandler(bool Test = false)
        {
            if (Test)
            {
                _userHandler = new UserHandler(true);
                _loginHandler = new LoginHandler();
                _gameHandler = new GameHandler(true);
            }
            else
            {
                _userHandler = new UserHandler();
                _gameHandler = new GameHandler();
                _loginHandler = new LoginHandler();
            }


        }

        public void UpdateUserType(int ID, string Type)
        {
            Models.DataModels.User User = _dbContext.User.Where(u => u.ID == ID).First();
            User.Type = Type;
            _dbContext.SaveChanges();
        }

        public User GenerateUser()
        {
            string UserName = "";
            while (true)
            {
                UserName = RandomString(5);
                if (_dbContext.Login.Where(l=>l.UserName == UserName).Count() == 0)
                {
                    break;
                }
            }

            string Password = "WelkomBrightLearn1!";
            string SaltKey = EncryptHandler.RandomString(8);
            string newPass = EncryptHandler.Encrypt(Password, SaltKey);
            Models.DataModels.Login login = new Models.DataModels.Login() {
                UserName = UserName,
                Password = newPass,
                Salt = SaltKey
            };
            _dbContext.Login.Add(login);
            _dbContext.SaveChanges();
            login = _dbContext.Login.OrderByDescending(u => u.ID).FirstOrDefault();

            Models.DataModels.User User = new Models.DataModels.User() {
                Email = "nieuw@mail.nl",
                FirstName = "Voornaam",
                LastName = "Achternaam",
                LoginID = login.ID,
                Type = "user"
            };
            _dbContext.User.Add(User);
            _dbContext.SaveChanges();

            return new User()
            {
                UserName = UserName,
                Password = Password
            };
        }

        private Random Random = new Random();
        private string RandomString(int Length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, Length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}