using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Models;
using Models.QuestionModels;
using System.IO;
using System.Web;

namespace Logic
{
    public class QuestionHandler
    {
        BrightLearnContext _dbContext = new BrightLearnContext();


        public QuestionHandler(bool Test = false)
        {
            if (Test)
            {
            }
            else
            {
            }
        }

        //Create
        
        public void CreateImageQuestion(ImageQuestion Question, HttpPostedFileBase File, int GameID)
        {
            if (_dbContext.Game.Where(g=>g.ID == GameID).Count() == 0)
                throw new Exception("Er is geen game gevonden met het ID : `"+GameID.ToString()+"`");
            if (Question.Question.Length < 5 || Question.Question.Length > 50)
                throw new Exception("Een vraag moet tussen de 5 en 50 characters bevatten!");
            if (Question.Question.Split(' ').Count() < 3)
                throw new Exception("Een vraag moet uit meer dan 3 woorden bestaan!");
            if (Question.X1 == 0 || Question.Y1 == 0)
                throw new Exception("Er moet een antwoord gegeven worden!");
            if (File == null)
                throw new Exception("Je moet een afbeelding uploaden!");


            string IMG = UploadHandler.UploadImage(File, UploadHandler.GetImageSize(PreDefImageSize.QuestionImage), PreDefImageLocation.QuestionImage).Name;
            Question.IMG = "/" + UploadHandler.GetImageUrlPath(PreDefImageLocation.QuestionImage) + IMG;

            Models.DataModels.ImageQuestion ImageQuestion = new Models.DataModels.ImageQuestion()
            {
                AnswerRatio = Question.Radius,
                AnswerX1 = Question.X1,
                AnswerX2 = Question.X2,
                AnswerY1 = Question.Y1,
                AnswerY2 = Question.Y2,
                Question = Question.Question,
                IMG = Question.IMG,
                ID = Question.ID
            };
            _dbContext.ImageQuestion.Add(ImageQuestion);
            _dbContext.SaveChanges();

            int QuestionID = _dbContext.ImageQuestion.OrderByDescending(o => o.ID).FirstOrDefault().ID;
            _dbContext.ChartGroup.Add(new Models.DataModels.ChartGroup());
            _dbContext.SaveChanges();
            int ChartGroupID = _dbContext.ChartGroup.OrderByDescending(o => o.ID).FirstOrDefault().ID;

            Models.DataModels.Question NewQuestion = new Models.DataModels.Question()
            {
                ChartGroupID = ChartGroupID,
                GameID = GameID,
                QuestionType = Models.DataModels.QuestionType.ImageQuestion,
                QuestionTypeID = QuestionID
            };

            _dbContext.Question.Add(NewQuestion);
            _dbContext.SaveChanges();
        }

