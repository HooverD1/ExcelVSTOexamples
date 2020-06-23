using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void btnHello_Click(object sender, RibbonControlEventArgs e)
        {
            Utilities.MsgBox("Hello World!");
        }

        private void btnAddFormulas_Click(object sender, RibbonControlEventArgs e)
        {
            ObjModel.SetFormulas("A1:B2", "=C3");
            ObjModel.SetFormulas("A4:B5", "=$C$3");
        }

        private void btnWorksheetFunction_Click(object sender, RibbonControlEventArgs e)
        {
            ObjModel.GetSelection().Value = Utilities.wsFunction.Norm_Inv(.3, 0, 1);
        }

        private void btnCopyFormats_Click(object sender, RibbonControlEventArgs e)
        {
            Utilities.CopyFormats();
        }
    }
}
