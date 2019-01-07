using Models.Models;
using Models.Models.SatisticModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace Logic
{
    public class StatisticHandler
    {
        BrightLearnContext dbContext = new BrightLearnContext();

        QuestionHandler question_Handler;
        GameHandler _gameHandler;
        ChartHandler _chartHandler;

        public StatisticHandler(bool test = false)
        {
            if (test)
            {
                question_Handler = new QuestionHandler(true);
                _gameHandler = new GameHandler(true);
                _chartHandler = new ChartHandler(true);
            }
            else
            {
                question_Handler = new QuestionHandler();
                _gameHandler = new GameHandler();
                _chartHandler = new ChartHandler();
            }
        }


        //Create

        public void Create(GameScore score)
        {
            string isPassed = "1";
            if (score.Passed)
                isPassed = "0";

            int ChartGroupID = dbContext.Game.Where(g => g.ID == score.GameID).Select(g => g.ChartGroupID).First();

            Models.DataModels.GameScore gs = new Models.DataModels.GameScore()
            {
                Lifes = score.Lifes,
                NumOfBonus = score.NumOfBonus,
                Passed = score.Passed,
                Points = score.Points,
                TimePlayed = score.TimePlayed,
                userID = score.userID,
                ChartGroupID = ChartGroupID
            };

            dbContext.GameScore.Add(gs);

            dbContext.SaveChanges();

            _chartHandler.UpdateChartValues(score.GameID);
        }

        public void SaveClickPoint(int QuestionID, int x, int y)
        {
            //Nog even kijken hoe .... 

        }

        //Read

        public GameStatistic GetGameStatistic(int GameID)
        {
            Models.DataModels.GameStatistics Stats = dbContext.GameStatistics.Where(g => g.GameID == GameID).OrderByDescending(O=>O.ID).FirstOrDefault();
            return new GameStatistic()
            {
                AVG_Lifes = Stats.AVG_Lifes,
                AVG_NumOfBonus = Stats.AVG_NumOfBonus,
                AVG_Points = Stats.AVG_Points,
                AVG_TimePlayed = Stats.AVG_TimePlayed,
                NumOfUsers = Stats.NumOfUsers,
                Procent_Failed = Stats.ProcentFailed,
                game = _gameHandler.GetGame(GameID),
                Charts = _chartHandler.GetGameCharts(GameID)
            };
        }

        public List<GameStatistic> GetGameStatistics()
        {
            List<Game> games = _gameHandler.GetAllGames();

            List<GameStatistic> list = new List<GameStatistic>();

            foreach (Game game in games)
            {
                list.Add(GetGameStatistic(game.ID));
            }
            return list;
        }


        //Update

        //Delete

    }
}