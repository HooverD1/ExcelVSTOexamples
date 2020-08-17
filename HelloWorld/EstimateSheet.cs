using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class EstimateSheet: Sheet
    {
        public EstimateSheet() : base()
        {
            //this.AttachSheet(ThisAddIn.MyApp.Worksheets["EST_1"]);      //this will need to be dynamic for multiple sheets
        }

        public List<Estimate> Estimates { get; set; }

        public override void Format()
        {
            
        }
        public static EstimateSheet Bootstrap(string ident)
        {
            var estimateSheet = new EstimateSheet();
            estimateSheet.AttachSheet(GetContainingSheet(ident, "Estimate"));
            return estimateSheet;
        }
        private static Excel.Worksheet GetContainingSheet(string ident, string type)
        {
            //find the possible worksheets
            IEnumerable<Excel.Worksheet> worksheets = from Excel.Worksheet wsht in ThisAddIn.MyApp.Worksheets
                                                      where wsht.Name.StartsWith("EST")
                                                      select wsht;
            if (worksheets.Count() >= 1)
            {
                //search the sheets for the sheet containing the ident string
                foreach (Excel.Worksheet sht in worksheets)
                {
                    Excel.Range search = sht.Cells.Find(ident);
                    if (search != null)
                        return sht;
                }
            }
            throw new KeyNotFoundException();       //Can't find the Ident in any sheet
        }
        public override void ReadFromSheet() { }
        public override void WriteToSheet() { }
    }
}
