using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;

namespace HelloWorld
{
    public enum GetOptions
    {
        ActiveSheet,
        SelectionRange,
        SelectionValue,
        SheetRange
    }
    public enum RefType
    {
        A1,
        R1C1
    }

    public static class ObjModel
    {
        //=========================================================GET=========================================
        private static Excel.Range GetSelectionCell() => (Excel.Range)Globals.ThisAddIn.Application.Selection;
        private static dynamic GetSelectionValue() => GetSelectionCell().Value;
        //private static Worksheet GetActiveSheet() => Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets[1]);
        private static Worksheet GetActiveSheet() => Globals.Factory.GetVstoObject(ThisAddIn.MyApp.ActiveSheet);
        //private static Excel.Application GetMyApp() => Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application);
        private static Excel.Range GetSheetRange(Worksheet sheet, string rangeString) => GetSheetRange(sheet, rangeString, RefType.A1);
        private static Excel.Range GetSheetRange(Worksheet sheet, string rangeString, RefType refType)
        {
            var parser = new RefParser(rangeString, refType);
            int rowNumber = parser.firstRowNumber;
            int columnNumber = parser.firstColumnNumber;
            int rowNumber2 = parser.secondRowNumber;
            int columnNumber2 = parser.secondColumnNumber;
            return sheet.Range[sheet.Cells[rowNumber, columnNumber], sheet.Cells[rowNumber2, columnNumber2]];
        }
        public static dynamic Get(GetOptions opt)
        {
            if (opt == GetOptions.ActiveSheet)
                return GetActiveSheet();
            else if (opt == GetOptions.SelectionRange)
                return GetSelectionCell();
            else if (opt == GetOptions.SelectionValue)
                return GetSelectionValue();
            else if (opt == GetOptions.SheetRange)
                throw new FieldAccessException();       //Requires Get(opt, sheet, rangeString) overload
            else
                throw new KeyNotFoundException();
        }
        public static dynamic Get(GetOptions opt, Worksheet sheet, string rangeString, RefType refType)
        {
            if (opt == GetOptions.SheetRange)
                return GetSheetRange(sheet, rangeString, refType);
            else
                throw new KeyNotFoundException();
        }
            //==================================SET===================================
        public static void SetSelection(dynamic newValue)
        {
            if (newValue != null)
                GetSelectionCell().Value = newValue;
        }
        
        public static void SetFormulas(string cellRange, string formula, RefType refType)
        {
            var sheet = GetActiveSheet();
            var parser = new RefParser(cellRange, refType);
            if (refType == RefType.R1C1)
            {
                //Need to convert back to A1
                cellRange = parser.ConvertRangeA1_R1C1(cellRange);
            }
            Excel.Range setRange = GetSheetRange(sheet, cellRange, refType);
            setRange.Formula = formula;
            sheet.Cells[1, 1].Calculate();       //this doesn't seem to do much..
        }

        public static void SetFormulas(string cellRange, string formula)
        {
            SetFormulas(cellRange, formula, RefType.A1);
        }

    }
}
