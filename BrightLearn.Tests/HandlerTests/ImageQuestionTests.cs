using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using Models;
using Models.QuestionModels;

namespace BrightLearn.Tests.HandlerTests
{
    [TestClass]
    public class ImageQuestionTests
    {
        QuestionHandler handler = new QuestionHandler(true);

        Stream test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("whatever"));
        TestObjects.MyTestPostedFileBase _postedFileBase;
        int GameID = 1;

        ImageQuestion NewImageQuestion = new ImageQuestion()
        {
            Question = "Dit is een Test vraag",
            X1 = 1,
            X2 = 5,
            Y1 = 1,
            Y2 = 5
        };

        ImageQuestion NewImageQuestion_Copy = new ImageQuestion()
        {
            Question = "Dit is een Test vraag",
            X1 = 1,
            X2 = 5,
            Y1 = 1,
            Y2 = 5
        };

        ImageQuestion ExistingImageQuestion = new ImageQuestion()
        {
            ID = 1,
            Question = "Dit is een test vraag",
            X1 = 1,
            X2 = 8,
            Y1 = 1,
            Y2 = 22,
            Radius = 0,
            IMG = "/Content/img/NoIMG.png"
        };

        ImageQuestion ExistingImageQuestion_copy = new ImageQuestion()
        {
            ID = 1,
            Question = "Dit is een test vraag",
            X1 = 1,
            X2 = 8,
            Y1 = 1,
            Y2 = 22,
            Radius = 0,
            IMG = "/Content/img/NoIMG.png"
        };

