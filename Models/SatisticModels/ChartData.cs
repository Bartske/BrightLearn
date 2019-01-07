using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.SatisticModels
{
    public class ChartData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<ChartValue> Values { get; set; }
    }

    public struct ChartValue
    {
        public ChartValue(string Label, string Value)
        {
            this.Label = Label;
            this.Value = Value;
        }

        public string Label { get; set; }
        public string Value { get; set; }
    }
}