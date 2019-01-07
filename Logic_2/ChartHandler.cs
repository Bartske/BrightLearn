using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using Models.SatisticModels;

namespace Logic
{
    public class ChartHandler
    {
        QuestionHandler _questionHandler;
        GameHandler _gameHandler;
        BrightLearnContext _dbContext = new BrightLearnContext();

        public ChartHandler(bool test = false)
        {
            if (test)
            {
                _questionHandler = new QuestionHandler(true);
                _gameHandler = new GameHandler(true);
            }
            else
            {
                _questionHandler = new QuestionHandler();
                _gameHandler = new GameHandler();
            }
        }

        //Create

        //Read

        public List<ChartValue> GetChartValues(int ChartID, int GameID)
        {
            if (GameID == 0)
                throw new Exception("GameID kan niet 0 zijn!");
            
            if (_dbContext.Chart.Where(c => c.ID == ChartID).Count() == 0)
                throw new Exception("Er is geen chart gevonden met het ID : `"+ ChartID.ToString() + "`");

            int ChartGroupID = _gameHandler.GetGame(GameID).ChartGroupID;


            List<ChartValue> list = new List<ChartValue>();

            foreach (Models.DataModels.ChartValues ChartValue in _dbContext.ChartValues.Where(cv => cv.ChartID == ChartID && cv.ChartGroupID == ChartGroupID).ToList())
            {
                string Label = ChartValue.Label;
                string Value = ChartValue.Value;

                if (ChartID == 2)
                {
                    if (Label == "1")
                        Label = "Niet behaald";
                    if (Label == "0")
                        Label = "Behaald";
                }

                list.Add(new ChartValue()
                {
                    Label = Label,
                    Value = Value
                });
            }
            
            return list;
        }

        public ChartData GetChart(string ChartName, int GameID)
        {
            if (GameID == 0)
                throw new Exception("GameID kan niet 0 zijn!");

            if (_dbContext.Chart.Where(c => c.Name == ChartName).Count() == 0)
                throw new Exception("Er is geen chart gevonden met de naam : `" + ChartName.ToString() + "`");

            _gameHandler.GetGame(GameID);


            return new ChartData()
            {
                ID = _dbContext.Chart.Where(c => c.Name == ChartName).First().ID,
                Name = ChartName,
                Values = GetChartValues(_dbContext.Chart.Where(c => c.Name == ChartName).First().ID, GameID)
            };
        }

        public List<ChartData> GetGameCharts(int GameID)
        {
            if (GameID == 0)
                throw new Exception("GameID kan niet 0 zijn!");
            
            _gameHandler.GetGame(GameID);


            List<ChartData> Charts = new List<ChartData>();
            foreach (Models.DataModels.Chart Chart in _dbContext.Chart.ToList())
            {
                Charts.Add(GetChart(Chart.Name, GameID));
            }
            return Charts;
        }

        //Update
        public void UpdateChartValues(int GameID)
        {
            if (GameID == 0)
                throw new Exception("GameID kan niet 0 zijn!");

            int ChartGroupID = _gameHandler.GetGame(GameID).ChartGroupID;

            List<Models.DataModels.GameScore> GameScores = _dbContext.GameScore.Where(g => g.ChartGroupID == ChartGroupID).ToList();
            List<Models.DataModels.ChartValues> ChartValues = new List<Models.DataModels.ChartValues>();


            foreach (int NumOfBonus in  GameScores.Select(g => g.NumOfBonus).Distinct())
            {
                ChartValues.Add( new Models.DataModels.ChartValues()
                {
                    ChartID = 1,
                    ChartGroupID = ChartGroupID,
                    Label = NumOfBonus.ToString(),
                    Value = Convert.ToString(_dbContext.GameScore.Where(gs => gs.ChartGroupID == ChartGroupID && gs.NumOfBonus == NumOfBonus).Count())
                });
            }
            
            foreach (int Passed in new List<int>() { 1, 0})
            {
                bool _Passed = false;

                if (Passed == 0)
                    _Passed = true;

                ChartValues.Add(new Models.DataModels.ChartValues()
                {
                    ChartID = 2,
                    ChartGroupID = ChartGroupID,
                    Label = Passed.ToString(),
                    Value = Convert.ToString(_dbContext.GameScore.Where(gs => gs.ChartGroupID == ChartGroupID && gs.Passed == _Passed).Count())
                });
            }
            
            foreach (int Lifes in GameScores.Select(g => g.Lifes).Distinct())
            {
                ChartValues.Add(new Models.DataModels.ChartValues()
                {
                    ChartID = 3,
                    ChartGroupID = ChartGroupID,
                    Label = Lifes.ToString(),
                    Value = Convert.ToString(_dbContext.GameScore.Where(gs => gs.ChartGroupID == ChartGroupID && gs.Lifes == Lifes).Count())
                });
            }
            
            foreach (int Points in GameScores.Select(g => g.Points).Distinct())
            {
                ChartValues.Add(new Models.DataModels.ChartValues()
                {
                    ChartID = 4,
                    ChartGroupID = ChartGroupID,
                    Label = Points.ToString(),
                    Value = Convert.ToString(_dbContext.GameScore.Where(gs => gs.ChartGroupID == ChartGroupID && gs.Points == Points).Count())
                });
            }
            
            foreach (int Time in GameScores.Select(g => g.TimePlayed).Distinct())
            {
                ChartValues.Add(new Models.DataModels.ChartValues()
                {
                    ChartID = 5,
                    ChartGroupID = ChartGroupID,
                    Label = Time.ToString(),
                    Value = Convert.ToString(_dbContext.GameScore.Where(gs => gs.ChartGroupID == ChartGroupID && gs.TimePlayed == Time).Count())
                });
            }


            _dbContext.ChartValues.RemoveRange(_dbContext.ChartValues.Where(cv => cv.ChartGroupID == ChartGroupID));
            _dbContext.SaveChanges();

            _dbContext.ChartValues.AddRange(ChartValues);
            _dbContext.SaveChanges();

            float Numerator = (GameScores.Where(gs => gs.Passed == false).Count());
            float Denumerator = (GameScores.Count());

            float ProcentFailed = (Numerator / Denumerator) * 100;

            if (_dbContext.GameStatistics.Where(gs=>gs.GameID == GameID).Count() == 0)
            {
                _dbContext.GameStatistics.Add(new Models.DataModels.GameStatistics() { GameID = GameID, AVGLifes = 1, AVGNumOfBonus = 1, AVGPoints = 1, NumOfUsers = 1, AVGTimePlayed = 1, ProcentFailed = 0});
                _dbContext.SaveChanges();
            }

            string SQLCommand = String.Format("UPDATE gamestatistics SET NumOfUsers = ( SELECT COUNT(DISTINCT userID) FROM gamescore WHERE ChartGroupID = {0}),AVG_TimePlayed =( SELECT FLOOR(AVG(TimePlayed)) FROM gamescore WHERE ChartGroupID = {0}),AVG_Points =( SELECT FLOOR(AVG(Points)) FROM gamescore WHERE ChartGroupID = {0}),AVG_Lifes =( SELECT FLOOR(AVG(Lifes)) FROM gamescore WHERE ChartGroupID = {0}),AVG_NumOfBonus =( SELECT FLOOR(AVG(NumOfBonus)) FROM gamescore WHERE ChartGroupID = {0}),ProcentFailed ={1} WHERE GameID = {2}", ChartGroupID, ProcentFailed, GameID);
            _dbContext.Database.ExecuteSqlCommand(SQLCommand);
            _dbContext.SaveChanges();
        }

        //Delete
    }
}