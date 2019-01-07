using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.DataModels
{
    public class HighScore
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public int UserID { get; set; }
        public int Points { get; set; }
        public int Time { get; set; }
    }
}