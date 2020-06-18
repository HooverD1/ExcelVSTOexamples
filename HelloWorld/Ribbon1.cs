using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Interop.Excel;
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
            ObjModel.SetSelection("Hello World");
        }

        private void btnConvert_Click(object sender, RibbonControlEventArgs e)
        {
            var converter = new StringParser();
            var cellValue = ObjModel.Get(GetOptions.SelectionValue);
            ObjModel.SetSelection(converter.ConvertToNumber(cellValue));
            //only convert cells containing text
        }
    }
}
