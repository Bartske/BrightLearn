using Logic;
using Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.QuestionModels;

namespace BrightLearn.Tests.HandlerTests
{
    [TestClass]
    public class MulitplechoiseQuestionTests
    {
        QuestionHandler handler = new QuestionHandler(true);
        int GameID = 1;

        MultipleChoiseQuestion updateQuestion = new MultipleChoiseQuestion()
        {
            ID = 1,
            Question = "Dit is een geupdate vraag",
            Correctanswer = "3",
            Answers = new List<string>()
            {
                "1",
                "2",
                "4"
            }
        };

        MultipleChoiseQuestion existingQuestion = new MultipleChoiseQuestion()
        {
            ID = 1,
            Question = "Dit is een test vraag",
            Correctanswer = "Correct",
            Answers = new List<string>()
            {
                "Fout",
                "Niet deze",
                "Ook niet deze"
            }
        };

        MultipleChoiseQuestion existingQuestion_copy = new MultipleChoiseQuestion()
        {
            ID = 1,
            Question = "Dit is een test vraag",
            Correctanswer = "Correct",
            Answers = new List<string>()
            {
                "Fout",
                "Niet deze",
                "Ook niet deze"
            }
        };

        MultipleChoiseQuestion newQuestion = new MultipleChoiseQuestion()
        {
            Question = "Dit is een nieuwe test vraag",
            Correctanswer = "AA",
            Answers = new List<string>()
            {
                "dd",
                "cc",
                "Niet a"
            }
        };

        MultipleChoiseQuestion newQuestion_copy = new MultipleChoiseQuestion()
        {
            Question = "Dit is een nieuwe test vraag",
            Correctanswer = "AA",
            Answers = new List<string>()
            {
                "dd",
                "cc",
                "Niet a"
            }
        };
        

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen vraag gevonden met het ID : `999`")]
        public void Test_GetImageQuestion_WrongID()
        {
            handler.GetMultipleChoiseQuestion(999);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen game gevonden met het ID : `999`")]
        public void Test_CreateMultipleChoiseQuestion_WrongGameID()
        {
            newQuestion = newQuestion_copy;
            handler.CreateMultipleChoiseQuestion(newQuestion, 999);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Een vraag moet tussen de 5 en 50 characters bevatten!")]
        public void Test_CreateMultipleChoiseQuestion_Question_char_3()
        {
            newQuestion = newQuestion_copy;
            newQuestion.Question = "333";
            handler.CreateMultipleChoiseQuestion(newQuestion, GameID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Een vraag moet tussen de 5 en 50 characters bevatten!")]
        public void Test_CreateMultipleChoiseQuestion_Question_char_55()
        {
            newQuestion = newQuestion_copy;
            newQuestion.Question = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            handler.CreateMultipleChoiseQuestion(newQuestion, GameID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Een vraag moet uit meer dan 3 woorden bestaan!")]
        public void Test_CreateMultipleChoiseQuestion_Question_2_words()
        {
            newQuestion = newQuestion_copy;
            newQuestion.Question = "addwwds dssaswdasdazdawwdadw";
            handler.CreateMultipleChoiseQuestion(newQuestion, GameID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er moet een antwoord gegeven worden!")]
        public void Test_CreateMultipleChoiseQuestion_No_answer()
        {
            newQuestion = newQuestion_copy;
            newQuestion.Correctanswer = "";
            handler.CreateMultipleChoiseQuestion(newQuestion, GameID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Je moet minimaal 3 antwoorden geven!")]
        public void Test_CreateMultipleChoiseQuestion_2_answer()
        {
            newQuestion = newQuestion_copy;
            newQuestion.Answers = new List<string>() { "a", "b" };
            handler.CreateMultipleChoiseQuestion(newQuestion, GameID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Je mag geen leeg antwoord geven!")]
        public void Test_CreateMultipleChoiseQuestion_Empty_answer()
        {
            newQuestion = newQuestion_copy;
            newQuestion.Answers = new List<string>() { "a", "b", "" };
            handler.CreateMultipleChoiseQuestion(newQuestion, GameID);
        }
        

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen vraag gevonden met het ID : `999`")]
        public void Test_UpdateMultipleChoiseQuestion_WrongGameID()
        {
            existingQuestion = existingQuestion_copy;
            existingQuestion.ID = 999;
            handler.UpdateMultipleChoiseQuestion(existingQuestion);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Een vraag moet tussen de 5 en 50 characters bevatten!")]
        public void Test_UpdateMultipleChoiseQuestion_Question_char_3()
        {
            existingQuestion = existingQuestion_copy;
            existingQuestion.Question = "333";
            handler.UpdateMultipleChoiseQuestion(existingQuestion);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Een vraag moet tussen de 5 en 50 characters bevatten!")]
        public void Test_UpdateMultipleChoiseQuestion_Question_char_55()
        {
            existingQuestion = existingQuestion_copy;
            existingQuestion.Question = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            handler.UpdateMultipleChoiseQuestion(existingQuestion);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Een vraag moet uit meer dan 3 woorden bestaan!")]
        public void Test_UpdateMultipleChoiseQuestion_Question_2_words()
        {
            existingQuestion = existingQuestion_copy;
            existingQuestion.Question = "addwwds dssaswdasdazdawwdadw";
            handler.UpdateMultipleChoiseQuestion(existingQuestion);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er moet een antwoord gegeven worden!")]
        public void Test_UpdateMultipleChoiseQuestion_No_answer()
        {
            existingQuestion = existingQuestion_copy;
            existingQuestion.Correctanswer = "";
            handler.UpdateMultipleChoiseQuestion(existingQuestion);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Je moet minimaal 3 antwoorden geven!")]
        public void Test_UpdateMultipleChoiseQuestion_2_answer()
        {
            existingQuestion = existingQuestion_copy;
            existingQuestion.Answers = new List<string>() { "a", "b" };
            handler.UpdateMultipleChoiseQuestion(existingQuestion);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Je mag geen leeg antwoord geven!")]
        public void Test_UpdateMultipleChoiseQuestion_Empty_answer()
        {
            existingQuestion = existingQuestion_copy;
            existingQuestion.Answers = new List<string>() { "a", "b", "" };
            handler.UpdateMultipleChoiseQuestion(existingQuestion);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen vraag gevonden met het ID : `999`")]
        public void Test_DeleteMultipleChoiseQuestion_WrongID()
        {
            handler.DeleteMultipleChoiseQuestion(999);
        }


    }
}
