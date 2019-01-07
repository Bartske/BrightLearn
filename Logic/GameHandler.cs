using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Models;
using DAL;

namespace Logic
{
    public class GameHandler
    {
        BrightLearnContext dbContext = new BrightLearnContext();
        QuestionHandler question_Handler;

        public GameHandler(bool test = false)
        {
            if (test)
            {
                question_Handler = new QuestionHandler(true);
            }
            else
            {
                question_Handler = new QuestionHandler();
            }
        }

        //Create

        public void CreateGame(Game game, HttpPostedFileBase file)
        {
            if (game.Lifes < 1 || game.Lifes > 5)
                throw new Exception("De levens moet tussen de 1 en de 5 zijn!");

            if (game.BonusTime < 1 || game.BonusTime > 10)
                throw new Exception("De bonustijd moet tussen de 1 en de 10 zijn!");

            if (game.Name.Length < 3 || game.Name.Length > 25)
                throw new Exception("De naam moet tussen de 3 en 25 characters bevatten!");


            Models.DataModels.Game NewGame = new Models.DataModels.Game() {
                bonusTime = game.BonusTime,
                lifes = game.Lifes,
                name = game.Name,
                Online = game.Online
            };

            string Image = "";
            if (file != null)
            {
                Image = UploadHandler.UploadImage(file, UploadHandler.GetImageSize(PreDefImageSize.GameImage), PreDefImageLocation.GameImage).Name;
                string img = UploadHandler.GetImageUrlPath(PreDefImageLocation.GameImage) + Image;
                //SQL.Insert("INSERT INTO `chartgroup`(`ID`) VALUES(NULL);INSERT INTO `game` (`ID`, `name`, `lifes`, `bonusTime`,`online`, `ChartGroupID`, `img`) VALUES (NULL, '" + game.Name + "', '" + game.Lifes + "', '" + game.BonusTime + "', '1',( SELECT MAX(`chartgroup`.`ID`) FROM `chartgroup`), '" + img + "');");
                NewGame.img = img;
            }

            dbContext.ChartGroup.Add(new Models.DataModels.ChartGroup());
            dbContext.SaveChanges();

            NewGame.ChartGroupID = dbContext.ChartGroup.OrderByDescending(o => o.ID).FirstOrDefault().ID;
            dbContext.Game.Add(NewGame);
            dbContext.SaveChanges();
        }
        
        //Read

        public List<Game> GetAllGames()
        {
            List<Game> list = new List<Game>();
            foreach (Models.DataModels.Game gm in dbContext.Game.ToList())
            {
                list.Add(GetGame(gm.ID));
            }

            return list;
        }

        public Game GetGame(int ID)
        {
            return GetGame(ID.ToString());
        }

        public Game GetGame(string ID)
        {
            int iID = Convert.ToInt32(ID);
            if (dbContext.Game.Where(g => g.ID == iID).Count() == 0)
                throw new Exception("Er is geen game gevonden met de ID : `" + ID + "`");

            Models.DataModels.Game game = dbContext.Game.Where(g => g.ID == iID).First();

            return new Game()
            {
                ID = Convert.ToInt32(ID),
                BonusTime = game.bonusTime,
                Lifes = game.lifes,
                Name = game.name,
                ImageURL = game.img,
                Online = game.Online,
                ChartGroupID = game.ChartGroupID,
                Questions = question_Handler.GetQuestions(Convert.ToInt32(ID))
            };
        }

        //Update

        public void UpdateValues(string Name, int Lifes, int Bonus, int GameID)
        {
            if (dbContext.Game.Where(g=>g.ID == GameID).Count() == 0)
                throw new Exception("Er is geen game gevonden met de ID : `" + GameID.ToString() + "`");

            if (Lifes < 1 || Lifes > 5)
                throw new Exception("De levens moet tussen de 1 en de 5 zijn!");

            if (Bonus < 1 || Bonus > 10)
                throw new Exception("De bonustijd moet tussen de 1 en de 10 zijn!");

            if (Name.Length < 3 || Name.Length > 25)
                throw new Exception("De naam moet tussen de 3 en 25 characters bevatten!");

            Models.DataModels.Game game = dbContext.Game.Where(g => g.ID == GameID).First();
            game.name = Name;
            game.lifes = Lifes;
            game.bonusTime = Bonus;

            dbContext.SaveChanges();
            //SQL.Update("UPDATE `game` SET `name` = '" + Name + "', `lifes` = '" + Lifes.ToString() + "', `bonusTime` = '" + Bonus.ToString() + "' WHERE `ID` = '" + GameID.ToString() + "'");
        }

        public void UpdateOnlineStatus(int GameID, bool status)
        {
            if (dbContext.Game.Where(g => g.ID == GameID).Count() == 0)
                throw new Exception("Er is geen game gevonden met het ID : `" + GameID.ToString() + "`");

            Models.DataModels.Game game = dbContext.Game.Where(g => g.ID == GameID).First();

            if (status)
                game.Online = true;
            else
                game.Online = false;

            dbContext.SaveChanges();
        }

        //Delete
        
        public void DeleteGame(int GameID)
        {
            Game game = GetGame(GameID);

            for (int i = 0; i < game.Questions.Count; i++)
            {
                question_Handler.DeleteQuestion(game.Questions[i].QuestionID, game.ID);
            }

            Models.DataModels.Game gm = dbContext.Game.Where(g => g.ID == GameID).First();
            dbContext.Game.Remove(gm);

            dbContext.SaveChanges();
        }

    }
}