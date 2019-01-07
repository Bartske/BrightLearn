using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.DataModels
{
    public class Game
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Lifes { get; set; }
        public int BonusTime { get; set; }
        public string IMG { get; set; }
        public bool Online { get; set; }
        public int ChartGroupID { get; set; }
    }
}