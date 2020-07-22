using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Accord.Statistics;

namespace HelloWorld
{
    public class CorrelationMatrix
    {
        public Correlation[,] Correlations { get; set; }

        public double[] GetEigenvalues(double[,] matrix)
        {
            throw new NotImplementedException();
        }
        public void GetEigenvectors(double[,] matrix)
        {
            throw new NotImplementedException();
        }
        public double[,] AdjustToPositiveSemiDefinite()
        {
            throw new NotImplementedException();
        }
        public Correlation[,] GetCorrelationMatrix(Excel.Range dataTable, bool headers=false)
        {
            var headerRow = dataTable.Rows[0];
            Excel.Range dataRange = null;
            foreach(Excel.Range cell in dataRange)
            {
                if(ThisAddIn.MyApp.Intersect(cell,headerRow) == null)
                {
                    dataRange = ThisAddIn.MyApp.Union(cell, dataRange);
                }
            }
            double[,] dataDoubles = Utilities.ConvertObjectArrayToDouble(dataRange.Value);
            double[,] coefficients = Measures.Correlation(dataDoubles);
            Correlation[,] correlationMatrix = new Correlation[coefficients.GetLength(0), coefficients.GetLength(1)];
            for(int i = 0; i < coefficients.GetLength(0); i++)  //input1
            {
                for (int j = 0; j < coefficients.GetLength(0); j++) //input2
                {
                    double[] input1Data = new double[coefficients.GetLength(1)];
                    //need to load this
                    double[] input2Data = new double[coefficients.GetLength(1)];
                    //need to load this
                    Correlation newCorrel = new Correlation(new EstimateInput(input1Data), new EstimateInput(input2Data));
                    correlationMatrix[i, j] = newCorrel;
                    newCorrel.Calculate();
                }
            }
            return null;
        }
    }
}
