using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logic;

namespace BrightLearn.Controllers
{
    public class HighScoreController : Controller
    {
        HighscoreHandler _highscoreHandler = new HighscoreHandler();
        
        [HttpPost]
        public void CreateHighScore(int GameID, int points, int time)
        {
            int userID = (int)System.Web.HttpContext.Current.Session["UserID"];
            _highscoreHandler.CreateHighScore(GameID, userID, points, time);
        }
    }
}