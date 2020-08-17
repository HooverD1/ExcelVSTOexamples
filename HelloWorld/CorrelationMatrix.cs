using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class CorrelationMatrix
    {
        public struct Correlation
        {
            public string ChildIdent1 { get; set; }
            public string ChildIdent2 { get; set; }
            public double Coefficient { get; set; }
        }
        public double[,] Coefficients { get; set; }
        public Correlation[,] Correlations { get; set; }
        public ICorrelParent Parent { get; set; }
        public CorrelationSheet CorrelSheet { get; set; }

        public CorrelationMatrix(string correlString)                          //build object off of a correlationString cell value for pushing into a correlation worksheet
        {
            throw new NotImplementedException();
        }
        public CorrelationMatrix(CorrelationSheet CorrelSheet)                 //build object off of the correlation sheet for pushing back into a cell
        {
            this.CorrelSheet = CorrelSheet;
            //Does a correlation worksheet exist?       -- method on correlSheet object
            //Does a well-formed correlation matrix exist on the sheet?     -- method on correlSheet object
            //Bootstrap Parent object
            //Build Correlations array
            this.Correlations = BuildCorrelationArray(this.CorrelSheet);
            //this.Parent = FindParent(this.CorrelSheet);

        }


        private Correlation[,] BuildCorrelationArray(CorrelationSheet CorrelSheet)  //take what's on the correlation sheet and build the array
        {
            Excel.Worksheet CorrelWorksheet = CorrelSheet.ThisWorkSheet;    //Excel tab
            //CorrelSheet.MatrixTopLeft has the cell with the first cell of where the correlation matrix goes
            var top_left = CorrelSheet.MatrixTopLeft;
            int row_index = top_left.Row;
            int col_index = top_left.Column;
            Excel.Range bottom_right = top_left.End[Excel.XlDirection.xlToRight].End[Excel.XlDirection.xlDown];
            Excel.Range matrix_range = CorrelWorksheet.Range[top_left, bottom_right];
            Correlation[,] returnArray = new Correlation[matrix_range.Rows.Count, matrix_range.Columns.Count];
            //does this need each position initialized?
            for(int r = 0; r < matrix_range.Rows.Count; r++)
            {
                for(int c = 0; c < matrix_range.Columns.Count; c++)
                {
                    returnArray[r, c].Coefficient = CorrelWorksheet.Range[row_index + r, col_index + c].Value;
                    returnArray[r, c].ChildIdent1 = CorrelWorksheet.Range[row_index + r, col_index].Value;
                    returnArray[r, c].ChildIdent2 = CorrelWorksheet.Range[row_index, col_index + c].Value;
                }
            }
            return returnArray;
        }        

        public string CreateCorrelString()
        {
            throw new NotImplementedException();
        }

        public void PrintToCorrelSheet()
        {
            if (ThisAddIn.Model.correlationSheet == null)
            {
                ThisAddIn.Model.correlationSheet = new CorrelationSheet();
                //check if a worksheet exists? Should that be implicit in creating a correlationSheet object? Probably
            }
            // CODE TO Print necessary object data to the correlation sheet here
            throw new NotImplementedException();
        }
    }
}
