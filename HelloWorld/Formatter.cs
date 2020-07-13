using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;

namespace HelloWorld
{
    public class Formatter
    {
        //Format correlation matrix
        
        private Excel.Range templateRange { get; set; }
        private Excel.Range targetRange { get; set; }
        private Excel.Worksheet targetSheet { get; set; }
        private Excel.Range targetCell { get; set; }
        public Formatter(Excel.Range templateRange, Excel.Range targetCell, Excel.Worksheet targetSheet)
        {
            this.targetCell = targetCell;
            this.templateRange = templateRange;
            //this.targetRange = targetCell;
            this.targetSheet = targetSheet;
            this.targetRange = ExpandTargetCell(targetCell);
            
        }

        private Excel.Range ExpandTargetCell(Excel.Range targetCell)
        {
            Excel.Range startCell = targetCell;
            Excel.Range endCell = targetRange;
            endCell = targetCell.End[Excel.XlDirection.xlDown];
            endCell = endCell.End[Excel.XlDirection.xlToRight];
            return targetSheet.Range[startCell, endCell];
        }
        public void FormatRange()
        {
            /*
             Loop the rows to build the target range and clear range with Union
             Paste formats in the target range
             */
            DiagnosticsMenu.StartStopwatch();
            Excel.Range newTargetRange = targetCell.Offset[0,1];
            Excel.Range newMergeRange = targetCell;
            RefParser targetParser = new RefParser(targetRange.Address);
            int rowNum_start = targetParser.firstRowNumber;
            int colNum_start = targetParser.firstColumnNumber;
            int rowNum_end = targetParser.secondRowNumber;
            int colNum_end = targetRange.End[Excel.XlDirection.xlToRight].Column;
            //int colNum_end = targetParser.secondColumnNumber;   //this is bugged
            string r1c1_end_ref = $"R{rowNum_end}C{colNum_end}";
            int rowCount = targetRange.Rows.Count;
            int shift = colNum_start;
            Excel.Range formatRange;
            for (int i = rowNum_start;i<=rowNum_end;i++)
            {       
                Excel.Range row = targetSheet.Range[$"A{i}"].EntireRow;
                if (shift + 1 <= colNum_end)
                    formatRange = targetSheet.Range[row.Cells[1, shift + 1], row.Cells[1, colNum_end]];
                else
                    formatRange = targetCell.Offset[0,1];
                Excel.Range mergeRange = targetSheet.Range[row.Cells[1, colNum_start], row.Cells[1, shift]];
                newTargetRange = ThisAddIn.MyApp.Union(newTargetRange, formatRange);
                newMergeRange = ThisAddIn.MyApp.Union(newMergeRange, mergeRange);
                mergeRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                shift++;
            }
            templateRange.Copy();
            newTargetRange.PasteSpecial(Excel.XlPasteType.xlPasteFormats);
            ThisAddIn.MyApp.DisplayAlerts = false;

            DiagnosticsMenu.StopStopwatch(true);
            DiagnosticsMenu.StartStopwatch();
            foreach (Excel.Range row in newMergeRange.Rows)
            {
                row.Merge();
                row.Value = "Hello";
            }
            ThisAddIn.MyApp.DisplayAlerts = true;
            DiagnosticsMenu.StopStopwatch(true);
        }

        public static void ResetSheet()
        {
            ObjModel.GetActiveSheet().Cells.Clear();
        }
    }
}
