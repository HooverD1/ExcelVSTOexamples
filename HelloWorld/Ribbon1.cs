using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace HelloWorld
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
        }

        private void btnHello_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("Hello World!");
        }

        private void btnAddFormulas_Click(object sender, RibbonControlEventArgs e)
        {
            ObjModel.SetFormulas("A1:B2", "=C3");       //relative references
            ObjModel.SetFormulas("A4:B5", "=$C$3");     //absolute references

            ObjModel.SetCell(5, "A6");
            ObjModel.SetFormulas("A7:A1000", "=A5+RandBetween(5,100)+Norm.Inv(.3,0,1)"); //add a formula to the sheet
        }

        private void btnWorksheetFunction_Click(object sender, RibbonControlEventArgs e)
        {
            ObjModel.GetSelection().Value = Utilities.wsFunction.Norm_Inv(.3, 0, 1);    //use a worksheetFunction

        }

        private void btnCopyFormats_Click(object sender, RibbonControlEventArgs e)
        {
            Utilities.CopyFormats();
        }
    }
}
