﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class TotalSheet: Sheet
    {
        public TotalSheet() : base()
        {
            this.AttachSheet(ThisAddIn.MyApp.Worksheets["Total"]);
        }

        public override void Format()
        {
            
        }
        
        public override void ReadFromSheet() { }
        public override void WriteToSheet() { }
    }
}
