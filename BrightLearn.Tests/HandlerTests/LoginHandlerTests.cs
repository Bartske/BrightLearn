using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using Models;

namespace BrightLearn.Tests.HandlerTests
{
    [TestClass]
    public class LoginHandlerTests
    {
        LoginHandler handler = new LoginHandler();
        
        [TestMethod]
        [ExpectedException(typeof(Exception), "Username cannot be empty")]
        public void Test_Auth_NoUserName()
        {
            handler.Auth("", "Password");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Password cannot be empty")]
        public void Test_Auth_NoPassword()
        {
            handler.Auth("Username", "");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Username cannot be empty")]
        public void Test_Auth_NoValues()
        {
            handler.Auth("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Deze gebruikersnaam is niet bekend bij ons!")]
        public void Test_Auth_WrongUsername()
        {
            handler.Auth("Wrong", "Password");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Het wachtwoord is incorrect!")]
        public void Test_Auth_WrongPassword()
        {
            handler.Auth("Username", "Wrong");
        }


        [TestMethod]
        public void Test_IsValidPassword_Normal()
        {
            Assert.AreEqual(true,handler.IsValidPassword("BrightLearn1!"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Het wachtwoord moet minstens 1 kleine letter bevatten!")]
        public void Test_IsValidPassword_NoSmallLetter()
        {
            Assert.AreEqual(true, handler.IsValidPassword("BRIGHTLEARN1!"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Het wachtwoord moet minstens 1 hoofd letter bevatten!")]
        public void Test_IsValidPassword_NoCAPS()
        {
            Assert.AreEqual(true, handler.IsValidPassword("brightlearn11!"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Het wachtwoord moet minstens 1 cijfer bevatten!")]
        public void Test_IsValidPassword_NoNumber()
        {
            Assert.AreEqual(true, handler.IsValidPassword("BRIGHTLEARN!"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Het wachtwoord moet minstens 1 speciaal teken bevatten!")]
        public void Test_IsValidPassword_NoSpecialSign()
        {
            Assert.AreEqual(true, handler.IsValidPassword("BRIGHTLEARN1"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Het wachtwoord moet minimaal 8 tekens zijn!")]
        public void Test_IsValidPassword_NotLongEnough()
        {
            Assert.AreEqual(true, handler.IsValidPassword("Pass1!"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Het wachtwoord kan niet leeg zijn!")]
        public void Test_IsValidPassword_Empty()
        {
            Assert.AreEqual(true, handler.IsValidPassword(""));
        }
        

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen account met deze gebruikersnaam bij ons bekent.")]
        public void Test_NewPassword_WrongUsername()
        {
            handler.RecoverPassword("Wrong", "test@mail.nl");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen account met deze email bij ons bekent.")]
        public void Test_NewPassword_WrongEmail()
        {
            handler.RecoverPassword("Username", "Wrong");
        }
        
    }
}
