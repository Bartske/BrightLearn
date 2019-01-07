using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using Models.Models.SatisticModels;

namespace Logic
{
    public class ChartHandler
    {
        QuestionHandler question_Handler;
        GameHandler _gameHandler;
        BrightLearnContext dbContext = new BrightLearnContext();

        public ChartHandler(bool test = false)
        {
            if (test)
            {
                question_Handler = new QuestionHandler(true);
                _gameHandler = new GameHandler(true);
            }
            else
            {
                question_Handler = new QuestionHandler();
                _gameHandler = new GameHandler();
            }
        }

        //Create
        public void CreateChart()
        {

        }

        //Read

        public List<ChartValue> GetChartValues(int ChartID, int GameID)
        {
            if (GameID == 0)
                throw new Exception("GameID kan niet 0 zijn!");
            
            if (dbContext.Chart.Where(c => c.ID == ChartID).Count() == 0)
                throw new Exception("Er is geen chart gevonden met het ID : `"+ ChartID.ToString() + "`");

            int ChartGroupID = _gameHandler.GetGame(GameID).ChartGroupID;


            List<ChartValue> list = new List<ChartValue>();

            foreach (Models.DataModels.ChartValues chartValue in dbContext.ChartValues.Where(cv => cv.ChartID == ChartID && cv.ChartGroupID == ChartGroupID).ToList())
            {
                string Label = chartValue.Label;
                string Value = chartValue.Value;

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

            if (dbContext.Chart.Where(c => c.Name == ChartName).Count() == 0)
                throw new Exception("Er is geen chart gevonden met de naam : `" + ChartName.ToString() + "`");

            _gameHandler.GetGame(GameID);


            return new ChartData()
            {
                ID = dbContext.Chart.Where(c => c.Name == ChartName).First().ID,
                Name = ChartName,
                Values = GetChartValues(dbContext.Chart.Where(c => c.Name == ChartName).First().ID, GameID)
            };
        }

        public List<ChartData> GetGameCharts(int GameID)
        {
            if (GameID == 0)
                throw new Exception("GameID kan niet 0 zijn!");
            
            _gameHandler.GetGame(GameID);


            List<ChartData> charts = new List<ChartData>();
            foreach (Models.DataModels.Chart chart in dbContext.Chart.ToList())
            {
                charts.Add(GetChart(chart.Name, GameID));
            }
            return charts;
        }

        //Update
        public void UpdateChartValues(int GameID)
        {
            if (GameID == 0)
                throw new Exception("GameID kan niet 0 zijn!");

            int ChartGroupID = _gameHandler.GetGame(GameID).ChartGroupID;

            //string DELETEQUERY = "DELETE FROM `chartvalues` WHERE `ChartGroupID` = '" + ChartGroupID.ToString() + "'";
            List<Models.DataModels.GameScore> gameScores = dbContext.GameScore.Where(g => g.ChartGroupID == ChartGroupID).ToList();
            //NumOfBonusses
            //string NumOfBonusQry = "INSERT INTO `chartvalues` (`ID`, `ChartID`, `ChartGroupID`, `Label`, `Value`) VALUES ";
            List<Models.DataModels.ChartValues> values = new List<Models.DataModels.ChartValues>();
            //SQL.Select("SELECT DISTINCT `NumOfBonus` FROM `gamescore` WHERE `ChartGroupID` = '"+ChartGroupID.ToString()+"'")
            foreach (int NumOfBonus in  gameScores.Select(g => g.NumOfBonus).Distinct())
            {
                //NumOfBonusQry += "(NULL, '1', '" +ChartGroupID.ToString()+"', '"+NumOfBonus+ "', (SELECT COUNT(`NumOfBonus`) FROM `gamescore` WHERE `ChartGroupID` = '"+ChartGroupID.ToString()+ "' AND `NumOfBonus` = '" + NumOfBonus + "')),";
                values.Add( new Models.DataModels.ChartValues()
                {
                    ChartID = 1,
                    ChartGroupID = ChartGroupID,
                    Label = NumOfBonus.ToString(),
                    Value = Convert.ToString(dbContext.GameScore.Where(gs => gs.ChartGroupID == ChartGroupID && gs.NumOfBonus == NumOfBonus).Count())
                });
            }

            //Passed
            /*
            string PassedQry = "INSERT INTO `chartvalues` (`ID`, `ChartID`, `ChartGroupID`, `Label`, `Value`) VALUES ";
            string _passed = ""; //SQL.Select("SELECT DISTINCT `Passed` FROM `gamescore` WHERE `ChartGroupID` = '" + ChartGroupID.ToString() + "'")
            foreach (string Passed in new List<string>() {"1","0" } )
            {
                if (Passed == "True" || Passed == "0") { _passed = "0"; }
                if (Passed == "False" || Passed == "1") { _passed = "1"; }
                PassedQry += "(NULL, '2', '" + ChartGroupID.ToString() + "', '" + _passed + "', (SELECT COUNT(`Passed`) FROM `gamescore` WHERE `ChartGroupID` = '" + ChartGroupID.ToString() + "' AND `Passed` = '" + _passed + "')),";
            }
            PassedQry = PassedQry.Remove(PassedQry.Length - 1);
            */
            foreach (int Passed in new List<int>() { 1, 0})
            {
                bool _passed = false;

                if (Passed == 0)
                    _passed = true;

                values.Add(new Models.DataModels.ChartValues()
                {
                    ChartID = 2,
                    ChartGroupID = ChartGroupID,
                    Label = Passed.ToString(),
                    Value = Convert.ToString(dbContext.GameScore.Where(gs => gs.ChartGroupID == ChartGroupID && gs.Passed == _passed).Count())
                });
            }


            //Lifes
            /*
            string LifesQry = "INSERT INTO `chartvalues` (`ID`, `ChartID`, `ChartGroupID`, `Label`, `Value`) VALUES ";
            foreach (string Lifes in SQL.Select("SELECT DISTINCT `Lifes` FROM `gamescore` WHERE `ChartGroupID` = '" + ChartGroupID.ToString() + "'"))
            {
                LifesQry += "(NULL, '3', '" + ChartGroupID.ToString() + "', '" + Lifes + "', (SELECT COUNT(`Lifes`) FROM `gamescore` WHERE `ChartGroupID` = '" + ChartGroupID.ToString() + "' AND `Lifes` = '" + Lifes + "' )),";
            }
            LifesQry = LifesQry.Remove(LifesQry.Length - 1);
            */

            foreach (int Lifes in gameScores.Select(g => g.Lifes).Distinct())
            {
                values.Add(new Models.DataModels.ChartValues()
                {
                    ChartID = 3,
                    ChartGroupID = ChartGroupID,
                    Label = Lifes.ToString(),
                    Value = Convert.ToString(dbContext.GameScore.Where(gs => gs.ChartGroupID == ChartGroupID && gs.Lifes == Lifes).Count())
                });
            }


            //Points
            /*
            string PointsQry = "INSERT INTO `chartvalues` (`ID`, `ChartID`, `ChartGroupID`, `Label`, `Value`) VALUES ";
            foreach (string Lifes in SQL.Select("SELECT DISTINCT `Points` FROM `gamescore` WHERE `ChartGroupID` = '" + ChartGroupID.ToString() + "'"))
            {
                PointsQry += "(NULL, '4', '" + ChartGroupID.ToString() + "', '" + Lifes + "', (SELECT COUNT(`Points`) FROM `gamescore` WHERE `ChartGroupID` = '" + ChartGroupID.ToString() + "' AND `Points` = '" + Lifes + "')),";
            }
            PointsQry = PointsQry.Remove(PointsQry.Length - 1);
            */
            foreach (int Points in gameScores.Select(g => g.Points).Distinct())
            {
                values.Add(new Models.DataModels.ChartValues()
                {
                    ChartID = 4,
                    ChartGroupID = ChartGroupID,
                    Label = Points.ToString(),
                    Value = Convert.ToString(dbContext.GameScore.Where(gs => gs.ChartGroupID == ChartGroupID && gs.Points == Points).Count())
                });
            }


            //Time
            /*
            string TimeQry = "INSERT INTO `chartvalues` (`ID`, `ChartID`, `ChartGroupID`, `Label`, `Value`) VALUES ";
            foreach (string Lifes in SQL.Select("SELECT DISTINCT `TimePlayed` FROM `gamescore` WHERE `ChartGroupID` = '" + ChartGroupID.ToString() + "'"))
            {
                TimeQry += "(NULL, '5', '" + ChartGroupID.ToString() + "', '" + Lifes + "', (SELECT COUNT(`TimePlayed`) FROM `gamescore` WHERE `ChartGroupID` = '" + ChartGroupID.ToString() + "'  AND `TimePlayed` = '" + Lifes + "')),";
            }
            TimeQry = TimeQry.Remove(TimeQry.Length - 1);
            */
            foreach (int Time in gameScores.Select(g => g.TimePlayed).Distinct())
            {
                values.Add(new Models.DataModels.ChartValues()
                {
                    ChartID = 5,
                    ChartGroupID = ChartGroupID,
                    Label = Time.ToString(),
                    Value = Convert.ToString(dbContext.GameScore.Where(gs => gs.ChartGroupID == ChartGroupID && gs.TimePlayed == Time).Count())
                });
            }


            dbContext.ChartValues.RemoveRange(dbContext.ChartValues.Where(cv => cv.ChartGroupID == ChartGroupID));
            dbContext.SaveChanges();

            dbContext.ChartValues.AddRange(values);
            dbContext.SaveChanges();

            float l = (gameScores.Where(gs => gs.Passed == false).Count());
            float r = (gameScores.Count());

            float ProcentFailed = (l / r) * 100;

            if (dbContext.GameStatistics.Where(gs=>gs.GameID == GameID).Count() == 0)
            {
                dbContext.GameStatistics.Add(new Models.DataModels.GameStatistics() { GameID = GameID, AVG_Lifes = 1, AVG_NumOfBonus = 1, AVG_Points = 1, NumOfUsers = 1, AVG_TimePlayed = 1, ProcentFailed = 0});
                dbContext.SaveChanges();
            }

            string command = String.Format("UPDATE gamestatistics SET NumOfUsers = ( SELECT COUNT(DISTINCT userID) FROM gamescore WHERE ChartGroupID = {0}),AVG_TimePlayed =( SELECT FLOOR(AVG(TimePlayed)) FROM gamescore WHERE ChartGroupID = {0}),AVG_Points =( SELECT FLOOR(AVG(Points)) FROM gamescore WHERE ChartGroupID = {0}),AVG_Lifes =( SELECT FLOOR(AVG(Lifes)) FROM gamescore WHERE ChartGroupID = {0}),AVG_NumOfBonus =( SELECT FLOOR(AVG(NumOfBonus)) FROM gamescore WHERE ChartGroupID = {0}),ProcentFailed ={1} WHERE GameID = {2}", ChartGroupID, ProcentFailed, GameID);
            dbContext.Database.ExecuteSqlCommand(command);
            dbContext.SaveChanges();
            /*
            SQL.Delete(DELETEQUERY);

            SQL.Update(NumOfBonusQry);
            SQL.Update(PassedQry);
            SQL.Update(LifesQry);
            SQL.Update(PointsQry);
            SQL.Update(TimeQry);
            */
        }

        //Delete
    }
}