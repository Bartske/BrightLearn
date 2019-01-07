using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.QuestionModels;

namespace Models
{
    public class Question
    {
        public int QuestionID { get; set; }
        public QuestionType Type { get; set; }
        public ImageQuestion ImageQuestion { get; set; }
        public MultipleChoiseQuestion MultipleChoiseQuestion { get; set; }
    }
    public enum QuestionType { ImageQuestion, MultipleChoise }
}