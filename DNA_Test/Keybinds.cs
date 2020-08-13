﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelDna.Integration;
using ExcelDna.ComInterop;
using System.Reflection;

namespace Primer
{
    public static class Keybinds
    {
        public static bool LoadKeybinds()
        {
            Excel.Application MyApp = (Excel.Application)ExcelDnaUtil.Application;
            MyApp.OnKey("^{Tab}", "FollowLink");
            return true;
        }

        [ExcelCommand(ShortCut = "^{Tab}")]
        public static void FollowLink()
        {
            XlCall.Excel(XlCall.xlcAlert, "Follow Link Code Here!");
        }

    }
}
