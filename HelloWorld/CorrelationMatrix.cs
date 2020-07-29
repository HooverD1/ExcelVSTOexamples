using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics;
using Accord.Math.Decompositions;
using MathNet.Numerics;
using MStats = MathNet.Numerics.Statistics;

namespace HelloWorld
{
    public class CorrelationMatrix
    {
        public Correlation[,] Correlations { get; set; }
        public Estimate Parent {get;set;}
        public double[] eigenvalues { get; set; }
        //private double[] new_eigenvalues { get; set; }

        public CorrelationMatrix(Estimate Parent)
        {
            this.Parent = Parent;
            Correlations = GetCorrelationMatrix();
            DiagnosticsMenu.StartStopwatch();
            eigenvalues = GetEigenvalues(GetCoefficientsMatrix());
            DiagnosticsMenu.StopStopwatch(TimeUnit.seconds, true, "eigens");
            if (eigenvalues.Min() < 0)
            {
                AdjustToPositiveSemiDefinite();
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
            
            var inputs = this.Parent.Inputs;
            if (!inputs.Any())
                return null;
            double[,] correlData = new double[inputs[0].Data.Length, inputs.Count];   //[datapoint, input]
            double[] means = new double[inputs.Count];
            double[] stdevs = new double[inputs.Count];
            for(int i = 0; i < inputs.Count; i++)
            {
                means[i] = 0;
                stdevs[i] = 1;
                for(int j = 0; j < inputs[i].Data.Length; j++)
                {
                    correlData[j, i] = inputs[i].Data[j];
                }
            }
            double[,] correlCoefs;
            DiagnosticsMenu.StartStopwatch();
            /*  OPTIONS   */
            //correlCoefs = Measures.Correlation(correlData, means, stdevs);      //uses known mean and stdev
            correlCoefs = Measures.Correlation(correlData);                   //computes mean and stdev
            //correlCoefs = BuildCoefs(correlData);                             //uses mean=0 and stdev=1
            DiagnosticsMenu.StopStopwatch(TimeUnit.seconds, true, "correlation matrix generation");
            DiagnosticsMenu.StartStopwatch();
            Correlation[,] CorrelationMatrix = new Correlation[correlCoefs.GetLength(0),correlCoefs.GetLength(1)];
            for (int i = 0; i < inputs.Count; i++)
            {
                for (int j = 0; j < inputs.Count; j++)      //use i+1?
                {
                    CorrelationMatrix[i, j] = new Correlation(inputs[i], inputs[j], correlCoefs[i, j]);
                }
            }
            DiagnosticsMenu.StopStopwatch(TimeUnit.seconds, true, "Min/Max calculations");
            return CorrelationMatrix;
        }
        
        public void FormatMatrix(Excel.Range printCell)
        {
            Excel.Workbook templateBook = ThisAddIn.Model.GetTemplateBook();
            Excel.Range template = templateBook.Worksheets["Correl_Template"].range["C3"];
            Excel.Range target = printCell;
            Formatter fmter = new Formatter(template, target, (Excel.Worksheet)target.Parent);
            fmter.FormatRange();
            ThisAddIn.MyApp.DisplayAlerts = false;
            templateBook.Saved = true;
            templateBook.Close();
            ThisAddIn.MyApp.DisplayAlerts = true;
        }
        private double[,] BuildCoefs(double[,] data)        //for mean = 0, stdev = 1
        {
            double[,] correlationMatrix = new double[data.GetLength(1), data.GetLength(1)];
            for (int j = 0; j < data.GetLength(1); j++)         //input1
            {
                for (int j2 = 0; j2 < data.GetLength(1); j2++)   //input2
                {
                    double sum = 0;
                    for (int i = 0; i < data.GetLength(0); i++) //row
                    {
                        sum += data[i, j] * data[i, j2];
                    }
                    correlationMatrix[j2, j] = sum / (data.GetLength(0)-1);
                }
            }
            return correlationMatrix;
        }
    }
}
