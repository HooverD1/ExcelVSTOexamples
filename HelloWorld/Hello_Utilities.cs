using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using System.Data.Linq;         //for LINQ-to-SQL
using Accord.Math.Decompositions;

namespace HelloWorld
{
    public static class Hello_Utilities
    {
        //============= KEYBINDS =============
                
        
        //=============== FORMATTING =========
        public static void CopyFormats()
        {            
            Worksheet localSheet = ObjModel.GetActiveSheet();
            Excel.Workbook formatBook = Utilities.ObjectModel.OpenWorkbook(@"C:\Users\grins\source\repos\HelloWorld\HelloWorld\format_test.xlsx", ThisAddIn.MyApp, false);
            Excel.Worksheet formatSheet = formatBook.Worksheets["Sheet1"];
            Excel.Range formatRange = formatSheet.Range["A1:C2"];
            //range.Copy;
            foreach(dynamic win in formatBook.Windows)
            {
                win.Visible = false;
            }
            //formatBook.Windows[1].Visible = false;
            Excel.Range localRange = ObjModel.GetSheetRange(localSheet, "A1:C2");
            formatRange.Copy();
            localRange.PasteSpecial(Excel.XlPasteType.xlPasteAll);
            //formatBook.Close();
        }
        
    }
}
