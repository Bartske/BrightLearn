using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.DataModels
{
    public class ImageQuestion
    {
        public int ID { get; set; }
        public string IMG { get; set; }
        public int AnswerX1 { get; set; }
        public int AnswerX2 { get; set; }
        public int AnswerY1 { get; set; }
        public int AnswerY2 { get; set; }
        public int AnswerRatio { get; set; }
        public string Question { get; set; }
    }
}