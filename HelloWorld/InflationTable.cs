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
        //public string agencyName { get; set; }
        private Dictionary<int, double> rawTable { get; set; }
        private Dictionary<int, double> weightTable { get; set; }
        public InflationTable(int agencyNumber)
        {
            this.agencyNumber = agencyNumber;
        }
        public void UpdateTable()
        {
            //Grab table from the excel
        }
    }
}
