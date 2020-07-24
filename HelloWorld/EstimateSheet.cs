﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class EstimateSheet: Sheet
    {
        public EstimateSheet() : base()
        {
            this.AttachSheet(ThisAddIn.MyApp.Worksheets["EST_1"]);      //this will need to be dynamic for multiple sheets
        }

        public List<Estimate> Estimates { get; set; }

        public override void Format()
        {
            
        }
        public override void ReadFromSheet() { }
        public override void WriteToSheet() { }
    }
}
