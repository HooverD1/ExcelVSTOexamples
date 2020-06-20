using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Tools.Ribbon;
using Interop = Microsoft.Office.Interop.Excel;
//using Interop = Microsoft.Office.Excel;
using Microsoft.Office.Tools.Excel;

namespace HelloWorld
{
    public enum GetOptions
    {
        ActiveSheet,
        ActiveWorkbook,
        SelectionRange,
        SelectionValue,
        SheetRange
    }

    public static class ObjModel
    {
        //=========================================================GET=========================================
        private static Workbook GetActiveWorkbook()
        {
            throw new NotImplementedException();
        }
        private static NamedRange GetSelectionCell()
        {
            return (NamedRange)Globals.ThisAddIn.Application.Selection;
        }
        private static dynamic GetSelectionValue()
        {
            return GetSelectionCell().Value;
        }
        private static Worksheet GetActiveSheet()
        {            
            return (Worksheet)GetActiveWorkbook().ActiveSheet;
        }
        private static NamedRange GetSheetRange(Worksheet sheet, string rangeString)
        {
            var parser = new StringParser(rangeString);
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
            else if (opt == GetOptions.ActiveWorkbook)
                return GetActiveWorkbook();
            else if (opt == GetOptions.SheetRange)
                throw new FieldAccessException();       //Requires Get(opt, sheet, rangeString) overload
            else
                throw new KeyNotFoundException();
        }
        public static dynamic Get(GetOptions opt, Worksheet sheet, string rangeString)
        {
            if (opt == GetOptions.SheetRange)
                return GetSheetRange(sheet, rangeString);
            else
                throw new KeyNotFoundException();
        }
            //==================================SET===================================
        public static void SetSelection(dynamic newValue)
        {
            if (newValue != null)
                GetSelectionCell().Value = newValue;
        }
        
        public static void SetFormulas(string cellRange, string formula)
        {
            //cellRange of form: "A1:C10"

            var parser = new StringParser(cellRange);
            cellRange = parser.ConvertRangeA1_R1C1(cellRange);
            int rows = parser.GetNumberOfRows();
            int cols = parser.GetNumberOfColumns();
            string[,] formulaArray = new string[rows, cols];
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    formulaArray[i, j] = formula;
                }
            }
            GetSheetRange(GetActiveSheet(), cellRange).Formula = formulaArray;  //.FormulaArray(formulaArray);
        }
        


    }
}
