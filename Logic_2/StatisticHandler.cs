using Models;
using Models.SatisticModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace Logic
{
    public class StatisticHandler
    {
        BrightLearnContext _dbContext = new BrightLearnContext();

        QuestionHandler _questionHandler;
        GameHandler _gameHandler;
        ChartHandler _chartHandler;

        public StatisticHandler(bool Test = false)
        {
            if (Test)
            {
                _questionHandler = new QuestionHandler(true);
                _gameHandler = new GameHandler(true);
                _chartHandler = new ChartHandler(true);
            }
            else
            {
                _questionHandler = new QuestionHandler();
                _gameHandler = new GameHandler();
                _chartHandler = new ChartHandler();
            }
        }


        //Create

        public void Create(GameScore Score)
        {
            string isPassed = "1";
            if (Score.Passed)
                isPassed = "0";

            int ChartGroupID = _dbContext.Game.Where(g => g.ID == Score.GameID).Select(g => g.ChartGroupID).First();

            Models.DataModels.GameScore GameScore = new Models.DataModels.GameScore()
            {
                Lifes = Score.Lifes,
                NumOfBonus = Score.NumOfBonus,
                Passed = Score.Passed,
                Points = Score.Points,
                TimePlayed = Score.TimePlayed,
                UserID = Score.UserID,
                ChartGroupID = ChartGroupID
            };

            _dbContext.GameScore.Add(GameScore);

            _dbContext.SaveChanges();

            _chartHandler.UpdateChartValues(Score.GameID);
        }
        
        //Read

        public GameStatistic GetGameStatistic(int GameID)
        {
            Models.DataModels.GameStatistics Stats = _dbContext.GameStatistics.Where(g => g.GameID == GameID).OrderByDescending(O=>O.ID).FirstOrDefault();
            return new GameStatistic()
            {
                AVGLifes = Stats.AVGLifes,
                AVGNumOfBonus = Stats.AVGNumOfBonus,
                AVGPoints = Stats.AVGPoints,
                AVGTimePlayed = Stats.AVGTimePlayed,
                NumOfUsers = Stats.NumOfUsers,
                ProcentFailed = Stats.ProcentFailed,
                Game = _gameHandler.GetGame(GameID),
                Charts = _chartHandler.GetGameCharts(GameID)
            };
        }

        public List<GameStatistic> GetGameStatistics()
        {
            List<Game> Games = _gameHandler.GetAllGames();

            List<GameStatistic> list = new List<GameStatistic>();

            foreach (Game Game in Games)
            {
                list.Add(GetGameStatistic(Game.ID));
            }
            return list;
        }


        //Update

        //Delete

    }
}