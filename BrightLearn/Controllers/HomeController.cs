using Logic;
using BrightLearn.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrightLearn.Controllers
{
    public class HomeController : Controller
    {
        private bool CheckLoggedIn()
        {
            return (System.Web.HttpContext.Current.Session["UserID"] == null);
        }

        UserHandler _userHandler = new UserHandler();
        HighscoreHandler _highscoreHandler = new HighscoreHandler();
        GameHandler _gameHandler = new GameHandler();
        LoginHandler login_Handler = new LoginHandler();

        // GET: Home
        public ActionResult Index()
        {
            if (CheckLoggedIn())
                return RedirectToAction("Index", "Login");

            IndexViewModel model = new IndexViewModel()
            {
                Games = _gameHandler.GetAllGames()
            };

            return View(model);
        }

        public ActionResult HighScore()
        {
            if (CheckLoggedIn())
                return RedirectToAction("Index", "Login");

            HighscoreViewModel model = new HighscoreViewModel()
            {
                gameHighscores = _highscoreHandler.GetAllGameHighscores()
            };

            return View(model);
        }

        public ActionResult Account()
        {
            if (CheckLoggedIn())
                return RedirectToAction("Index", "Login");

            AccountViewModel model = new AccountViewModel()
            {
                user = _userHandler.GetCurrentUser()
            };

            return View(model);
        }

        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult AccountWithModel(AccountViewModel model)
        {
            if (CheckLoggedIn())
                return RedirectToAction("Index", "Login");

            return View("Account", model);
        }

        [HttpPost]
        public ActionResult UpdateAccount(AccountViewModel model)
        {
            if (CheckLoggedIn())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                try
                {
                    _userHandler.UpdateUser(model.user);
                }
                catch (Exception e)
                {
                    model.ValidationErrorMessage = e.Message;
                }
            }
            
            return AccountWithModel(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(PasswordViewModel PasswordModel)
        {
            AccountViewModel model = new AccountViewModel();
            model.user = _userHandler.GetCurrentUser();

            if (ModelState.IsValid)
            {
                PasswordModel.ID = System.Web.HttpContext.Current.Session["UserID"].ToString();

                try
                {
                    _userHandler.UpdatePassword(PasswordModel.currentPassword, PasswordModel.newPassword, PasswordModel.repeatNewPassword, Convert.ToInt32(PasswordModel.ID));
                }
                catch (Exception e)
                {
                    model.passwordViewModel.ValidationErrorMessage = "";
                    model.passwordViewModel.ValidationErrorMessage = e.Message;
                }

                PasswordModel.currentPassword = "";
                PasswordModel.newPassword = "";
                PasswordModel.repeatNewPassword = "";

                model.passwordViewModel = PasswordModel;
                return AccountWithModel(model);
            }
            else
            {
                PasswordModel.currentPassword = "";
                PasswordModel.newPassword = "";
                PasswordModel.repeatNewPassword = "";

                model.passwordViewModel = PasswordModel;
                return AccountWithModel(model);
            }
        }
    }
}