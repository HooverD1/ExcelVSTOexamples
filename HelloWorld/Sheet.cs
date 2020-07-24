using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace HelloWorld
{
    public abstract class Sheet
    {
        protected enum RangeType
        {
            sum,
            input,
            header,
            border
        }
        
        private Dictionary<RangeType, Excel.Range> format_template { get; set; }       //<range context type, template range>
        public Excel.Worksheet ThisSheet { get; set; }

        public void AttachSheet(Excel.Worksheet ThisSheet)
        {
            this.ThisSheet = ThisSheet;
        }

        public void ClearSheet()
        {
            if (this.ThisSheet != null)
                this.ThisSheet.Cells.Clear();
            else
                MessageBox.Show("No sheet attached to object.");
        }

        public abstract void WriteToSheet();        //Print the stored objects to the sheet
        public abstract void ReadFromSheet();
        public abstract void Format();        //virtuals can be overwritten (or not). Abstracts have to be overwritten.
        private RangeType GetRangeType()    //Get the RangeType of some cell (like right click context)
        {
            throw new NotImplementedException();
        }
    }
}
