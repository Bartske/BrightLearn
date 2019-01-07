using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace BrightLearn.ViewModels.Statistic
{
    public class StatisticQuestionsViewModel
    {
        public List<Question> questions { get; set; }
        public Models.Game game { get; set; }
    }
}