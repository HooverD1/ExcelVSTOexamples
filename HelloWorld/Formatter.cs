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
            ThisAddIn.MyApp.DisplayAlerts = false;
            ThisAddIn.MyApp.ScreenUpdating = false;
            Excel.Range newTargetRange = targetCell.Offset[0,1];
            Excel.Range newMergeRange = targetCell;
            Excel.Range newHeaderRange = targetCell;
            Utilities.RefParser targetParser = new Utilities.RefParser(targetRange.Address, ThisAddIn.MyApp);
            int rowNum_start = targetParser.firstRowNumber;
            int colNum_start = targetParser.firstColumnNumber;
            int rowNum_end = targetParser.secondRowNumber;
            int colNum_end = targetRange.End[Excel.XlDirection.xlToRight].Column;
            int shift = colNum_start;
            Excel.Range formatRange;
            ThisAddIn.MyApp.ScreenUpdating = false;
            int rangeCount = 0;
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
                Excel.Range headerRange = targetSheet.Range[row.Cells[1, shift], row.Cells[1, shift]];
                newHeaderRange = ThisAddIn.MyApp.Union(newHeaderRange, headerRange);
                mergeRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                rangeCount += 3;
                if (rangeCount > 240)
                {
                    ExecuteFormat(newTargetRange, newMergeRange, newHeaderRange, templateRange);
                    rangeCount = 0;
                    newTargetRange = targetCell.Offset[0, 1];       //resets the union ranges back to their defaults
                    newMergeRange = targetCell;
                    newHeaderRange = targetCell;
                }
                shift++;
            }
            ExecuteFormat(newTargetRange, newMergeRange, newHeaderRange, templateRange);    //execute the final batch
            ThisAddIn.MyApp.DisplayAlerts = true;
            ThisAddIn.MyApp.ScreenUpdating = true;
        }

        private void ExecuteFormat(Excel.Range newTargetRange, Excel.Range newMergeRange, Excel.Range newHeaderRange, Excel.Range templateRange)
        {
            newMergeRange.Clear();
            templateRange.Copy();
            newTargetRange.PasteSpecial(Excel.XlPasteType.xlPasteFormats);
            foreach (Excel.Range cell in newHeaderRange)
                cell.Formula = targetSheet.Range[$"A{cell.Row}"].Formula;
        }

        public static void ResetSheet()
        {
            ObjModel.GetActiveSheet().Cells.Clear();
        }
    }
}
