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
        SelectionCell,
        SelectionValue
    }

    public static class ObjModel
    {
        private static Range GetSelectionCell()
        {
            return Globals.ThisAddIn.Application.Selection;
        }
        private static dynamic GetSelectionValue()
        {
            return GetSelectionCell().Value;
        }
        public static void SetSelection(dynamic newValue)
        {
            GetSelectionCell().Value = newValue;
        }
        private static Worksheet GetActiveSheet()
        {
            return Globals.ThisAddIn.GetActiveWorksheet();
        }
        public static dynamic Get(GetOptions opt)
        {
            if (opt == GetOptions.ActiveSheet)
                return GetActiveSheet();
            else if (opt == GetOptions.SelectionCell)
                return GetSelectionCell();
            else if (opt == GetOptions.SelectionValue)
                return GetSelectionValue();
            else
                throw new KeyNotFoundException();
        }
    }
}
