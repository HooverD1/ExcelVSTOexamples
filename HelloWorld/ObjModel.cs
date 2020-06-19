using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

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
            return Globals.ThisAddIn.Application.ActiveWorkbook;
        }
        private static Range GetSelectionCell()
        {
            return Globals.ThisAddIn.Application.Selection;
        }
        private static dynamic GetSelectionValue()
        {
            return GetSelectionCell().Value;
        }
        private static Worksheet GetActiveSheet()
        {
            return Globals.ThisAddIn.GetActiveWorksheet();
        }
        private static Range GetSheetRange(Worksheet sheet, string rangeString)
        {
            return sheet.Range[rangeString];
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
    }
}
