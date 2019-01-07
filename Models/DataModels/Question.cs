using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.DataModels
{
    public class Question
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public QuestionType QuestionType { get; set; }
        public int QuestionTypeID { get; set; }
        public int ChartGroupID { get; set; }
    }
    public enum QuestionType { ImageQuestion, MultipleChoise }

}