using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using ExcelDna.Integration;

namespace DNA_Test
{
    public static class Utilities
    {
        public static Object GetApplication()
        {
            return ExcelDnaUtil.Application;
        }
    }
}
