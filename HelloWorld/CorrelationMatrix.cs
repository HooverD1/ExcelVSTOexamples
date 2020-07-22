using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics;
using Accord.Math.Decompositions;

namespace HelloWorld
{
    public class CorrelationMatrix
    {
        public Correlation[,] Correlations { get; set; }
        public IHasCorrelations Parent {get;set;}
        private double[] eigenvalues { get; set; }
        private double[] new_eigenvalues { get; set; }

        public CorrelationMatrix(IHasCorrelations Parent)
        {
            this.Parent = Parent;
            Correlations = GetCorrelationMatrix();
            eigenvalues = GetEigenvalues(GetCoefficientsMatrix());
            if (eigenvalues.Min() < 0)
            {
                AdjustToPositiveSemiDefinite();
                new_eigenvalues = GetEigenvalues(GetCoefficientsMatrix());
            }
        }
        public double[,] GetCoefficientsMatrix()
        {
            double[,] matrix = new double[Correlations.GetLength(0), Correlations.GetLength(1)];
            for(int i = 0; i < Correlations.GetLength(0);i++)
            {
                for(int j = 0; j < Correlations.GetLength(1); j++)
                {
                    matrix[i, j] = Correlations[i, j].Coefficient;
                }
            }
            return matrix;
        }
        public double[] GetEigenvalues(double[,] matrix)
        {
            return new EigenvalueDecomposition(matrix, false, true).RealEigenvalues;
        }
        public double[,] GetEigenvectors(double[,] matrix)
        {
            return new EigenvalueDecomposition(matrix, false, true).Eigenvectors;
        }
        public void AdjustToPositiveSemiDefinite()
        {
            int iNum = Correlations.GetLength(0);
            double minEigen = eigenvalues.Min();
            if (minEigen < 0)
            {
                for (int i = 0; i < iNum; i++)
                {
                    Correlations[i, i].Coefficient = Correlations[i, i].Coefficient - minEigen * 1.000000001;
                }
                for (int i = 0; i < iNum; i++)
                {
                    for (int j = 0; j < iNum; j++)
                    {
                        Correlations[i, j].Coefficient = Correlations[i, j].Coefficient / (1 - minEigen * 1.000000001);
                    }
                }
            }
        }
        public Correlation[,] GetCorrelationMatrix()   //this should take the data from the Estimate Inputs
        {
            DiagnosticsMenu.StartStopwatch();
            var inputs = this.Parent.Inputs;
            if (!inputs.Any())
                return null;
            double[,] correlData = new double[inputs[0].Data_Simulated.Length, inputs.Count];   //[datapoint, input]
            double[] means = new double[inputs.Count];
            double[] stdevs = new double[inputs.Count];
            for(int i = 0; i < inputs.Count; i++)
            {
                means[i] = 0;
                stdevs[i] = 0;
                for(int j = 0; j < inputs[i].Data_Simulated.Length; j++)
                {
                    correlData[j, i] = inputs[i].Data_Simulated[j];
                }
            }
            double[,] correlCoefs;
            correlCoefs = Measures.Correlation(correlData);
            
            
            DiagnosticsMenu.StopStopwatch(true, "Setup and Measures.Correlation time");
            DiagnosticsMenu.StartStopwatch();
            //double[,] correlCoefs = Measures.Correlation(correlData, means, stdevs);
            Correlation[,] CorrelationMatrix = new Correlation[correlCoefs.GetLength(0),correlCoefs.GetLength(1)];
            for (int i = 0; i < inputs.Count; i++)
            {
                for (int j = 0; j < inputs.Count; j++)
                {
                    CorrelationMatrix[i, j] = new Correlation(inputs[i], inputs[j], correlCoefs[i, j]);
                }
            }
            DiagnosticsMenu.StopStopwatch(true, "Time to crete Correlation objects - Min/Max, Eigens & PSD Adjust");
            return CorrelationMatrix;
        }
        public void PrintCorrelationMatrix()
        {
            ThisAddIn.MyApp.ScreenUpdating = false;
            Excel.Range printCell = ThisAddIn.MyApp.ActiveWorkbook.ActiveSheet.range["D1"];
            for(int i=0;i<Correlations.GetLength(0); i++)
            {
                printCell.Offset[i, -3].Value = eigenvalues[i];
                if(new_eigenvalues != null)
                    printCell.Offset[i, -2].Value = new_eigenvalues[i];
                for (int j=0;j<Correlations.GetLength(1); j++)
                {
                    printCell.Offset[i, j].Value = Correlations[i, j].Coefficient;
                }
            }
            ThisAddIn.MyApp.ScreenUpdating = true;
        }
    }
}
