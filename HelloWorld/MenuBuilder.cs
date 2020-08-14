using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;

namespace HelloWorld
{
    public enum Region
    {
        Estimate,
        WBS,
        Other
    }

    public class MenuBuilder
    {
        private Utilities.RefParser Parser { get; set; }
        private Excel.Range Selection { get; set; }
        private Region CellRegion {get;set;}

        public MenuBuilder()
        {
            Selection = ObjModel.GetSelection();
            Parser = new Utilities.RefParser(Selection.Address, ThisAddIn.MyApp);
            CellRegion = GetRegion();
        }
        private Region GetRegion()
        {
            if (Parser.firstColumn == "A")
                return Region.Estimate;
            else if (Parser.firstColumn == "B")
                return Region.WBS;
            else
                return Region.Other;
        }
        public string Build()
        {
            if (CellRegion == Region.Estimate)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<menu xmlns=\"http://schemas.microsoft.com/office/2006/01/customui\">");
                sb.Append("<button id =\"button1\" getImage=\"GetImage\" label=\"Estimate\"/>");
                sb.Append("</menu>");
                return sb.ToString();
            }
            else if(CellRegion == Region.WBS)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<menu xmlns=\"http://schemas.microsoft.com/office/2006/01/customui\">");
                sb.Append("<button id =\"button1\" getImage=\"GetImage\" label=\"WBS\"/>");
                sb.Append("</menu>");
                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<menu xmlns=\"http://schemas.microsoft.com/office/2006/01/customui\">");
                sb.Append("<button id =\"button1\" getImage=\"GetImage\" label=\"Other\"/>");
                sb.Append("</menu>");
                return sb.ToString();
            }
        }
    }
}
