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
            m2 = Matrix.Transpose(m2);
            double[,] resultMatrix = new double[m2.GetLength(0), m1.GetLength(1)];
            for(int row=0;row<m1.GetLength(0);row++)
            {
                for (int col = 0; col < m1.GetLength(0); col++)
                {
                    resultMatrix[row,col] = LinearCombination(m1.GetRow(row), m2.GetRow(col));
                }
            }
            return resultMatrix;
        }

        private static double LinearCombination(double[] row, double[] col)
        {
            double returnVal = 0;
            for(int i = 0; i < row.Length; i++)
                returnVal += row[i] * col[i];
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
        
    }
}
