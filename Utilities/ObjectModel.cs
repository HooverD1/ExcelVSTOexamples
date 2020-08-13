using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Utilities
{
    public static class ObjectModel
    {
        public static List<Excel.Application> TempAppList = new List<Excel.Application>();

        public static Excel.Application CreateTempApplication()
        {
            Excel.Application tempApp = new Excel.Application();
            TempAppList.Add(tempApp);
            return tempApp;
        }
        public static Excel.Workbook OpenWorkbook(string path, Excel.Application Application, bool Visible = true)
        {
            Excel.Workbook newBook = Application.Workbooks.Open(path);
            if (Visible == false)
            {
                foreach (dynamic win in newBook.Windows)
                    win.Visible = false;
            }            
            return newBook;
        }
    }
}
