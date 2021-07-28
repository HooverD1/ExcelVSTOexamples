using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace DNA_Test
{
    public static class DNA_Sandbox
    {
        public static void PrintToSheet(Excel.Range printRange, dynamic[,] printValues)
        {
            printRange.Value = printValues;
        }
    }
}
