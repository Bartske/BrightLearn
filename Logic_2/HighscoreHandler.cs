using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using Models;

namespace Logic
{
    public class HighscoreHandler
    {
        BrightLearnContext _dbContext = new BrightLearnContext();

        public HighscoreHandler(bool test = false)
        {
            
        }

        //Create
        public void CreateHighScore(int GameID, int UserID, int Points, int Time)
        {
            if (GameID == 0)
                throw new Exception("The ID of the Game cannot be 0");
            if (UserID == 0)
                throw new Exception("The ID of the user cannot be 0");
            if (Points == 0)
                throw new Exception("Time cannot be 0");
            if (Time == 0)
                throw new Exception("Points cannot be 0");

            if (_dbContext.Game.Where(g=>g.ID == GameID).Count() == 0)
                throw new Exception("There is no record of that Game!");
            if (_dbContext.User.Where(u=>u.ID == UserID).Count() == 0)
                throw new Exception("There is no record of that user!");
            

            if (_dbContext.HighScore.Where(hs=>hs.GameID == GameID && hs.UserID == UserID).Count() > 0)
            {
                UpdateScore(GameID, UserID, Points, Time);
            }
            else
            {
                _dbContext.HighScore.Add(new Models.DataModels.HighScore() {
                    GameID = GameID,
                    UserID = UserID,
                    Points = Points,
                    Time = Time
                });
                _dbContext.SaveChanges();
            }
        }

        //Read

        public GameHighscore GetGameHighscore(int GameID)
        {
            if (_dbContext.Game.Where(g => g.ID == GameID).Count() == 0)
                throw new Exception("There is no record of that Game!");

            List<Models.DataModels.HighScore> GameHighScores = _dbContext.HighScore.Where(hs => hs.GameID == GameID).ToList();
            List<int> UserIDs = GameHighScores.Select(ghs => ghs.UserID).ToList();
            List<Models.DataModels.User> users = _dbContext.User.Where(u => UserIDs.Contains(u.ID)).ToList();
            List<string> FirstNames = users.Select(u => u.FirstName).ToList();
            List<string> MiddleNames = users.Select(u => u.MiddleName).ToList();
            List<string> LastNames      = users.Select(u => u.LastName).ToList();
            List<string> Fullnames = new List<string>();

            for (int i = 0; i < FirstNames.Count; i++)
            {
                if (MiddleNames[i] != "")
                {
                    Fullnames.Add(FirstNames[i] + " " + MiddleNames[i] + " " + LastNames[i]);
                }
                else
                {
                    Fullnames.Add(FirstNames[i] + " " + LastNames[i]);
                }
            }

            Models.DataModels.Game Game = _dbContext.Game.Find(GameID);

            return new GameHighscore()
            {
                GameID = GameID,
                GameName = Game.Name,
                Name = Fullnames,
                points = _dbContext.HighScore.Where(hs => hs.GameID == GameID).ToList().Select(hs => hs.Points.ToString()).ToList()
            };
        }

        public GameHighscore GetGameHighscore(string GameName)
        {
            if (GameName == "")
                throw new Exception("Name cannot be empty!");
            if (_dbContext.Game.Where(g => g.Name == GameName).Count() == 0)
                throw new Exception("There is no record of that Game!");

            return GetGameHighscore(_dbContext.Game.Where(g => g.Name == GameName).First().ID);
        }

        public List<GameHighscore> GetAllGameHighscores()
        {
            List<GameHighscore> GameHighScores = new List<GameHighscore>();

            foreach (string GameName in _dbContext.Game.Select(g=>g.Name).ToList())
            {
                GameHighScores.Add(GetGameHighscore(GameName));
            }

            return GameHighScores;
        }

        //Update

        public void UpdateScore(int GameID, int UserID, int Points, int Time)
        {
            if (GameID == 0)
                throw new Exception("The ID of the Game cannot be 0");
            if (UserID == 0)
                throw new Exception("The ID of the user cannot be 0");
            if (Points == 0)
                throw new Exception("Time cannot be 0");
            if (Time == 0)
                throw new Exception("Points cannot be 0");
            if (_dbContext.Game.Where(g=>g.ID == GameID).Count() == 0)
                throw new Exception("There is no record of that Game!");
            if (_dbContext.User.Where(u=>u.ID == UserID).Count() == 0)
                throw new Exception("There is no record of that user!");

            Models.DataModels.HighScore HighScore = _dbContext.HighScore.Where(hs => hs.GameID == GameID && hs.UserID == UserID).First();
            if (HighScore.Points < Points)
            {
                HighScore.Points = Points;
                HighScore.Time = Time;
                _dbContext.SaveChanges();
            }

            _dbContext.SaveChanges();
        }

        //Delete


    }
}