using Models.SatisticModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class GameStatistic
    {
        public Game Game { get; set; }
        public int NumOfUsers { get; set; }
        public int AVGTimePlayed { get; set; }
        public int AVGPoints { get; set; }
        public int AVGLifes { get; set; }
        public int AVGNumOfBonus { get; set; }
        public int ProcentFailed { get; set; }
        public List<ChartData> Charts { get; set; }
    }
}