        public void CreateMultipleChoiseQuestion(MultipleChoiseQuestion Question, int GameID)
        {
            if (_dbContext.Game.Where(g => g.ID == GameID).Count() == 0)
                throw new Exception("Er is geen game gevonden met het ID : `" + GameID.ToString() + "`");
            if (Question.Question.Length < 5 || Question.Question.Length > 50)
                throw new Exception("Een vraag moet tussen de 5 en 50 characters bevatten!");
            if (Question.Question.Split(' ').Count() < 3)
                throw new Exception("Een vraag moet uit meer dan 3 woorden bestaan!");
            if (Question.Correctanswer == "")
                throw new Exception("Er moet een antwoord gegeven worden!");
            if (Question.Answers.Count < 3)
                throw new Exception("Je moet minimaal 3 antwoorden geven!");
            if (Question.Answers.Contains(""))
                throw new Exception("Je mag geen leeg antwoord geven!");


            _dbContext.MultipleChoiseQuestion.Add(new Models.DataModels.MultipleChoiseQuestion() { Question = Question.Question, CorrectAnswerID = 0 });
            _dbContext.SaveChanges();
            int QuestionID = _dbContext.MultipleChoiseQuestion.OrderByDescending(o => o.ID).FirstOrDefault().ID;

            _dbContext.MultipleChoiseQuestionAnswer.Add(new Models.DataModels.MultipleChoiseQuestionAnswer() { MultipleChoiseQuestionID = QuestionID, Answer = Question.Correctanswer });
            foreach (string Answer in Question.Answers)
            {
                _dbContext.MultipleChoiseQuestionAnswer.Add(new Models.DataModels.MultipleChoiseQuestionAnswer() { MultipleChoiseQuestionID = QuestionID, Answer = Answer });
            }

            _dbContext.SaveChanges();
            int CorrectanswerID = _dbContext.MultipleChoiseQuestionAnswer.Where(a=>a.MultipleChoiseQuestionID == QuestionID && a.Answer == Question.Correctanswer).OrderByDescending(o => o.ID).FirstOrDefault().ID;
            Models.DataModels.MultipleChoiseQuestion MultipleChoiseQuestion = _dbContext.MultipleChoiseQuestion.Where(q => q.ID == QuestionID).OrderByDescending(o => o.ID).FirstOrDefault();
            MultipleChoiseQuestion.CorrectAnswerID = CorrectanswerID;
            _dbContext.SaveChanges();

            _dbContext.ChartGroup.Add(new Models.DataModels.ChartGroup());
            _dbContext.SaveChanges();
            int ChartGroupID = _dbContext.ChartGroup.OrderByDescending(o => o.ID).FirstOrDefault().ID;

            _dbContext.Question.Add(new Models.DataModels.Question() { GameID = GameID, QuestionType = Models.DataModels.QuestionType.MultipleChoise, ChartGroupID = ChartGroupID, QuestionTypeID = QuestionID });
            _dbContext.SaveChanges();

        }

        //Read
        
        public List<Question> GetQuestions(int GameID)
        {
            List<Question> Questions = new List<Question>();

            foreach (Models.DataModels.Question Question in _dbContext.Question.Where(q => q.GameID == GameID).ToList())
            {
                switch (Question.QuestionType)
                {
                    case Models.DataModels.QuestionType.ImageQuestion: //Image Question
                        int ImageQuestionID = Question.QuestionTypeID;
                        Questions.Add(new Question() { QuestionID = Question.ID, ImageQuestion = GetImageQuestion(ImageQuestionID), Type = QuestionType.ImageQuestion });
                        break;
                    case Models.DataModels.QuestionType.MultipleChoise: //Multiple Choise Question
                        int MultipleChoiseQuestionID = Question.QuestionTypeID;
                        Questions.Add(new Question() { QuestionID = Question.ID, MultipleChoiseQuestion = GetMultipleChoiseQuestion(MultipleChoiseQuestionID), Type = QuestionType.MultipleChoise });
                        break;
                }
            }

            return Questions;
        }
        
        public ImageQuestion GetImageQuestion(int ID)
        {
            if (_dbContext.ImageQuestion.Where(q=>q.ID == ID).Count() == 0)
                throw new Exception("Er is geen vraag gevonden met het ID : `"+ID.ToString() +"`");

            ImageQuestion Question = new ImageQuestion();
            Models.DataModels.ImageQuestion ImageQuestion = _dbContext.ImageQuestion.Find(ID);

            Question.ID = ImageQuestion.ID;
            Question.X1 = ImageQuestion.AnswerX1;
            Question.Y1 = ImageQuestion.AnswerY1;
            Question.IMG = ImageQuestion.IMG;
            Question.Question = ImageQuestion.Question;

            if (ImageQuestion.AnswerX2 != 0 && ImageQuestion.AnswerX2 != 0)
                Question.X2 = Convert.ToInt32(ImageQuestion.AnswerX2);

            if (ImageQuestion.AnswerY2 != 0 && ImageQuestion.AnswerY2 != 0)
                Question.Y2 = Convert.ToInt32(ImageQuestion.AnswerY2);

            if (ImageQuestion.AnswerRatio != 0 && ImageQuestion.AnswerRatio != 0)
                Question.Radius = Convert.ToInt32(ImageQuestion.AnswerRatio);

            return Question;

        }

