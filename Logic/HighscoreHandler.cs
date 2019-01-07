using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using Models.Models;

namespace Logic
{
    public class HighscoreHandler
    {
        BrightLearnContext dbContext = new BrightLearnContext();

        public HighscoreHandler(bool test = false)
        {
            
        }

        //Create
        public void CreateHighScore(int GameID, int userID, int points, int time)
        {
            if (GameID == 0)
                throw new Exception("The ID of the game cannot be 0");
            if (userID == 0)
                throw new Exception("The ID of the user cannot be 0");
            if (points == 0)
                throw new Exception("Time cannot be 0");
            if (time == 0)
                throw new Exception("Points cannot be 0");

            if (dbContext.Game.Where(g=>g.ID == GameID).Count() == 0)
                throw new Exception("There is no record of that game!");
            if (dbContext.User.Where(u=>u.ID == userID).Count() == 0)
                throw new Exception("There is no record of that user!");
            

            if (dbContext.HighScore.Where(hs=>hs.GameID == GameID && hs.UserID == userID).Count() > 0)
            {
                UpdateScore(GameID, userID, points, time);
            }
            else
            {
                dbContext.HighScore.Add(new Models.DataModels.HighScore() {
                    GameID = GameID,
                    UserID = userID,
                    Points = points,
                    Time = time
                });
                dbContext.SaveChanges();
                //SQL.Insert("INSERT INTO `highscore` (`ID`, `gameID`, `userID`, `points`, `time`) VALUES (NULL, '" + GameID.ToString() + "', '" + userID.ToString() + "', '" + points.ToString() + "', '" + time.ToString() + "');");
            }
        }

        //Read

        public GameHighscore GetGameHighscore(int GameID)
        {
            if (dbContext.Game.Where(g => g.ID == GameID).Count() == 0)
                throw new Exception("There is no record of that game!");

            List<Models.DataModels.HighScore> GameHighScores = dbContext.HighScore.Where(hs => hs.GameID == GameID).ToList();
            List<int> userIDs = GameHighScores.Select(ghs => ghs.UserID).ToList();
            List<Models.DataModels.User> users = dbContext.User.Where(u => userIDs.Contains(u.ID)).ToList();
            List<string> firstNames = users.Select(u => u.FirstName).ToList();
            List<string> middleNames = users.Select(u => u.MiddleName).ToList();
            List<string> LastNames      = users.Select(u => u.LastName).ToList();
            List<string> fullnames = new List<string>();

            for (int i = 0; i < firstNames.Count; i++)
            {
                if (middleNames[i] != "")
                {
                    fullnames.Add(firstNames[i] + " " + middleNames[i] + " " + LastNames[i]);
                }
                else
                {
                    fullnames.Add(firstNames[i] + " " + LastNames[i]);
                }
            }

            Models.DataModels.Game game = dbContext.Game.Find(GameID);

            return new GameHighscore()
            {
                GameID = GameID,
                GameName = game.name,
                Name = fullnames,
                points = dbContext.HighScore.Where(hs => hs.GameID == GameID).ToList().Select(hs => hs.Points.ToString()).ToList()
            };
        }

        public GameHighscore GetGameHighscore(string GameName)
        {
            if (GameName == "")
                throw new Exception("Name cannot be empty!");

            return GetGameHighscore(dbContext.Game.Where(g => g.name == GameName).First().ID);
        }

        public List<GameHighscore> GetAllGameHighscores()
        {
            List<GameHighscore> list = new List<GameHighscore>();

            foreach (string gameName in dbContext.Game.Select(g=>g.name).ToList())
            {
                list.Add(GetGameHighscore(gameName));
            }

            return list;
        }

        //Update

        public void UpdateScore(int GameID, int userID, int points, int time)
        {
            if (GameID == 0)
                throw new Exception("The ID of the game cannot be 0");
            if (userID == 0)
                throw new Exception("The ID of the user cannot be 0");
            if (points == 0)
                throw new Exception("Time cannot be 0");
            if (time == 0)
                throw new Exception("Points cannot be 0");
            if (dbContext.Game.Where(g=>g.ID == GameID).Count() == 0)
                throw new Exception("There is no record of that game!");
            if (dbContext.User.Where(u=>u.ID == userID).Count() == 0)
                throw new Exception("There is no record of that user!");

            Models.DataModels.HighScore highScore = dbContext.HighScore.Where(hs => hs.GameID == GameID && hs.UserID == userID).First();
            if (highScore.Points < points)
            {
                highScore.Points = points;
                highScore.Time = time;
                dbContext.SaveChanges();
                //SQL.Update("UPDATE `highscore` SET `points` = '" + points.ToString() + "', `time` = '" + time.ToString() + "' WHERE `gameID` = '" + GameID.ToString() + "' AND `userID` = '" + userID.ToString() + "'");
            }

            dbContext.SaveChanges();
        }

        //Delete


    }
}