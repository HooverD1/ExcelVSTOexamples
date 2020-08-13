using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelDna.Integration;

namespace Primer
{
    public static class UDFs
    {
        private static InflationTable inflTable { get; set; }
        private static InflationCalculator inflCalc { get; set; }
        //All public static functions will be registered as UDFs

        [ExcelFunction(Description = "Inflation calculation function", Category = "ROSE Primer")]
        public static double INFL(int fromYear, int toYear, int mode, int category, int agency=4)
        {
            if(inflCalc == null)
            {
                inflCalc = new InflationCalculator();
            }
            if (inflTable == null)
            {
                inflTable = new InflationTable();
                inflTable.SetAgency(4);
                inflTable.UpdateTable();
            }
            var returnVal = inflCalc.Calculate(fromYear, toYear, mode, category, agency);
            return returnVal;
        }
    }
}