        public MultipleChoiseQuestion GetMultipleChoiseQuestion(int ID)
        {
            if (_dbContext.MultipleChoiseQuestion.Where(q=>q.ID == ID).Count() == 0)
                throw new Exception("Er is geen vraag gevonden met het ID : `" + ID.ToString() + "`");

            MultipleChoiseQuestion MultipleChoiseQuestion = new MultipleChoiseQuestion();
            Models.DataModels.MultipleChoiseQuestion Question = _dbContext.MultipleChoiseQuestion.Where(q => q.ID == ID).First();
            MultipleChoiseQuestion.ID = ID;
            MultipleChoiseQuestion.Question = Question.Question;
            MultipleChoiseQuestion.Answers = _dbContext.MultipleChoiseQuestionAnswer.Where(qa => qa.MultipleChoiseQuestionID == Question.ID).Select(qa => qa.Answer).ToList();
            MultipleChoiseQuestion.Correctanswer = _dbContext.MultipleChoiseQuestionAnswer.Where(qa => qa.ID == Question.CorrectAnswerID).Select(qa => qa.Answer).First();

            return MultipleChoiseQuestion;
        }

        //update

        public void UpdateImageQuestion(ImageQuestion Question, HttpPostedFileBase File)
        {
            if (_dbContext.ImageQuestion.Where(q=>q.ID == Question.ID).Count() == 0)
                throw new Exception("Er is geen vraag gevonden met het ID : `" + Question.ID.ToString() + "`");
            if (Question.Question.Length < 5 || Question.Question.Length > 50)
                throw new Exception("Een vraag moet tussen de 5 en 50 characters bevatten!");
            if (Question.Question.Split(' ').Count() < 3)
                throw new Exception("Een vraag moet uit meer dan 3 woorden bestaan!");
            if (Question.X1 == 0 || Question.Y1 == 0)
                throw new Exception("Er moet een antwoord gegeven worden!");


            if (File != null)
            {
                string IMG = UploadHandler.UploadImage(File, UploadHandler.GetImageSize(PreDefImageSize.QuestionImage), PreDefImageLocation.QuestionImage).Name;
                Question.IMG = "/" + UploadHandler.GetImageUrlPath(PreDefImageLocation.QuestionImage) + IMG;
            }

            Models.DataModels.ImageQuestion ImageQuestion = _dbContext.ImageQuestion.Where(q => q.ID == Question.ID).First();
            ImageQuestion.AnswerX1 = Question.X1;
            ImageQuestion.AnswerX2 = Question.X2;
            ImageQuestion.AnswerY1 = Question.Y1;
            ImageQuestion.AnswerY2 = Question.Y2;
            ImageQuestion.AnswerRatio = Question.Radius;
            ImageQuestion.Question = Question.Question;
            ImageQuestion.IMG = Question.IMG;
            _dbContext.SaveChanges();
        }

