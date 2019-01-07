using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.QuestionModels
{
    public class ImageQuestion : IQuestionType
    {
        public int ID { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int Radius { get; set; }
        public string IMG { get; set; }
        public string Question { get; set; }
    }
}