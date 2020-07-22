using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

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

        public Sheet(Excel.Worksheet ThisSheet)
        {
            this.ThisSheet = ThisSheet;
        }

        public void ClearSheet()
        {
            this.ThisSheet.Cells.Clear();
        }

        public virtual void WriteSheet() { }        //Print the stored objects to the sheet
        public virtual void Format() { }        //virtuals can be overwritten (or not). Abstracts have to be overwritten.
        private RangeType GetRangeType()    //Get the RangeType of some cell
        {
            throw new NotImplementedException();
        }
    }
}
