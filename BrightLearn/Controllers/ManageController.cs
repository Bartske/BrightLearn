using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logic;
using BrightLearn.ViewModels.Manage;
using Models;
using Models;

namespace BrightLearn.Controllers
{
    public class ManageController : Controller
    {
        ManageHandler handler = new ManageHandler();
        GameHandler gameHandler = new GameHandler();
        QuestionHandler question_Handler = new QuestionHandler();
        UserHandler userHandler = new UserHandler();

        private bool CheckLoggedIn()
        {
            return (System.Web.HttpContext.Current.Session["UserID"] == null);
        }

        // GET: Manage
        public ActionResult Accounts()
        {
            if (CheckLoggedIn())
                return RedirectToAction("Index", "Login");

            AccountsViewModel model = new AccountsViewModel()
            {
                Accounts = userHandler.GetAllUsers()
            };

            if (model.Accounts.Count == 0)
                model.ErrorMessage = "Er zijn geen accounts gevonden!";

            return View(model);
        }

        public ActionResult Games()
        {
            if (CheckLoggedIn())
                return RedirectToAction("Index", "Login");

            GamesViewModel model = new GamesViewModel()
            {
                Games = gameHandler.GetAllGames()
            };
            
            return View(model);
        }

        public ActionResult Questions(int GameID)
        {
            if (CheckLoggedIn())
                return RedirectToAction("Index", "Login");

            QuestionsViewModel model = new QuestionsViewModel();

            model.game = gameHandler.GetGame(GameID);
            model.game.Questions = question_Handler.GetQuestions(GameID);

            return View("Questions", model);
        }

        public ActionResult CreateQuestion(int GameID)
        {
            if (CheckLoggedIn())
                return RedirectToAction("Index", "Login");

            AddQuestionViewModel model = new AddQuestionViewModel();
            model.Game = gameHandler.GetGame(GameID);
            model.Question = new Question();
            model.Question.ImageQuestion = new Models.QuestionModels.ImageQuestion();
            model.Question.MultipleChoiseQuestion = new Models.QuestionModels.MultipleChoiseQuestion();
            model.Question.MultipleChoiseQuestion.Answers = new List<string>();

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateImageQuestion(AddQuestionViewModel model, HttpPostedFileBase file)
        {
                question_Handler.CreateImageQuestion(model.Question.ImageQuestion, file, model.GameID);
            
            return RedirectToAction("Questions", new { model.GameID });
        }

        [HttpPost]
        public ActionResult CreateMultipleChoiseQuestion( string Question, string Correctanswer, string[] Answers, string GameID)
        {
            question_Handler.CreateMultipleChoiseQuestion(new Models.QuestionModels.MultipleChoiseQuestion() {Question = Question, Correctanswer = Correctanswer, Answers = Answers.ToList()}, Convert.ToInt32(GameID));

            return RedirectToAction("Questions", new { GameID = Convert.ToInt32(GameID) });
        }

        public ActionResult CreateGame()
        {
            if (CheckLoggedIn())
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public ActionResult CreateGame(CreateGameViewModel model, HttpPostedFileBase file)
        {
            gameHandler.CreateGame(model.game, file);
            return RedirectToAction("Games");
        }


        public ActionResult EditQuestion(int QuestionID, int GameID)
        {
            if (CheckLoggedIn())
                return RedirectToAction("Index", "Login");

            EditQuestionViewModel model = new EditQuestionViewModel();

            model.Question = question_Handler.GetQuestions(GameID).Where(q => q.QuestionID == QuestionID).Single();

            if (model.Question.Type == QuestionType.MultipleChoise)
                model.Question.MultipleChoiseQuestion.Answers.Remove(model.Question.MultipleChoiseQuestion.Correctanswer);

            model.Game = gameHandler.GetGame(GameID);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditMultipleChoiseQuestion(int ID, string Question, string Correctanswer, string[] Answers, string GameID)
        {
            question_Handler.UpdateMultipleChoiseQuestion(new Models.QuestionModels.MultipleChoiseQuestion() { ID = ID, Question = Question, Correctanswer = Correctanswer, Answers = Answers.ToList() });
            return RedirectToAction("Questions", new { GameID = Convert.ToInt32(GameID) });
        }

        [HttpPost]
        public ActionResult EditImageQuestion(AddQuestionViewModel model, HttpPostedFileBase file)
        {
            question_Handler.UpdateImageQuestion(model.Question.ImageQuestion, file);

            return RedirectToAction("Questions", new { model.GameID });
        }


        public void UpdateUserType(int ID, string type)
        {
            handler.UpdateUserType(ID, type);
        }

        public void UpdateGameOnlineStatus(int ID, string status)
        {
            if (status == "true")
            {
                gameHandler.UpdateOnlineStatus(ID, true);
            }
            else
            {
                gameHandler.UpdateOnlineStatus(ID, false);
            }
        }

        public ActionResult DeleteQuestion(int QuestionID, int GameID)
        {
            if (CheckLoggedIn())
                return RedirectToAction("Index", "Login");

            question_Handler.DeleteQuestion(QuestionID, GameID);
            return Questions(GameID);
        }

        public ActionResult DeleteGame(int GameID)
        {
            gameHandler.DeleteGame(GameID);

            return RedirectToAction("Games");
        }

        public void SaveGameValues(string Name, int Lifes, int Bonus, int GameID)
        {
            gameHandler.UpdateValues(Name, Lifes, Bonus, GameID);
        }

        [HttpPost]
        public JsonResult GenerateUser()
        {
            return Json (handler.GenerateUser(), JsonRequestBehavior.AllowGet);
        }
    }
}