using Logic;
using BrightLearn.ViewModels.Statistic;
using Models;
using Models.SatisticModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BrightLearn.Controllers
{
    public class StatisticController : Controller
    {
        StatisticHandler _statisticHandler = new StatisticHandler();
        GameHandler _gameHandler = new GameHandler();
        ChartHandler _chartHandler = new ChartHandler();
        
        [HttpPost]
        public void CreateGameScore(int GameID, int points, int time, int lifes, int NumOfBonus, bool passed)
        {
            GameScore score = new GameScore()
            {
                GameID = GameID,
                Lifes = lifes,
                NumOfBonus = NumOfBonus,
                Passed = passed,
                Points = points,
                TimePlayed = time
            };
            score.UserID = (int)System.Web.HttpContext.Current.Session["UserID"];
            _statisticHandler.Create(score);
        }

        public ActionResult Games()
        {
            StatisticGameViewModel model = new StatisticGameViewModel()
            {
                Games = _gameHandler.GetAllGames()
            };

            return View(model);
        }

        public ActionResult Questions(int GameID)
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetGameStats(int GameID)
        {
            return Json(_statisticHandler.GetGameStatistic(GameID));
        }

        public ActionResult GameStatistics(int GameID)
        {
            return PartialView("_GameStatistics", GetGameStats(GameID));
        }

        public ActionResult QuestionStatistics(int GameID)
        {
            //Statistieken ophalen
            StatisticQuestionsViewModel model = new StatisticQuestionsViewModel()
            {
                questions = _gameHandler.GetGame(GameID).Questions,
                game = _gameHandler.GetGame(GameID)
            };

            //View Teruggeven
            return View("Questions", model);
        }

    }
}