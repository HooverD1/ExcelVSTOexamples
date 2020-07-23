using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class CASE_Model
    {
        private List<Sheet> sheets = new List<Sheet>();
        public FileSheet fileSheet { get; set; }
        public DataSheet dataSheet { get; set; }
        public TotalSheet totalSheet { get; set; }
        public List<WBSsheet> WBSsheets = new List<WBSsheet>();
        public List<EstimateSheet> EstimateSheets = new List<EstimateSheet>();
        public CorrelationSheet correlationSheet { get; set; }

        public CASE_Model()
        {
            //Create model from template
            List<string> sheetNames = new List<string>() { "Correlation", "EST_1", "WBS_1", "Total", "Data", "File" };
            foreach(Excel.Worksheet sheet in ThisAddIn.MyApp.Worksheets)
            {
                if (sheetNames.Contains(sheet.Name))
                    sheetNames.Remove(sheet.Name);
            }
            foreach(string name in sheetNames)
            {
                Excel.Worksheet newSheet = ThisAddIn.MyApp.Sheets.Add();
                newSheet.Name = name;
            }

            //Create sheet objects
            fileSheet = new FileSheet(ThisAddIn.MyApp.Worksheets["File"]);
            dataSheet = new DataSheet(ThisAddIn.MyApp.Worksheets["Data"]);
            totalSheet = new TotalSheet(ThisAddIn.MyApp.Worksheets["Total"]);
            WBSsheets.Add(new WBSsheet(ThisAddIn.MyApp.Worksheets["WBS_1"]));
            EstimateSheets.Add(new EstimateSheet(ThisAddIn.MyApp.Worksheets["EST_1"]));
            correlationSheet = new CorrelationSheet(ThisAddIn.MyApp.Worksheets["Correlation"]);
            //Add to sheets collection
            sheets.Add(fileSheet);
            sheets.Add(dataSheet);
            sheets.Add(totalSheet);
            sheets.AddRange(WBSsheets);
            sheets.AddRange(EstimateSheets);
            sheets.Add(correlationSheet);
        }

        public void UpdateAllSheets()
        {
            foreach(Sheet sheet in this.sheets)
            {
                sheet.Format();
            }
        }
    }
}
