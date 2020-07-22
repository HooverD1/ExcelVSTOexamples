using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class CASE_Model
    {
        private List<Sheet> sheets { get; set; }
        public FileSheet fileSheet { get; set; }
        public DataSheet dataSheet { get; set; }
        public TotalSheet totalSheet { get; set; }
        public List<WBSsheet> WBSsheets { get; set; }
        public List<EstimateSheet> EstimateSheets { get; set; }
        public CorrelationSheet correlationSheet { get; set; }

        public CASE_Model()
        {
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
