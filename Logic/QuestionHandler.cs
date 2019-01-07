using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Models.Models;
using Models.Models.QuestionModels;
using System.IO;
using System.Web;

namespace Logic
{
    public class QuestionHandler
    {
        BrightLearnContext dbContext = new BrightLearnContext();


        public QuestionHandler(bool test = false)
        {
            if (test)
            {
            }
            else
            {
            }
        }

        //Create
        
        public void CreateImageQuestion(ImageQuestion question, HttpPostedFileBase file, int GameID)
        {
            if (dbContext.Game.Where(g=>g.ID == GameID).Count() == 0)
                throw new Exception("Er is geen game gevonden met het ID : `"+GameID.ToString()+"`");
            if (question.Question.Length < 5 || question.Question.Length > 50)
                throw new Exception("Een vraag moet tussen de 5 en 50 characters bevatten!");
            if (question.Question.Split(' ').Count() < 3)
                throw new Exception("Een vraag moet uit meer dan 3 woorden bestaan!");
            if (question.X1 == 0 || question.Y1 == 0)
                throw new Exception("Er moet een antwoord gegeven worden!");
            if (file == null)
                throw new Exception("Je moet een afbeelding uploaden!");


            string img = UploadHandler.UploadImage(file, UploadHandler.GetImageSize(PreDefImageSize.QuestionImage), PreDefImageLocation.QuestionImage).Name;
            question.img = "/" + UploadHandler.GetImageUrlPath(PreDefImageLocation.QuestionImage) + img;

            Models.DataModels.ImageQuestion imageQuestion = new Models.DataModels.ImageQuestion()
            {
                answerRatio = question.Radius,
                answerX1 = question.X1,
                answerX2 = question.X2,
                answerY1 = question.Y1,
                answerY2 = question.Y2,
                Question = question.Question,
                img = question.img,
                ID = question.ID
            };
            dbContext.ImageQuestion.Add(imageQuestion);
            dbContext.SaveChanges();
            //string qry = "INSERT INTO `imagequestion` (`ID`, `img`, `question`, `answerX1`, `answerX2`, `answerY1`, `answerY2`, `answerRatio`) VALUES (NULL, '"+question.img+"', '"+question.Question+"', '"+question.X1+"', '"+question.X2+"', '"+question.Y1+"', '"+question.Y2+"', '"+question.Radius+"')";
            //SQL.Insert(qry);

            int questionID = dbContext.ImageQuestion.OrderByDescending(o => o.ID).FirstOrDefault().ID;
            dbContext.ChartGroup.Add(new Models.DataModels.ChartGroup());
            dbContext.SaveChanges();
            int chartGroupID = dbContext.ChartGroup.OrderByDescending(o => o.ID).FirstOrDefault().ID;

            Models.DataModels.Question q = new Models.DataModels.Question()
            {
                ChartGroupID = chartGroupID,
                GameID = GameID,
                QuestionType = Models.DataModels.QuestionType.ImageQuestion,
                QuestionTypeID = questionID
            };

            dbContext.Question.Add(q);
            dbContext.SaveChanges();
            //string QuestionID = SQL.Select("SELECT `ID` FROM `imagequestion` WHERE `question` = '"+question.Question+ "' AND `img` = '"+question.img+"'").Last();

            //SQL.Insert("INSERT INTO `chartgroup`(`ID`) VALUES(NULL); INSERT INTO `question` (`ID`, `gameID`, `Type`, `QuestionTypeID`, `ChartGroupID`) VALUES (NULL, '" + GameID.ToString()+"', '0', '"+QuestionID+ "', ( SELECT MAX(`chartgroup`.`ID`) FROM `chartgroup`))");
        }

