using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.DataModels
{
    public class ChartValues
    {
        public int ID { get; set; }
        public int ChartID { get; set; }
        public int ChartGroupID { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
    }
}