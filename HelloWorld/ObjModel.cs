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
        Selection
    }

    public static class ObjModel
    {
        public static Range GetSelection()
        {
            return Globals.ThisAddIn.Application.Selection;
        }
        public static void SetSelection<T>(T newValue)
        {
            Range selection = Globals.ThisAddIn.Application.Selection;
            selection.Value = newValue;
        }
        public static Worksheet GetActiveSheet()
        {
            return Globals.ThisAddIn.GetActiveWorksheet();
        }
    }
}
