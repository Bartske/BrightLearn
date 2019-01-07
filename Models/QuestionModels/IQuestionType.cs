using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.QuestionModels
{
    interface IQuestionType
    {
        int ID { get; set; }
        string Question { get; set; }
    }
}
