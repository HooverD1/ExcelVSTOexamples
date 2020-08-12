using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelDna.Integration;
using System.Diagnostics;

namespace DNA_Test
{
    public static class UDFs
    {
        private static InflationTable inflTable { get; set; }
        //All public static functions will be registered as UDFs

        [ExcelFunction(Description = "Inflation calculation function", Category = "ROSE Primer")]
        public static double INFL(int fromYear, int toYear, int mode, int category, int agency)
        {
            if (inflTable == null)
            {
                inflTable = new InflationTable();
                inflTable.SetAgency(4);
                inflTable.UpdateTable();
            }
            var returnVal = InflationCalculator.Calculate(fromYear, toYear, mode, category, agency);
            return returnVal;
        }
    }
}