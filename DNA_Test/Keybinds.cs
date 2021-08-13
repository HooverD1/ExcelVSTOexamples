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
            if (DNA_Test.MyAddin.MyApp.Selection.Cells.Count() == 1)
                new DNA_Test.DateSelector().ShowDialog();
            else
            {
                var schForm = new DNA_Test.Scheduler.SchedulerForm();
                if (!schForm.VerifySingleDimension())
                    MessageBox.Show("Selection must be a single row or column.");
                else
                    schForm.ShowDialog();
            }
                
        }
        [ExcelCommand(ShortCut = "^{F6}")]
        public static void AutoBucketData()
        {
            Excel.Range selection = DNA_Test.MyAddin.MyApp.Selection;
            
            object[,] data = selection.Value;
            DateTime[] dates = new DateTime[data.GetLength(0)];
            double[] values = new double[data.GetLength(0)];
            for (int r = 1; r <= data.GetLength(0); r++)
            {
                if(DateTime.TryParse(data[r, 1].ToString(), out DateTime dt))
                    dates[r - 1] = dt;
                if(double.TryParse(data[r,2].ToString(), out double dbl))
                    values[r - 1] = dbl;
            }
            DNA_Test.Bucketer bucketer = new DNA_Test.Bucketer();
            List<DNA_Test.Optimizers.OptimizerFunction> optimizers = new List<DNA_Test.Optimizers.OptimizerFunction> { DNA_Test.Optimizers.ScheduleOptimizer };
            List<DNA_Test.OptimizationResult> optimizationResults = bucketer.AutoBucket(dates, values, optimizers, true);
            //Hand off the results to the user selection form
            DNA_Test.FitSelectorForm fitSelector = new DNA_Test.FitSelectorForm(optimizationResults);
            fitSelector.Show();

        }

        [ExcelCommand(ShortCut = "^{F1}")]
        public static void FakeData()
        {
            int datapoints = 100000;
            Accord.Statistics.Distributions.Univariate.NormalDistribution normDist = new Accord.Statistics.Distributions.Univariate.NormalDistribution(10, 1);
            Excel.Worksheet sheet = DNA_Test.MyAddin.MyApp.ActiveSheet;
            DateTime[,] dates = new DateTime[datapoints,1];
            double[,] values = new double[datapoints, 1];
            Random rando = new Random();
            for(int i = 0; i < datapoints; i++)
            {
                dates[i,0] = DateTime.Today.AddDays(rando.Next(365*10));
                values[i, 0] = normDist.Generate();
            }
            sheet.Range[$"A1:A{datapoints}"].Value = dates;
            sheet.Range[$"A1:A{datapoints}"].NumberFormat = "MM/DD/YYYY";
            sheet.Range[$"B1:B{datapoints}"].Value = values;
            sheet.Range[$"A1:B{datapoints}"].Select();
        }
    }
}