        public void CreateMultipleChoiseQuestion(MultipleChoiseQuestion question, int GameID)
        {
            if (dbContext.Game.Where(g => g.ID == GameID).Count() == 0)
                throw new Exception("Er is geen game gevonden met het ID : `" + GameID.ToString() + "`");
            if (question.Question.Length < 5 || question.Question.Length > 50)
                throw new Exception("Een vraag moet tussen de 5 en 50 characters bevatten!");
            if (question.Question.Split(' ').Count() < 3)
                throw new Exception("Een vraag moet uit meer dan 3 woorden bestaan!");
            if (question.Correctanswer == "")
                throw new Exception("Er moet een antwoord gegeven worden!");
            if (question.answer.Count < 3)
                throw new Exception("Je moet minimaal 3 antwoorden geven!");
            if (question.answer.Contains(""))
                throw new Exception("Je mag geen leeg antwoord geven!");


            //string qry = "INSERT INTO `mcquestion` (`ID`, `Question`, `CorrectanswerID`) VALUES (NULL, '"+question.Question+"', '"+"0"+"')";
            dbContext.MultipleChoiseQuestion.Add(new Models.DataModels.MultipleChoiseQuestion() { Question = question.Question, CorrectAnswerID = 0 });
            dbContext.SaveChanges();
            int QuestionID = dbContext.MultipleChoiseQuestion.OrderByDescending(o => o.ID).FirstOrDefault().ID;

            dbContext.MultipleChoiseQuestionAnswer.Add(new Models.DataModels.MultipleChoiseQuestionAnswer() { MultipleChoiseQuestionID = QuestionID, Answer = question.Correctanswer });
            //SQL.Insert("INSERT INTO `mcanswer` (`ID`, `mcQuestionID`, `answer`) VALUES (NULL, '" + QuestionID + "', '" + question.Correctanswer + "')");
            foreach (string answer in question.answer)
            {
                dbContext.MultipleChoiseQuestionAnswer.Add(new Models.DataModels.MultipleChoiseQuestionAnswer() { MultipleChoiseQuestionID = QuestionID, Answer = answer });
                    //SQL.Insert("INSERT INTO `mcanswer` (`ID`, `mcQuestionID`, `answer`) VALUES (NULL, '" + QuestionID + "', '" + answer + "')");
            }
            dbContext.SaveChanges();
            int CorrectanswerID = dbContext.MultipleChoiseQuestionAnswer.Where(a=>a.MultipleChoiseQuestionID == QuestionID && a.Answer == question.Correctanswer).OrderByDescending(o => o.ID).FirstOrDefault().ID;
            Models.DataModels.MultipleChoiseQuestion mcq = dbContext.MultipleChoiseQuestion.Where(q => q.ID == QuestionID).OrderByDescending(o => o.ID).FirstOrDefault();
            mcq.CorrectAnswerID = CorrectanswerID;
            dbContext.SaveChanges();
            //SQL.Update("UPDATE `mcquestion` SET `CorrectanswerID` = '"+CorrectanswerID+"' WHERE `mcquestion`.`ID` = '"+QuestionID+"'");

            dbContext.ChartGroup.Add(new Models.DataModels.ChartGroup());
            dbContext.SaveChanges();
            int chartGroupID = dbContext.ChartGroup.OrderByDescending(o => o.ID).FirstOrDefault().ID;

            dbContext.Question.Add(new Models.DataModels.Question() { GameID = GameID, QuestionType = Models.DataModels.QuestionType.MultipleChoise, ChartGroupID = chartGroupID, QuestionTypeID = QuestionID });
            dbContext.SaveChanges();

            //SQL.Insert("INSERT INTO `chartgroup`(`ID`) VALUES(NULL); INSERT INTO `question` (`ID`, `gameID`, `Type`, `QuestionTypeID`, `ChartGroupID`) VALUES (NULL, '" + GameID.ToString() + "', '1', '" + QuestionID + "', ( SELECT MAX(`chartgroup`.`ID`) FROM `chartgroup`))");
        }

        //Read
        
        public List<Question> GetQuestions(int GameID)
        {
            List<Question> list = new List<Question>();

            foreach (Models.DataModels.Question Q in dbContext.Question.Where(q => q.GameID == GameID).ToList())
            {
                switch (Q.QuestionType)
                {
                    case Models.DataModels.QuestionType.ImageQuestion: //Image Question
                        int QuestionTypeID = Q.QuestionTypeID;
                        list.Add(new Question() { QuestionID = Q.ID, imageQuestion = GetImageQuestion(QuestionTypeID), Type = QuestionType.ImageQuestion });
                        break;
                    case Models.DataModels.QuestionType.MultipleChoise: //Multiple Choise Question
                        int mcQuestionID = Q.QuestionTypeID;
                        list.Add(new Question() { QuestionID = Q.ID, multipleChoiseQuestion = GetMultipleChoiseQuestion(mcQuestionID), Type = QuestionType.MultipleChoise });
                        break;
                }
            }

            return list;
        }
        
