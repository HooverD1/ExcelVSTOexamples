﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class FileSheet: Sheet
    {
        public FileSheet(Excel.Worksheet ThisSheet) : base(ThisSheet) { }
        public override void Format()
        {
            base.Format();
        }
    }
}
