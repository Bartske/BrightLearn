using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using Models;

namespace BrightLearn.ViewModels.Manage
{
    public class AddQuestionViewModel
    {
        public Models.Game Game { get; set; }
        public Question Question { get; set; }
        public int GameID { get; set; }
    }
}