        public void UpdateMultipleChoiseQuestion(MultipleChoiseQuestion Question)
        {
            if (_dbContext.MultipleChoiseQuestion.Where(q=>q.ID == Question.ID).Count() == 0)
                throw new Exception("Er is geen vraag gevonden met het ID : `" + Question.ID.ToString() + "`");
            if (Question.Question.Length < 5 || Question.Question.Length > 50)
                throw new Exception("Een vraag moet tussen de 5 en 50 characters bevatten!");
            if (Question.Question.Split(' ').Count() < 3)
                throw new Exception("Een vraag moet uit meer dan 3 woorden bestaan!");
            if (Question.Correctanswer == "")
                throw new Exception("Er moet een antwoord gegeven worden!");
            if (Question.Answers.Count < 3)
                throw new Exception("Je moet minimaal 3 antwoorden geven!");
            if (Question.Answers.Contains(""))
                throw new Exception("Je mag geen leeg antwoord geven!");

            _dbContext.MultipleChoiseQuestionAnswer.RemoveRange(_dbContext.MultipleChoiseQuestionAnswer.Where(q => q.MultipleChoiseQuestionID == Question.ID));

            _dbContext.MultipleChoiseQuestionAnswer.Add(new Models.DataModels.MultipleChoiseQuestionAnswer() { MultipleChoiseQuestionID = Question.ID, Answer = Question.Correctanswer });

            foreach (string Answer in Question.Answers)
            {
                _dbContext.MultipleChoiseQuestionAnswer.Add(new Models.DataModels.MultipleChoiseQuestionAnswer() { MultipleChoiseQuestionID = Question.ID, Answer = Answer });
            }

            int CorrectAnswerID = Convert.ToInt32(_dbContext.MultipleChoiseQuestionAnswer.Where(q => q.Answer == Question.Correctanswer && q.MultipleChoiseQuestionID == Question.ID).Select(q=>q.ID));

            Models.DataModels.MultipleChoiseQuestion MultipleChoiseQuestion = _dbContext.MultipleChoiseQuestion.Where(q => q.ID == Question.ID).First();
            MultipleChoiseQuestion.Question = Question.Question;
            MultipleChoiseQuestion.CorrectAnswerID = CorrectAnswerID;

            _dbContext.SaveChanges();
        }


        //delete

        public void DeleteQuestion(int QuestionID, int GameID)
        {
            Models.DataModels.Question Question = _dbContext.Question.Where(q => q.ID == QuestionID).First();
            switch (Question.QuestionType)
            {
                case Models.DataModels.QuestionType.ImageQuestion: //image Question
                    DeleteImageQuestion(Question.QuestionTypeID);
                    break;
                case Models.DataModels.QuestionType.MultipleChoise: //Multiple choise Question
                    DeleteMultipleChoiseQuestion(Question.QuestionTypeID);
                    break;
            }
        }

        public void DeleteImageQuestion(int ImageQuestionID)
        {
            if (_dbContext.ImageQuestion.Where(q=>q.ID == ImageQuestionID).Count() == 0)
                throw new Exception("Er is geen vraag gevonden met het ID : `"+ImageQuestionID.ToString()+"`");

            _dbContext.Question.Remove(_dbContext.Question.Where(q => q.QuestionTypeID == ImageQuestionID && q.QuestionType == Models.DataModels.QuestionType.ImageQuestion).First());
            _dbContext.ImageQuestion.Remove(_dbContext.ImageQuestion.Where(q=>q.ID == ImageQuestionID).First());
            _dbContext.SaveChanges();

        }

        public void DeleteMultipleChoiseQuestion(int MultipleChoiseQuestionID)
        {
            if (_dbContext.MultipleChoiseQuestion.Where(q => q.ID == MultipleChoiseQuestionID).Count() == 0)
                throw new Exception("Er is geen vraag gevonden met het ID : `" + MultipleChoiseQuestionID.ToString() + "`");

            _dbContext.Question.Remove(_dbContext.Question.Where(q => q.QuestionTypeID == MultipleChoiseQuestionID && q.QuestionType == Models.DataModels.QuestionType.MultipleChoise).First());
            _dbContext.MultipleChoiseQuestion.Remove(_dbContext.MultipleChoiseQuestion.Where(q=>q.ID == MultipleChoiseQuestionID).First());
            _dbContext.MultipleChoiseQuestionAnswer.RemoveRange(_dbContext.MultipleChoiseQuestionAnswer.Where(q=>q.MultipleChoiseQuestionID == MultipleChoiseQuestionID).ToList());
            _dbContext.SaveChanges();
        }

    }
}