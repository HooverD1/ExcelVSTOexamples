﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class WBSsheet: Sheet
    {
        public WBSsheet(Excel.Worksheet ThisSheet) : base(ThisSheet) { }
        public List<WBS> WBSs { get; set; }

        public override void Format()
        {
            base.Format();
        }
    }
}