        public ImageQuestion GetImageQuestion(int ID)
        {
            if (dbContext.ImageQuestion.Where(q=>q.ID == ID).Count() == 0)
                throw new Exception("Er is geen vraag gevonden met het ID : `"+ID.ToString() +"`");

            ImageQuestion question = new ImageQuestion();
            Models.DataModels.ImageQuestion iq = dbContext.ImageQuestion.Find(ID);

            question.ID = iq.ID;
            question.X1 = iq.answerX1;
            question.Y1 = iq.answerY1;
            question.img = iq.img;
            question.Question = iq.Question;

            if (iq.answerX2 != 0 && iq.answerX2 != null)
                question.X2 = Convert.ToInt32(iq.answerX2);

            if (iq.answerY2 != 0 && iq.answerY2 != null)
                question.Y2 = Convert.ToInt32(iq.answerY2);

            if (iq.answerRatio != 0 && iq.answerRatio != null)
                question.Radius = Convert.ToInt32(iq.answerRatio);

            return question;

        }

        public MultipleChoiseQuestion GetMultipleChoiseQuestion(int ID)
        {
            if (dbContext.MultipleChoiseQuestion.Where(q=>q.ID == ID).Count() == 0)
                throw new Exception("Er is geen vraag gevonden met het ID : `" + ID.ToString() + "`");

            MultipleChoiseQuestion multipleChoiseQuestion = new MultipleChoiseQuestion();
            Models.DataModels.MultipleChoiseQuestion Q = dbContext.MultipleChoiseQuestion.Where(q => q.ID == ID).First();
            multipleChoiseQuestion.ID = ID;
            multipleChoiseQuestion.Question = Q.Question;
            multipleChoiseQuestion.answer = dbContext.MultipleChoiseQuestionAnswer.Where(qa => qa.MultipleChoiseQuestionID == Q.ID).Select(qa => qa.Answer).ToList();
            multipleChoiseQuestion.Correctanswer = dbContext.MultipleChoiseQuestionAnswer.Where(qa => qa.ID == Q.CorrectAnswerID).Select(qa => qa.Answer).First();

            return multipleChoiseQuestion;
        }

        //update

        public void UpdateImageQuestion(ImageQuestion question, HttpPostedFileBase file)
        {
            if (dbContext.ImageQuestion.Where(q=>q.ID == question.ID).Count() == 0)
                throw new Exception("Er is geen vraag gevonden met het ID : `" + question.ID.ToString() + "`");
            if (question.Question.Length < 5 || question.Question.Length > 50)
                throw new Exception("Een vraag moet tussen de 5 en 50 characters bevatten!");
            if (question.Question.Split(' ').Count() < 3)
                throw new Exception("Een vraag moet uit meer dan 3 woorden bestaan!");
            if (question.X1 == 0 || question.Y1 == 0)
                throw new Exception("Er moet een antwoord gegeven worden!");


            if (file != null)
            {
                string img = UploadHandler.UploadImage(file, UploadHandler.GetImageSize(PreDefImageSize.QuestionImage), PreDefImageLocation.QuestionImage).Name;
                question.img = "/" + UploadHandler.GetImageUrlPath(PreDefImageLocation.QuestionImage) + img;
            }

            Models.DataModels.ImageQuestion iq = dbContext.ImageQuestion.Where(q => q.ID == question.ID).First();
            iq.answerX1 = question.X1;
            iq.answerX2 = question.X2;
            iq.answerY1 = question.Y1;
            iq.answerY2 = question.Y2;
            iq.answerRatio = question.Radius;
            iq.Question = question.Question;
            iq.img = question.img;
            dbContext.SaveChanges();
        }

