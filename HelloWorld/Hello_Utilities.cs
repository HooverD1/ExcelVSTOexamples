﻿using System;
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
        public static void LoadKeybinds()
        {
            ThisAddIn.MyApp.OnKey("^{Tab}", "Keybinds.FollowCtrlTab"); //this attaches your keybind to a VBA sub
        }        
        
        //=============== FORMATTING =========
        public static void CopyFormats()
        {            
            Worksheet localSheet = ObjModel.GetActiveSheet();
            //Excel.Workbook formatBook = ThisAddIn.MyApp.Workbooks.Open(@"C:\Users\grins\source\repos\HelloWorld\HelloWorld\format_test.xlsx");
            Excel.Workbook formatBook = Hello_Utilities.OpenWorkbook(@"C:\Users\grins\source\repos\HelloWorld\HelloWorld\format_test.xlsx");
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
        public static Excel.Application CreateTempApplication()
        {
            Excel.Application tempApp = new Excel.Application();
            ThisAddIn.TempAppList.Add(tempApp);
            return tempApp;
        }
        public static Excel.Workbook OpenWorkbook(string path, bool RunInNewApp = false, bool Visible=true)
        {
            if (RunInNewApp == false)
            {
                Excel.Workbook newBook = ThisAddIn.MyApp.Workbooks.Open(path);
                if (Visible == false)
                {
                    foreach (dynamic win in newBook.Windows)
                    {
                        win.Visible = false;
                    }
                }                
                return newBook;
            }
                
            else if (RunInNewApp == true)
            {
                Excel.Application tempApp = Hello_Utilities.CreateTempApplication();
                return tempApp.Workbooks.Open(path);

            }
            else
                throw new Exception();
        }
        public static double[,] ConvertObjectArrayToDouble(object[,] objArray)
        {
            double[,] correlArray = new double[objArray.GetLength(0), objArray.GetLength(1)];
            for (int i = 1; i <= objArray.GetLength(0); i++)
            {
                for (int j = 1; j <= objArray.GetLength(1); j++)
                {
                    correlArray[i-1, j-1] = (double)objArray[i, j];
                }
            }
            return correlArray;
        }

        

        
        
    }
}