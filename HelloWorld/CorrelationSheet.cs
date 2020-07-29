using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class CorrelationSheet : Sheet
    {
        public CorrelationMatrix CorrelationMatrix { get; set; }     //things that are correlated - estimates and inputs
        private Excel.Range MatrixTopLeft { get; set; }
        
        public CorrelationSheet() : base()
        {
            bool sheetFound = false;
            foreach(Excel.Worksheet sheet in ThisAddIn.MyApp.Worksheets)
            {
                if (sheet.Name == "Correlation")
                    sheetFound = true;
            }
            if (!sheetFound)
            {
                Excel.Worksheet correlSheet = ThisAddIn.MyApp.Worksheets.Add();
                correlSheet.Name = "Correlation";
            }
            this.AttachSheet(ThisAddIn.MyApp.Worksheets["Correlation"]);
            MatrixTopLeft = ThisSheet.Range["I4"];
        }

        public void PrintCorrelationMatrix()        //this should be within a sheet object I think..
        {
            ThisAddIn.MyApp.ScreenUpdating = false;
            Excel.Range printCell = this.MatrixTopLeft;
            int columns = GetCorrelationColCount();
            for(int row = 0; row < columns; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    printCell.Offset[row, -3].Value = CorrelationMatrix.eigenvalues[row];
                    printCell.Offset[row, col].Value = CorrelationMatrix.Correlations[row,col].Coefficient;
                }
            }
            CorrelationMatrix.FormatMatrix(printCell);
            for (int row = 0; row < columns; row++)
            {
                MatrixTopLeft.Offset[row, row].Value = CorrelationMatrix.Correlations[row, 0].input1.Name;
            }
            ThisAddIn.MyApp.ScreenUpdating = true;
        }
        public int GetCorrelationColCount()
        {
            int sum = 0;
            int this_sum = 0;
            double correlationCount = this.CorrelationMatrix.Correlations.GetLength(0) * this.CorrelationMatrix.Correlations.GetLength(1);
            correlationCount -= Math.Sqrt(correlationCount);
            correlationCount /= 2;

            while (sum <= correlationCount)
            {
                this_sum += 1;
                sum += this_sum;
            }
            return this_sum;
        }

        public override void Format() { }
        public override void ReadFromSheet() { }
        public override void WriteToSheet() { }
    }
}
