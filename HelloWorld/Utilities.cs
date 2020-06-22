using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;

namespace HelloWorld
{
    public static class Utilities
    {
        //=======WORKSHEET FUNCTIONS==========
        private static Excel.Application myApp = new Excel.Application();
        public static Excel.WorksheetFunction wsFunction = myApp.WorksheetFunction;
        //====================================

        //=========Message Box================
        public static void MsgBox(string message)
        {
            MessageBox.Show(message);
        }

        public static void CopyFormats()
        {
            Excel.Application myApp2 = new Excel.Application();
            Excel.Workbook book = myApp2.Workbooks.Open(@"C:\Users\grins\source\repos\HelloWorld\HelloWorld\format_test.xlsx");
            Excel.Worksheet sheet = book.Worksheets["Sheet1"];
            Excel.Range range = sheet.Range["A1:C2"];
            //range.Copy;
            Worksheet localSheet = ObjModel.Get(GetOptions.ActiveSheet);
            Excel.Range localRange = ObjModel.Get(GetOptions.SheetRange, localSheet, "A1:C2", RefType.A1);
            
            range.Copy();
            localRange.PasteSpecial(Excel.XlPasteType.xlPasteFormats);
        }
    }
}
