using DAL;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logic
{
    public class ManageHandler
    {
        BrightLearnContext dbContext = new BrightLearnContext();
        LoginHandler login_Handler;
        GameHandler game_Handler;
        UserHandler _userHandler;

        public ManageHandler(bool test = false)
        {
            if (test)
            {
                _userHandler = new UserHandler(true);
                login_Handler = new LoginHandler();
                game_Handler = new GameHandler(true);
            }
            else
            {
                _userHandler = new UserHandler();
                game_Handler = new GameHandler();
                login_Handler = new LoginHandler();
            }


        }
        
        public AccountsViewModel AccountsViewModel()
        {
            List<string> accountIDS = dbContext.User.Select(u => u.ID.ToString()).ToList();
            
            AccountsViewModel model = new AccountsViewModel();
            model.Accounts = new List<User>();

            if (accountIDS.Count == 0)
                model.ErrorMessage = "Er zijn geen accounts gevonden!";

            foreach (string ID in accountIDS)
            {
                model.Accounts.Add(_userHandler.GetUser(Convert.ToInt32(ID)));
            }

            return model;
        }

        public GamesViewModel GamesViewModel()
        {
            return new GamesViewModel()
            {
                Games = game_Handler.GetAllGames()
            };
        }

        public void UpdateUserType(int ID, string type)
        {
            Models.DataModels.User usr = dbContext.User.Where(u => u.ID == ID).First();
            usr.Type = type;
            dbContext.SaveChanges();
        }

        public User GenerateUser()
        {
            string UserName = "";
            while (true)
            {
                UserName = RandomString(5);
                if (dbContext.Login.Where(l=>l.UserName == UserName).Count() == 0)
                {
                    break;
                }
            }

            string Password = "WelkomBrightLearn1!";
            string SaltKey = EncryptHandler.RandomString(8);
            Models.DataModels.Login login = new Models.DataModels.Login() {
                UserName = UserName,
                Password = Password,
                Salt = SaltKey
            };
            dbContext.SaveChanges();
            int loginID = dbContext.Login.Last().ID;

            //SQL.Insert("INSERT INTO `login` (`ID`, `username`, `password`, `salt`) VALUES (NULL, '" + UserName + "', '" + EncryptHandler.Encrypt(Password, SaltKey) + "', '" + SaltKey + "');");

            //string loginID = SQL.Select("SELECT `ID` FROM `login` WHERE `username` = '"+UserName+"'")[0];

            Models.DataModels.User user = new Models.DataModels.User() {
                Email = "nieuw@mail.nl",
                FirstName = "Voornaam",
                LastName = "Achternaam",
                LoginID = loginID,
                Type = "user"
            };
            dbContext.User.Add(user);
            dbContext.SaveChanges();

            //SQL.Insert("INSERT INTO `user` (`ID`, `loginID`, `email`, `firstName`, `middleName`, `lastName`, `type`) VALUES (NULL, '"+loginID+"', 'nieuw@mail.nl', 'Voornaam', NULL, 'Achternaam', 'user');");

            return new User()
            {
                UserName = UserName,
                Password = Password
            };
        }

        private Random random = new Random();
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}