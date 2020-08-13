using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace HelloWorld
{
    public class CASE_Model
    {
        public Excel.Workbook TemplateBook { get; set; }
        private List<Sheet> sheets = new List<Sheet>();
        public FileSheet fileSheet { get; set; }
        public DataSheet dataSheet { get; set; }
        public TotalSheet totalSheet { get; set; }
        public List<WBSsheet> WBSsheets = new List<WBSsheet>();
        public List<EstimateSheet> EstimateSheets = new List<EstimateSheet>();
        public CorrelationSheet correlationSheet { get; set; }

        public void SetupModel()
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
            fileSheet = new FileSheet();
            
            dataSheet = new DataSheet();
            dataSheet.AttachSheet(ThisAddIn.MyApp.Worksheets["Data"]);
            totalSheet = new TotalSheet();
            totalSheet.AttachSheet(ThisAddIn.MyApp.Worksheets["Total"]);
            WBSsheet newWBS = new WBSsheet();
            WBSsheets.Add(newWBS);
            newWBS.AttachSheet(ThisAddIn.MyApp.Worksheets["WBS_1"]);
            EstimateSheet newEstimateSheet = new EstimateSheet();
            EstimateSheets.Add(newEstimateSheet);
            newEstimateSheet.AttachSheet(ThisAddIn.MyApp.Worksheets["EST_1"]);
            correlationSheet = new CorrelationSheet();
            correlationSheet.AttachSheet(ThisAddIn.MyApp.Worksheets["Correlation"]);
            //Add to sheets collection
            sheets.Add(fileSheet);
            sheets.Add(dataSheet);
            sheets.Add(totalSheet);
            sheets.AddRange(WBSsheets);
            sheets.AddRange(EstimateSheets);
            sheets.Add(correlationSheet);

            fileSheet.Format();
        }

        public void UpdateAllSheets()
        {
            foreach(Sheet sheet in this.sheets)
            {
                sheet.Format();
            }
        }
        public Excel.Workbook GetTemplateBook()
        {
            string path = fileSheet.Settings.TemplatePath;
            if (TemplateBook == null)
                return Hello_Utilities.OpenWorkbook(path, false, false);
            else
                return TemplateBook;
        }
    }
}
