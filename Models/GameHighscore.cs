using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class GameHighscore
    {
        public int GameID { get; set; }
        public string GameName { get; set; }
        public List<string> Name { get; set; }
        public List<string> points { get; set; }
    }
}