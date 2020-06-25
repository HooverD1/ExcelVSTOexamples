using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class InflationTable
    {
        public int agencyNumber { get; set; }
        public int category { get; set; }
        //public string agencyName { get; set; }
        private Dictionary<int, double> rawTable { get; set; }      //<year, inflation raw weight>
        private Dictionary<int, double> weightTable { get; set; }   //<year, inflation weighted weight>
        public InflationTable(int agencyNumber)
        {
            this.agencyNumber = agencyNumber;
        }
        public void UpdateTable()
        {
            //Grab table from the excel
        }
        public double GetRawValue(int year)
        {
            return .5;
            //return rawTable[year];
        }
        public double GetWeightedValue(int year)
        {
            return .5;
            //return weightTable[year];
        }
    }
}
