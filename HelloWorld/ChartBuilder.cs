using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class ChartBuilder
    {
        List<double> Data { get; set; }

        public void ExportToChart()
        {
            Excel.Worksheet sheet = ThisAddIn.HelperVBA.Application.Worksheets["ABC"];
            //print Data to ABC column A
            Excel.Chart chart = sheet.ChartObjects(0);
            chart.CopyPicture();
            Excel.Range pasteRange = ObjModel.GetSelection();
            pasteRange.PasteSpecial(Excel.XlPasteType.xlPasteAll);

        }
    }
}
