using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class DataSheet: Sheet
    {
        public DataSheet(): base(){ }

        public override void Format()
        {
            
        }
        public override void ReadFromSheet() { }
        public override void WriteToSheet() { }
    }
}
