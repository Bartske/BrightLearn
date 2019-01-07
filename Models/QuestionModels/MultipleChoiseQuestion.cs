using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.QuestionModels
{
    public class MultipleChoiseQuestion : IQuestionType
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public List<string> Answers { get; set; }
        public string Correctanswer { get; set; }
    }
}