using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Logic;
using Models;
using Models.QuestionModels;

namespace BrightLearn.Tests.HandlerTests
{
    [TestClass]
    public class GameHandlerTests
    {
        GameHandler handler = new GameHandler(true);

        Game ExistingGame = new Game()
        {
            ID = 1,
            Name = "Test",
            Lifes = 2,
            BonusTime = 3,
            ImageURL = null,
            Online = false,
            ChartGroupID = 1,
            Questions = new List<Question>()
            {
                new Question (){
                    QuestionID = 1,
                    ImageQuestion = new Models.QuestionModels.ImageQuestion()
                    {
                        ID = 1,
                        Question = "Dit is een test vraag",
                        X1 = 1,
                        X2 = 8,
                        Y1 = 1,
                        Y2 = 22,
                        Radius = 0,
                        IMG = "/Content/img/NoIMG.png"
                    },
                    MultipleChoiseQuestion = null,
                    Type = QuestionType.ImageQuestion
                },
                new Question(){
                    QuestionID = 2,
                    ImageQuestion = null,
                    Type = QuestionType.MultipleChoise,
                    MultipleChoiseQuestion = new MultipleChoiseQuestion()
                        {
                            ID = 1,
                            Question = "Dit is een test vraag",
                            Correctanswer = "Correct",
                            Answers = new List<string>()
                            {
                                "Fout",
                                "Correct",
                                "Niet deze",
                                "Ook niet deze"
                            }
                        }
    }
            }
        };


        Game NewGame = new Game()
        {
            Name = "NewGame",
            Lifes = 4,
            BonusTime = 1,
            ImageURL = "/Content/NoIMG.png",
            Online = false,
            ChartGroupID = 4,
            Questions = new List<Question>()
        };
        Game NewGame_Copy = new Game()
        {
            Name = "NewGame",
            Lifes = 4,
            BonusTime = 1,
            ImageURL = "/Content/NoIMG.png",
            Online = false,
            ChartGroupID = 4,
            Questions = new List<Question>()
        };


        private void CompareGames(Game a, Game b)
        {
            Assert.AreEqual(a.ID, b.ID);
            Assert.AreEqual(a.Name, b.Name);
            Assert.AreEqual(a.Lifes, b.Lifes);
            Assert.AreEqual(a.BonusTime, b.BonusTime);
            Assert.AreEqual(a.ImageURL, b.ImageURL);
            Assert.AreEqual(a.Online, b.Online);
            Assert.AreEqual(a.Questions.Count, b.Questions.Count);
        }
        

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen game gevonden met de ID : `999`")]
        public void Test_GetGame_WrongID()
        {
            handler.GetGame(999);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception), "De levens moet tussen de 1 en de 5 zijn!")]
        public void Test_CreateGame_Lifes_0()
        {
            NewGame = NewGame_Copy;
            NewGame.Lifes = 0;
            handler.CreateGame(NewGame, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "De levens moet tussen de 1 en de 5 zijn!")]
        public void Test_CreateGame_Lifes_6()
        {
            NewGame = NewGame_Copy;
            NewGame.Lifes = 6;
            handler.CreateGame(NewGame, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "De bonustijd moet tussen de 1 en de 10 zijn!")]
        public void Test_CreateGame_Bonus_0()
        {
            NewGame = NewGame_Copy;
            NewGame.BonusTime = 0;
            handler.CreateGame(NewGame, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "De bonustijd moet tussen de 1 en de 10 zijn!")]
        public void Test_CreateGame_Bonus_11()
        {
            NewGame = NewGame_Copy;
            NewGame.BonusTime = 11;
            handler.CreateGame(NewGame, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "De naam moet tussen de 3 en 25 characters bevatten!")]
        public void Test_CreateGame_Name_char_2()
        {
            NewGame = NewGame_Copy;
            NewGame.Name = "on";
            handler.CreateGame(NewGame, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "De naam moet tussen de 3 en 25 characters bevatten!")]
        public void Test_CreateGame_Name_char_26()
        {
            NewGame = NewGame_Copy;
            NewGame.Name = "123456789jwbdkajbwdjkawd487";
            handler.CreateGame(NewGame, null);
        }
        

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen game gevonden met het ID : `999`")]
        public void Test_UpdateOnlineStatus_WrongID()
        {
            handler.UpdateOnlineStatus(999, true);
        }

        [TestMethod]
        public void Test_DeleteGame_Normal()
        {
            NewGame = NewGame_Copy;
            handler.CreateGame(NewGame, null);
            int ID = handler.GetAllGames().Last().ID;

            handler.DeleteGame(ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen game gevonden met het ID : `999`")]
        public void Test_DeleteGame_WrongID()
        {
            handler.DeleteGame(999);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen game gevonden met het ID : `999`")]
        public void Test_UpdateValues_WrongID()
        {
            handler.UpdateValues("TestUpdate", 1, 1, 999);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "De levens moet tussen de 1 en de 5 zijn!")]
        public void Test_UpdateValues_Lifes_0()
        {
            handler.UpdateValues("TestUpdate", 0, 1, ExistingGame.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "De levens moet tussen de 1 en de 5 zijn!")]
        public void Test_UpdateValues_Lifes_6()
        {
            handler.UpdateValues("TestUpdate", 6, 1, ExistingGame.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "De bonustijd moet tussen de 1 en de 10 zijn!")]
        public void Test_UpdateValues_Bonus_0()
        {
            handler.UpdateValues("TestUpdate", 1, 0, ExistingGame.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "De bonustijd moet tussen de 1 en de 10 zijn!")]
        public void Test_UpdateValues_Bonus_11()
        {
            handler.UpdateValues("TestUpdate", 1, 11, ExistingGame.ID);
        }

        [ExpectedException(typeof(Exception), "De naam moet tussen de 3 en 25 characters bevatten!")]
        public void Test_UpdateValues_Name_Char_2()
        {
            handler.UpdateValues("2", 1, 8, ExistingGame.ID);
        }

        [ExpectedException(typeof(Exception), "De naam moet tussen de 3 en 25 characters bevatten!")]
        public void Test_UpdateValues_Name_Char_26()
        {
            handler.UpdateValues("123456789jwbdkajbwdjkawd487", 1, 8, ExistingGame.ID);
        }


    }
}
