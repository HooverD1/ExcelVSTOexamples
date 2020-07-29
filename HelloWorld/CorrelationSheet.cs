using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

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
            MatrixTopLeft = ThisSheet.Range["H4"];

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
                    //printCell.Offset[row, -3].Value = CorrelationMatrix.eigenvalues[row];
                    printCell.Offset[row, col].Value = CorrelationMatrix.Correlations[row,col].Coefficient;
                }
            }
            CorrelationMatrix.FormatMatrix(printCell);
            for (int row = 0; row < columns; row++)
            {
                Excel.Range header = MatrixTopLeft.Offset[-1, row];
                if(row>0)
                    header.Orientation = Excel.XlOrientation.xlUpward;
                header.Value = CorrelationMatrix.Correlations[0, row].input2.Name;
                MatrixTopLeft.Offset[row, row].Value = CorrelationMatrix.Correlations[row, 0].input1.Name;
            }
            printCell.Offset[-1, -7].Value = "Distribution";
            printCell.Offset[-1, -6].Value = "Param1";
            printCell.Offset[-1, -5].Value = "Param2";
            printCell.Offset[-1, -4].Value = "Param3";
            printCell.Offset[-1, -3].Value = "Param4";
            printCell.Offset[-1, -2].Value = "Param5";
            printCell.Offset[-1, -1].Value = "Location";
            printCell.Offset[-1, 0].Value = "Variable Name";
            ThisAddIn.MyApp.ScreenUpdating = true;
        }
        private int GetCorrelationColCount()
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
        public bool CheckTransitivity()
        {
            foreach(Correlation correl in CorrelationMatrix.Correlations)
            {
                if(Math.Round(correl.Coefficient, 10) > correl.TransitivityMax)
                {
                    MessageBox.Show($"{correl.input1.Name} and {correl.input2.Name} fail max; {correl.Coefficient}");
                    return false;
                }
                else if(Math.Round(correl.Coefficient, 10) < correl.TransitivityMin)
                {
                    MessageBox.Show($"{correl.input1.Name} and {correl.input2.Name} fail min; {correl.Coefficient}");
                    return false;
                }
            }
            return true;
        }

        public override void Format() { }
        public override void ReadFromSheet() { }
        public override void WriteToSheet() { }
    }
}
