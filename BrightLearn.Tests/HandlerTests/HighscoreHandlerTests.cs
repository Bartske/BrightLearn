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
    public class HighscoreHandlerTests
    {
        HighscoreHandler handler = new HighscoreHandler(true);

        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that game!")]
        public void test_GetGameHighScore_ID_NoRecord()
        {
            Models.GameHighscore testGame = handler.GetGameHighscore(0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that game!")]
        public void test_GetGameHighScore_Name_NoRecord()
        {
            Models.GameHighscore testGame = handler.GetGameHighscore("wrong");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Name cannot be empty!")]
        public void test_GetGameHighScore_Name_NoName()
        {
            Models.GameHighscore testGame = handler.GetGameHighscore("");
        }

        Models.GameHighscore CorrectGameHighscore = new Models.GameHighscore()
        {
            GameID = 20,
            GameName = "test",
            Name = new List<string>() { "Bert van de Ven" },
            points = new List<string>() { "520" }
        };
        
        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that game!")]
        public void test_CreateHighScore_NoGame()
        {
            handler.CreateHighScore(8000, 2, 15, 20);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that user!")]
        public void test_CreateHighScore_NoUser()
        {
            handler.CreateHighScore(1, 999, 15, 20);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "The ID of the game cannot be 0")]
        public void test_CreateHighScore_Game_0()
        {
            handler.CreateHighScore(0, 2, 15, 20);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "The ID of the user cannot be 0")]
        public void test_CreateHighScore_User_0()
        {
            handler.CreateHighScore(1, 0, 15, 20);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "Time cannot be 0")]
        public void test_CreateHighScore_Time_0()
        {
            handler.CreateHighScore(1, 1, 15, 0);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "Points cannot be 0")]
        public void test_CreateHighScore_Points_0()
        {
            handler.CreateHighScore(1, 1, 0, 11);
        }



        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that game!")]
        public void test_UpdateScore_NoGame()
        {
            handler.UpdateScore(8000, 2, 15, 20);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that user!")]
        public void test_UpdateScore_NoUser()
        {
            handler.UpdateScore(1, 999, 15, 20);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "The ID of the game cannot be 0")]
        public void test_UpdateScore_Game_0()
        {
            handler.UpdateScore(0, 2, 15, 20);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "The ID of the user cannot be 0")]
        public void test_UpdateScore_User_0()
        {
            handler.UpdateScore(1, 0, 15, 20);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "Time cannot be 0")]
        public void test_UpdateScore_Time_0()
        {
            handler.UpdateScore(1, 1, 15, 0);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "Points cannot be 0")]
        public void test_UpdateScore_Points_0()
        {
            handler.UpdateScore(1, 1, 0, 11);
        }

    }
}
