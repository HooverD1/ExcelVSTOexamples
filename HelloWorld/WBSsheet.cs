using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class WBSsheet: Sheet
    {
        public WBSsheet() : base()
        {
            this.AttachSheet(ThisAddIn.MyApp.Worksheets["WBS_1"]);
        }
        public List<WBS> WBSs { get; set; }

        public override void Format() { }
        public override void ReadFromSheet() { }
        public override void WriteToSheet() { }
    }
}
