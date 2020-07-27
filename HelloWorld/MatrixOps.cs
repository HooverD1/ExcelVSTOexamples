using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;
using Accord.Math.Decompositions;

namespace HelloWorld
{
    public static class MatrixOps
    {
        public static double MMult(double[] m1, double[] m2)        //one dimensional
        {
            if(m1.GetLength(0) == m2.GetLength(0))
            {
                double result = 0;
                for (int i = 0; i<m1.GetLength(0); i++)
                {
                    result += m1[i] * m2[i];
                }
                return result;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }
        public static double[,] MMult(double[,] m1, double[,] m2)
        {
            double[][] m1_rows = SplitOnRows(m1);
            double[][] m2_rows = SplitOnRows(Matrix.Transpose(m2));
            double[,] resultMatrix = new double[m2_rows.GetLength(0), m1.GetLength(1)];
            for(int row=0;row<m1.GetLength(0);row++)
            {
                for (int col = 0; col < m1.GetLength(0); col++)
                {
                    resultMatrix[row,col] = LinearCombination(m1_rows[row], m2_rows[col]);
                }
            }
            return resultMatrix;
        }

        private static double[][] SplitOnRows(double[,] input)
        {
            return Enumerable.Range(0, input.GetLength(0))
            .Select(x => Enumerable.Range(0, input.GetLength(1))
            .Select(y => input[x, y]).ToArray()).ToArray();
        }

        private static double LinearCombination(double[] row, double[] col)
        {
            double returnVal = 0;
            for(int i = 0; i < row.Length; i++)
            {
                returnVal += row[i] * col[i];
            }
            return returnVal;
        }   

        public static double Accord_MMult(double[] m1, double[] m2)
        {
            DiagnosticsMenu.StartStopwatch();
            var retVal = Matrix.Dot(m1, m2);
            DiagnosticsMenu.StopStopwatch();
            return retVal;
        }
        public static double[,] Accord_MMult(double[,] m1, double[,] m2)
        {
            DiagnosticsMenu.StartStopwatch();
            var retVal = Matrix.Dot(m1, m2);
            DiagnosticsMenu.StopStopwatch();
            return retVal;
        }
        public static double[,] MatrixInverse(double[,] m1)
        {
            return Matrix.Inverse(m1);
        }
        public static double[,] Transpose(double[,] m1)
        {
            return Matrix.Transpose<double>(m1);
        }
        public static double Determinant(double[,] m1)
        {
            return Matrix.Determinant(m1);
        }
        public static double[] RealEigenvalues_Accord(double[,] matrix)
        {
            return new EigenvalueDecomposition(matrix, false, true).RealEigenvalues;
        }

        //public static double[] RealEigenvalues_Manual(double[,] matrix)
        //{
        //    double A;
        //    double[] b;
        //    double[] dReal;
        //    double[] dImag;
        //    int n;
        //    int iErr;
        //    double dT;
        //}

        //private static double[,] Hessenberg(long n, double[,] matrix)
        //{
        //    long i, j;
        //    double dT1, dT2, dT;
        //    for(long k = 2; k < n; k++)
        //    {
        //        i = k;
        //        dT1 = 0;
        //        for(j=k; j <= n; j++)
        //        {
        //            if(Math.Abs(matrix[j, k-1]) > Math.Abs(dT1))
        //            {
        //                dT1 = matrix[j, k - 1];
        //                i = j;
        //            }

        //        }
        //        if (i != k)
        //        {
        //            for (j = k - 1; j < n; j++)
        //            {
        //                dT = matrix[i, j];
        //                matrix[i, j] = matrix[k, j];
        //                matrix[k, j] = dT;
        //            }
        //            for (j = 1; j < n; j++)
        //            {
        //                dT = matrix[j, i];
        //                matrix[j, i] = matrix[j, k];
        //                matrix[j, k] = dT;
        //            }
        //        }
        //        if(dT1 != 0)
        //        {
        //            for (i = k + 1; i < n; i++)
        //            {
        //                dT2 = matrix[i, k - 1];
        //                if (dT2 != 0)
        //                {
        //                    dT2 = dT2 / dT1;
        //                    matrix[i, k - 1] = dT2;
        //                    for (j = k; j < n; j++)
        //                    {
        //                        matrix[i, j] = matrix[i, j] - dT2 * matrix[k, j];
        //                    }
        //                    for (j = 1; j < n; j++)
        //                    {
        //                        matrix[j, k] = matrix[j, k] + matrix[j, i];
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return matrix;
        //}
        
    }
}
