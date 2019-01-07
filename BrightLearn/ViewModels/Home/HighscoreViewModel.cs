using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace BrightLearn.ViewModels.Home
{
    public class HighscoreViewModel
    {
        public List<GameHighscore> gameHighscores { get; set; }
    }
}