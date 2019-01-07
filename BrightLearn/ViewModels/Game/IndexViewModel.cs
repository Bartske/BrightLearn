using Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrightLearn.ViewModels.Game
{
    public class IndexViewModel
    {
        public int GameID { get; set; }
        public int Lifes { get; set; }
        public int bonusTime { get; set; }

        public List<Question> Questions { get; set; }
    }

}