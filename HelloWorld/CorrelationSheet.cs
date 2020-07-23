using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class CorrelationSheet: Sheet
    {
        public Estimate Correlates { get; set; }     //things that are correlated - estimates and inputs
        public CorrelationSheet() : base() { }
        
        public override void Format()
        {
            base.Format();
        }
    }
}
