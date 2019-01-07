using Models;
using Models.QuestionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logic;

namespace BrightLearn.Controllers
{
    public class GameController : Controller
    {
        private void isLoggedIn()
        {
            if (Session["Type"] == null)
            {
                Response.Redirect("~/Login/");
            }
        }
        
        GameHandler gameHandler = new GameHandler();
        QuestionHandler questionHandler = new QuestionHandler();

        // GET: Game
        public ActionResult Index(string ID)
        {
            isLoggedIn();
            BrightLearn.ViewModels.Game.IndexViewModel model = new BrightLearn.ViewModels.Game.IndexViewModel();
            Game game = gameHandler.GetGame(ID);

            model.GameID = game.ID;
            model.Lifes = game.Lifes;
            model.bonusTime = game.BonusTime;
            model.Questions = questionHandler.GetQuestions(game.ID);

            if (model.Questions.Count == 0)
            {
                //Display error that there are no questions
            }

            return View(model);
        }
        
        private ImageQuestion GetImageQuestion(int ID)
        {
            return questionHandler.GetImageQuestion(ID);

        }

        private MultipleChoiseQuestion GetMultipleChoiseQuestion(int ID)
        {
            return questionHandler.GetMultipleChoiseQuestion(ID);
        }
    }
}