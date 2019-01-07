using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using DAL;

namespace Logic
{
    public class GameHandler
    {
        BrightLearnContext _dbContext = new BrightLearnContext();
        QuestionHandler _questionHandler;

        public GameHandler(bool test = false)
        {
            if (test)
            {
                _questionHandler = new QuestionHandler(true);
            }
            else
            {
                _questionHandler = new QuestionHandler();
            }
        }

        //Create

        public void CreateGame(Game Game, HttpPostedFileBase File)
        {
            if (Game.Lifes < 1 || Game.Lifes > 5)
                throw new Exception("De levens moet tussen de 1 en de 5 zijn!");

            if (Game.BonusTime < 1 || Game.BonusTime > 10)
                throw new Exception("De bonustijd moet tussen de 1 en de 10 zijn!");

            if (Game.Name.Length < 3 || Game.Name.Length > 25)
                throw new Exception("De naam moet tussen de 3 en 25 characters bevatten!");


            Models.DataModels.Game NewGame = new Models.DataModels.Game() {
                BonusTime = Game.BonusTime,
                Lifes = Game.Lifes,
                Name = Game.Name,
                Online = Game.Online
            };

            string Image = "";
            if (File != null)
            {
                Image = UploadHandler.UploadImage(File, UploadHandler.GetImageSize(PreDefImageSize.GameImage), PreDefImageLocation.GameImage).Name;
                string img = UploadHandler.GetImageUrlPath(PreDefImageLocation.GameImage) + Image;
                //SQL.Insert("INSERT INTO `chartgroup`(`ID`) VALUES(NULL);INSERT INTO `game` (`ID`, `name`, `lifes`, `bonusTime`,`online`, `ChartGroupID`, `img`) VALUES (NULL, '" + game.Name + "', '" + game.Lifes + "', '" + game.BonusTime + "', '1',( SELECT MAX(`chartgroup`.`ID`) FROM `chartgroup`), '" + img + "');");
                NewGame.IMG = img;
            }

            _dbContext.ChartGroup.Add(new Models.DataModels.ChartGroup());
            _dbContext.SaveChanges();

            NewGame.ChartGroupID = _dbContext.ChartGroup.OrderByDescending(o => o.ID).FirstOrDefault().ID;
            _dbContext.Game.Add(NewGame);
            _dbContext.SaveChanges();
        }
        
        //Read

        public List<Game> GetAllGames()
        {
            List<Game> Games = new List<Game>();
            foreach (Models.DataModels.Game Game in _dbContext.Game.ToList())
            {
                Games.Add(GetGame(Game.ID));
            }

            return Games;
        }

        public Game GetGame(int ID)
        {
            return GetGame(ID.ToString());
        }

        public Game GetGame(string ID)
        {
            int iID = Convert.ToInt32(ID);
            if (_dbContext.Game.Where(g => g.ID == iID).Count() == 0)
                throw new Exception("Er is geen game gevonden met de ID : `" + ID + "`");

            Models.DataModels.Game Game = _dbContext.Game.Where(g => g.ID == iID).First();

            return new Game()
            {
                ID = Convert.ToInt32(ID),
                BonusTime = Game.BonusTime,
                Lifes = Game.Lifes,
                Name = Game.Name,
                ImageURL = Game.IMG,
                Online = Game.Online,
                ChartGroupID = Game.ChartGroupID,
                Questions = _questionHandler.GetQuestions(Convert.ToInt32(ID))
            };
        }

        //Update

        public void UpdateValues(string Name, int Lifes, int Bonus, int GameID)
        {
            if (_dbContext.Game.Where(g=>g.ID == GameID).Count() == 0)
                throw new Exception("Er is geen game gevonden met de ID : `" + GameID.ToString() + "`");

            if (Lifes < 1 || Lifes > 5)
                throw new Exception("De levens moet tussen de 1 en de 5 zijn!");

            if (Bonus < 1 || Bonus > 10)
                throw new Exception("De bonustijd moet tussen de 1 en de 10 zijn!");

            if (Name.Length < 3 || Name.Length > 25)
                throw new Exception("De naam moet tussen de 3 en 25 characters bevatten!");

            Models.DataModels.Game Game = _dbContext.Game.Where(g => g.ID == GameID).First();
            Game.Name = Name;
            Game.Lifes = Lifes;
            Game.BonusTime = Bonus;

            _dbContext.SaveChanges();
        }

        public void UpdateOnlineStatus(int GameID, bool status)
        {
            if (_dbContext.Game.Where(g => g.ID == GameID).Count() == 0)
                throw new Exception("Er is geen game gevonden met het ID : `" + GameID.ToString() + "`");

            Models.DataModels.Game Game = _dbContext.Game.Where(g => g.ID == GameID).First();

            if (status)
                Game.Online = true;
            else
                Game.Online = false;

            _dbContext.SaveChanges();
        }

        //Delete
        
        public void DeleteGame(int GameID)
        {
            Game Game = GetGame(GameID);

            for (int i = 0; i < Game.Questions.Count; i++)
            {
                _questionHandler.DeleteQuestion(Game.Questions[i].QuestionID, Game.ID);
            }

            Models.DataModels.Game GameToDelete = _dbContext.Game.Where(g => g.ID == GameID).First();
            _dbContext.Game.Remove(GameToDelete);

            _dbContext.SaveChanges();
        }

    }
}