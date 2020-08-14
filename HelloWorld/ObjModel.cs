﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;

namespace HelloWorld
{

    public static class ObjModel
    {
        //=========================================================GET=========================================
        public static Excel.Range GetSelection() => ThisAddIn.MyApp.Selection;
        //public static dynamic GetSelectionValue() => GetSelectionCell().Value;
        //private static Worksheet GetActiveSheet() => Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets[1]);
        public static Worksheet GetActiveSheet() => Globals.Factory.GetVstoObject(ThisAddIn.MyApp.ActiveSheet);
        //private static Excel.Application GetMyApp() => Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application);
        public static Excel.Range GetSheetRange(Worksheet sheet, string rangeString) => GetSheetRange(sheet, rangeString, Utilities.RefType.A1);
        public static Excel.Range GetSheetRange(Worksheet sheet, string rangeString, Utilities.RefType refType)
        {
            var parser = new Utilities.RefParser(rangeString, ThisAddIn.MyApp, refType);
            int rowNumber = parser.firstRowNumber;
            int columnNumber = parser.firstColumnNumber;
            int rowNumber2 = parser.secondRowNumber;
            int columnNumber2 = parser.secondColumnNumber;
            return sheet.Range[sheet.Cells[rowNumber, columnNumber], sheet.Cells[rowNumber2, columnNumber2]];
        }
            //==================================SET===================================
        public static void SetSelection(dynamic newValue)
        {
            if (newValue != null)
                GetSelection().Value = newValue;
        }

        public static void SetCell(dynamic newValue, string cellRange)
        {
            Worksheet sheet = GetActiveSheet();
            Excel.Range cell = sheet.Range[cellRange];
            if (newValue != null)
                cell.Value = newValue;
        }
        
        public static void SetFormulas(string cellRange, string formula, Utilities.RefType refType = Utilities.RefType.A1)
        {
            var sheet = GetActiveSheet();
            var parser = new Utilities.RefParser(cellRange, ThisAddIn.MyApp, refType);
            if (refType == Utilities.RefType.R1C1)
            {
                //Need to convert back to A1
                cellRange = parser.ConvertRangeA1_R1C1(cellRange);
            }
            Excel.Range setRange = GetSheetRange(sheet, cellRange, refType);
            setRange.Formula = formula;
            sheet.Cells[1, 1].Calculate();       //this doesn't seem to do much..
        }
        public static void SetArrayFormulas(string cellRange, string formula, Utilities.RefType refType = Utilities.RefType.A1)
        {
            var sheet = GetActiveSheet();
            var parser = new Utilities.RefParser(cellRange, ThisAddIn.MyApp, refType);
            if (refType == Utilities.RefType.R1C1)
            {
                //Need to convert back to A1
                cellRange = parser.ConvertRangeA1_R1C1(cellRange);
            }
            Excel.Range setRange = GetSheetRange(sheet, cellRange, refType);
            setRange.FormulaArray = formula;
            sheet.Cells[1, 1].Calculate();       //this doesn't seem to do much..
        }

    }
}
