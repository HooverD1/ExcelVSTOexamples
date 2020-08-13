using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelDna.Integration;

namespace Primer
{
    public class Main: IExcelAddIn
    {
        public void AutoOpen()      //Startup
        {
            Keybinds.LoadKeybinds();
        }
        public void AutoClose()     //Shutdown
        {

        }
    }
}