        public void UpdateMultipleChoiseQuestion(MultipleChoiseQuestion question)
        {
            if (dbContext.MultipleChoiseQuestion.Where(q=>q.ID == question.ID).Count() == 0)
                throw new Exception("Er is geen vraag gevonden met het ID : `" + question.ID.ToString() + "`");
            if (question.Question.Length < 5 || question.Question.Length > 50)
                throw new Exception("Een vraag moet tussen de 5 en 50 characters bevatten!");
            if (question.Question.Split(' ').Count() < 3)
                throw new Exception("Een vraag moet uit meer dan 3 woorden bestaan!");
            if (question.Correctanswer == "")
                throw new Exception("Er moet een antwoord gegeven worden!");
            if (question.answer.Count < 3)
                throw new Exception("Je moet minimaal 3 antwoorden geven!");
            if (question.answer.Contains(""))
                throw new Exception("Je mag geen leeg antwoord geven!");

            dbContext.MultipleChoiseQuestionAnswer.RemoveRange(dbContext.MultipleChoiseQuestionAnswer.Where(q => q.MultipleChoiseQuestionID == question.ID));

            dbContext.MultipleChoiseQuestionAnswer.Add(new Models.DataModels.MultipleChoiseQuestionAnswer() { MultipleChoiseQuestionID = question.ID, Answer = question.Correctanswer });
            //SQL.Insert("INSERT INTO `mcanswer` (`ID`, `mcQuestionID`, `answer`) VALUES (NULL, '" + QuestionID + "', '" + question.Correctanswer + "')");
            foreach (string answer in question.answer)
            {
                dbContext.MultipleChoiseQuestionAnswer.Add(new Models.DataModels.MultipleChoiseQuestionAnswer() { MultipleChoiseQuestionID = question.ID, Answer = answer });
                //SQL.Insert("INSERT INTO `mcanswer` (`ID`, `mcQuestionID`, `answer`) VALUES (NULL, '" + QuestionID + "', '" + answer + "')");
            }
            int CorrectAnswerID = Convert.ToInt32(dbContext.MultipleChoiseQuestionAnswer.Where(q => q.Answer == question.Correctanswer && q.MultipleChoiseQuestionID == question.ID).Select(q=>q.ID));

            Models.DataModels.MultipleChoiseQuestion mcQ = dbContext.MultipleChoiseQuestion.Where(q => q.ID == question.ID).First();
            mcQ.Question = question.Question;
            mcQ.CorrectAnswerID = CorrectAnswerID;

            dbContext.SaveChanges();
        }


        //delete

        public void DeleteQuestion(int QuestionID, int GameID)
        {
            Models.DataModels.Question Q = dbContext.Question.Where(q => q.ID == QuestionID).First();
            switch (Q.QuestionType)
            {
                case Models.DataModels.QuestionType.ImageQuestion: //image Question
                    DeleteImageQuestion(Q.QuestionTypeID);
                    break;
                case Models.DataModels.QuestionType.MultipleChoise: //Multiple choise Question
                    DeleteMultipleChoiseQuestion(Q.QuestionTypeID);
                    break;
            }
        }

        public void DeleteImageQuestion(int ImageQuestionID)
        {
            if (dbContext.ImageQuestion.Where(q=>q.ID == ImageQuestionID).Count() == 0)
                throw new Exception("Er is geen vraag gevonden met het ID : `"+ImageQuestionID.ToString()+"`");

            dbContext.Question.Remove(dbContext.Question.Where(q => q.QuestionTypeID == ImageQuestionID && q.QuestionType == Models.DataModels.QuestionType.ImageQuestion).First());
            dbContext.ImageQuestion.Remove(dbContext.ImageQuestion.Where(q=>q.ID == ImageQuestionID).First());
            dbContext.SaveChanges();

        }

        public void DeleteMultipleChoiseQuestion(int MultipleChoiseQuestionID)
        {
            if (dbContext.MultipleChoiseQuestion.Where(q => q.ID == MultipleChoiseQuestionID).Count() == 0)
                throw new Exception("Er is geen vraag gevonden met het ID : `" + MultipleChoiseQuestionID.ToString() + "`");

            dbContext.Question.Remove(dbContext.Question.Where(q => q.QuestionTypeID == MultipleChoiseQuestionID && q.QuestionType == Models.DataModels.QuestionType.MultipleChoise).First());
            dbContext.MultipleChoiseQuestion.Remove(dbContext.MultipleChoiseQuestion.Where(q=>q.ID == MultipleChoiseQuestionID).First());
            dbContext.MultipleChoiseQuestionAnswer.RemoveRange(dbContext.MultipleChoiseQuestionAnswer.Where(q=>q.MultipleChoiseQuestionID == MultipleChoiseQuestionID).ToList());
            dbContext.SaveChanges();
        }

    }
}