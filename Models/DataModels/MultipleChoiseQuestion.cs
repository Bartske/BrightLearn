using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.DataModels
{
    public class MultipleChoiseQuestion
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public int CorrectAnswerID { get; set; }
    }
}