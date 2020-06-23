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
        public static Excel.WorksheetFunction wsFunction = ThisAddIn.MyApp.WorksheetFunction;
        //====================================
        
        public static void LoadKeybind()
        {
            string message = "Hey there";
            ThisAddIn.MyApp.OnKey("^{Tab}", $"MsgBox(\"{message}\")");
        }
        //=========Message Box================
        public static void MsgBox(string message)
        {
            MessageBox.Show(message);
        }

        public static void CopyFormats()
        {
            Worksheet localSheet = ObjModel.Get(GetOptions.ActiveSheet);
            Excel.Workbook formatBook = ThisAddIn.MyApp.Workbooks.Open(@"C:\Users\grins\source\repos\HelloWorld\HelloWorld\format_test.xlsx");
            Excel.Worksheet formatSheet = formatBook.Worksheets["Sheet1"];
            Excel.Range formatRange = formatSheet.Range["A1:C2"];
            //range.Copy;
            Excel.Range localRange = ObjModel.Get(GetOptions.SheetRange, localSheet, "A1:C2", RefType.A1);
            
            formatRange.Copy();
            localRange.PasteSpecial(Excel.XlPasteType.xlPasteAll);
            formatBook.Close();
        }
    }
}
