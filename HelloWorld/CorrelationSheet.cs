using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class CorrelationSheet : Sheet
    {
        public Estimate Correlates { get; set; }     //things that are correlated - estimates and inputs
        private Excel.Range MatrixTopLeft { get; set; }
        
        public CorrelationSheet() : base()
        {
            bool sheetFound = false;
            foreach(Excel.Worksheet sheet in ThisAddIn.MyApp.Worksheets)
            {
                if (sheet.Name == "Correlation")
                    sheetFound = true;
            }
            if (!sheetFound)
            {
                Excel.Worksheet correlSheet = ThisAddIn.MyApp.Worksheets.Add();
                correlSheet.Name = "Correlation";
            }
            this.AttachSheet(ThisAddIn.MyApp.Worksheets["Correlation"]);

            MatrixTopLeft = ThisSheet.Range["E4"];
        }
        public override void Format()
        {
            
        }
        public override void ReadFromSheet() { }
        public override void WriteToSheet() { }
    }
}
