using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.DataModels
{
    public class MultipleChoiseQuestionAnswer
    {
        public int ID { get; set; }
        public int MultipleChoiseQuestionID { get; set; }
        public string Answer { get; set; }
    }
}