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
        public static Excel.WorksheetFunction wsFunction { get; } = ThisAddIn.MyApp.WorksheetFunction;
        
        //============= KEYBINDS =============
        public static void LoadKeybinds()
        {
            ThisAddIn.MyApp.OnKey("^{Tab}", "Keybinds.FollowCtrlTab"); //this attaches your keybind to a VBA sub
        }        

        //=============== FORMATTING =========
        public static void CopyFormats()
        {
            Worksheet localSheet = ObjModel.GetActiveSheet();
            Excel.Workbook formatBook = ThisAddIn.MyApp.Workbooks.Open(@"C:\Users\grins\source\repos\HelloWorld\HelloWorld\format_test.xlsx");
            Excel.Worksheet formatSheet = formatBook.Worksheets["Sheet1"];
            Excel.Range formatRange = formatSheet.Range["A1:C2"];
            //range.Copy;
            Excel.Range localRange = ObjModel.GetSheetRange(localSheet, "A1:C2");
            formatRange.Copy();
            localRange.PasteSpecial(Excel.XlPasteType.xlPasteAll);
            formatBook.Close();
        }
    }
}
