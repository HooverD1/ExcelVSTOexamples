using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace DNA_Test
{

    public class MyAddin : ExcelDna.Integration.IExcelAddIn
    {
        public static Excel.Application MyApp { get; set; }

        public void AutoOpen()
        {            
            MyApp = (Excel.Application)ExcelDna.Integration.ExcelDnaUtil.Application;

        }
        public void AutoClose()
        {

        }
    }
}
