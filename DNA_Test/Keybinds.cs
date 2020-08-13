using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelDna.Integration;

namespace Primer
{
    public static class Keybinds
    {
        public static void LoadKeybinds(Excel.Application MyApp)       //this needs auto-loaded within the xll somehow to work with primer
        {
            //Excel.Application MyApp = (Excel.Application)ExcelDnaUtil.Application;
            MyApp.OnKey("^{Tab}", "Keybinds.FollowCtrlTab"); //this attaches your keybind to a VBA sub
        }
    }
}
