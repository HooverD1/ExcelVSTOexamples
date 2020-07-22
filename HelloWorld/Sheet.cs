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

        public virtual void Format() { }        //virtuals can be overwritten. Abstracts have to be.
        private RangeType GetRangeType()    //Get the RangeType of some cell
        {
            throw new NotImplementedException();
        }
    }
}