        private void CompareImageQuestions(ImageQuestion a, ImageQuestion b)
        {
            Assert.AreEqual(a.IMG, b.IMG);
            Assert.AreEqual(a.Question, b.Question);
            Assert.AreEqual(a.X1, b.X1);
            Assert.AreEqual(a.X2, b.X2);
            Assert.AreEqual(a.Y1, b.Y1);
            Assert.AreEqual(a.Y2, b.Y2);
            Assert.AreEqual(a.Radius, b.Radius);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Een vraag moet tussen de 5 en 50 characters bevatten!")]
        public void Test_CreateImageQuestion_Question_Char_4()
        {
            NewImageQuestion = NewImageQuestion_Copy;
            NewImageQuestion.Question = "1234";
            _postedFileBase = new TestObjects.MyTestPostedFileBase(test_Stream, "test/content", "test-file.png");
            handler.CreateImageQuestion(NewImageQuestion, _postedFileBase, GameID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Een vraag moet tussen de 5 en 50 characters bevatten!")]
        public void Test_CreateImageQuestion_Question_Char_51()
        {
            NewImageQuestion = NewImageQuestion_Copy;
            NewImageQuestion.Question = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            _postedFileBase = new TestObjects.MyTestPostedFileBase(test_Stream, "test/content", "test-file.png");
            handler.CreateImageQuestion(NewImageQuestion, _postedFileBase, GameID);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception), "Een vraag moet uit meer dan 3 woorden bestaan!")]
        public void Test_CreateImageQuestion_Question_2_Words()
        {
            NewImageQuestion = NewImageQuestion_Copy;
            NewImageQuestion.Question = "wdjkawdwd dwadwadw";
            _postedFileBase = new TestObjects.MyTestPostedFileBase(test_Stream, "test/content", "test-file.png");
            handler.CreateImageQuestion(NewImageQuestion, _postedFileBase, GameID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er moet een antwoord gegeven worden!")]
        public void Test_CreateImageQuestion_Question_X1_0()
        {
            NewImageQuestion = NewImageQuestion_Copy;
            NewImageQuestion.X1 = 0;
            _postedFileBase = new TestObjects.MyTestPostedFileBase(test_Stream, "test/content", "test-file.png");
            handler.CreateImageQuestion(NewImageQuestion, _postedFileBase, GameID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er moet een antwoord gegeven worden!")]
        public void Test_CreateImageQuestion_Question_Y1_0()
        {
            NewImageQuestion = NewImageQuestion_Copy;
            NewImageQuestion.Y1 = 0;
            _postedFileBase = new TestObjects.MyTestPostedFileBase(test_Stream, "test/content", "test-file.png");
            handler.CreateImageQuestion(NewImageQuestion, _postedFileBase, GameID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Je moet een afbeelding uploaden!")]
        public void Test_CreateImageQuestion_Question_IMG_NULL()
        {
            NewImageQuestion = NewImageQuestion_Copy;
            handler.CreateImageQuestion(NewImageQuestion, null, GameID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen game gevonden met de ID : `999`")]
        public void Test_CreateImageQuestion_GameID_Wrong()
        {
            NewImageQuestion = NewImageQuestion_Copy;
            handler.CreateImageQuestion(NewImageQuestion, null, 999);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen vraag gevonden met het ID : `999`")]
        public void Test_GetImageQuestion_WrongID()
        {
            handler.GetImageQuestion(999);
        }



        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen vraag gevonden met het ID : `999`")]
        public void Test_UpdateImageQuestion_Question_WrongID()
        {
            ExistingImageQuestion = ExistingImageQuestion_copy;
            ExistingImageQuestion.ID = 999;
            _postedFileBase = new TestObjects.MyTestPostedFileBase(test_Stream, "test/content", "test-file.png");
            handler.UpdateImageQuestion(ExistingImageQuestion, _postedFileBase);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Een vraag moet tussen de 5 en 50 characters bevatten!")]
        public void Test_UpdateImageQuestion_Question_Char_4()
        {
            ExistingImageQuestion = ExistingImageQuestion_copy;
            ExistingImageQuestion.Question = "1234";
            _postedFileBase = new TestObjects.MyTestPostedFileBase(test_Stream, "test/content", "test-file.png");
            handler.UpdateImageQuestion(ExistingImageQuestion, _postedFileBase);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Een vraag moet tussen de 5 en 50 characters bevatten!")]
        public void Test_UpdateImageQuestion_Question_Char_51()
        {
            ExistingImageQuestion = ExistingImageQuestion_copy;
            ExistingImageQuestion.Question = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            _postedFileBase = new TestObjects.MyTestPostedFileBase(test_Stream, "test/content", "test-file.png");
            handler.UpdateImageQuestion(ExistingImageQuestion, _postedFileBase);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception), "Een vraag moet uit meer dan 3 woorden bestaan!")]
        public void Test_UpdateImageQuestion_Question_2_Words()
        {
            ExistingImageQuestion = ExistingImageQuestion_copy;
            ExistingImageQuestion.Question = "wdjkawdwd dwadwadw";
            _postedFileBase = new TestObjects.MyTestPostedFileBase(test_Stream, "test/content", "test-file.png");
            handler.UpdateImageQuestion(ExistingImageQuestion, _postedFileBase);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er moet een antwoord gegeven worden!")]
        public void Test_UpdateImageQuestion_Question_X1_0()
        {
            ExistingImageQuestion = ExistingImageQuestion_copy;
            ExistingImageQuestion.X1 = 0;
            _postedFileBase = new TestObjects.MyTestPostedFileBase(test_Stream, "test/content", "test-file.png");
            handler.UpdateImageQuestion(ExistingImageQuestion, _postedFileBase);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er moet een antwoord gegeven worden!")]
        public void Test_UpdateImageQuestion_Question_Y1_0()
        {
            ExistingImageQuestion = ExistingImageQuestion_copy;
            ExistingImageQuestion.Y1 = 0;
            _postedFileBase = new TestObjects.MyTestPostedFileBase(test_Stream, "test/content", "test-file.png");
            handler.UpdateImageQuestion(ExistingImageQuestion, _postedFileBase);
        }




        [TestMethod]
        public void Test_DeleteImageQuestion_Normal()
        {
            //Als er een kan aangemaakt worden dan kan deze funtie getest worden
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen vraag gevonden met het ID : `999`")]
        public void Test_DeleteImageQuestion_WrongID()
        {
            handler.DeleteImageQuestion(999);
        }

    }
}
