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
        public virtual void Format() { }

    }
}
