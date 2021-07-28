using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelDna.Integration;
using ExcelDna.ComInterop;
using System.Reflection;
using System.Windows;

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
            //XlCall.Excel(XlCall.xlcAlert, "Follow Link Code Here!");
            Excel.Application MyApp = (Excel.Application)ExcelDnaUtil.Application;
            string link_reference = (string)MyApp.ActiveCell.Formula;     //cell reference to go to
            Primer.RefParser parser = new Primer.RefParser(link_reference, MyApp, Primer.RefType.A1);
            if (parser.malformed == false)
            {
                parser.firstCell.Worksheet.Activate();
                parser.firstCell.Select();
            }
        }
        [ExcelCommand(ShortCut = "^{F5}")]
        public static void LaunchScheduler()
        {
            DNA_Test.Scheduler.SchedulerForm schedulerForm = new DNA_Test.Scheduler.SchedulerForm();
            if (!schedulerForm.VerifySingleDimension())
                MessageBox.Show("Selection must be a single row or column.");
            else
                schedulerForm.ShowDialog();
        }
    }
}
