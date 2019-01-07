using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logic;
using Models;

namespace BrightLearn.Controllers
{
    public class LoginController : Controller
    {
        LoginHandler login_Handler = new LoginHandler();
        UserHandler _userHandler = new UserHandler();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ViewModels.Login.LoginDataModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);
            try
            {
                //Checking if user login data is correct
                if (login_Handler.Auth(model.UserName, model.Password))
                {
                    string Encrypted_Password = login_Handler.EncryptPass(model.Password, model.UserName);
                    //Getting the users information
                    User user = _userHandler.GetUser(model.UserName, Encrypted_Password);

                    //Filling the Session data
                    Session["UserID"] = user.ID;
                    Session["Type"] = user.AccountType;

                    if (user.MiddleName == "")
                        Session["fullName"] = user.FirstName + " " + user.LastName;
                    else
                        Session["fullName"] = user.FirstName + " " + user.MiddleName + " " + user.LastName;

                    //Redirect to the home screen
                    return RedirectToAction("Index", "Home");
                }
                return View("Index", model);
            }
            catch (Exception e)
            {
                model.LoginErrorMessage = e.Message;
                return View("Index", model);
            } 
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ViewModels.Login.ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    login_Handler.RecoverPassword(model.Username, model.Email);
                    return Index();
                }
                catch (Exception e)
                {
                    model.LoginErrorMessage = e.Message;
                }
            }
            return View(model);
        }
    }
}