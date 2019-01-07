using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrightLearn.ViewModels.Manage
{
    public class EditQuestionViewModel
    {
        public Question Question { get; set; }
        public Models.Game Game { get; set; }
    }
}