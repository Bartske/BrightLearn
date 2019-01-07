using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.SatisticModels
{
    public class GameScore
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public int UserID { get; set; }
        public int TimePlayed { get; set; }
        public int Points { get; set; }
        public int Lifes { get; set; }
        public int NumOfBonus { get; set; }
        public bool Passed { get; set; }
    }
}