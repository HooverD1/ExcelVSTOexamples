using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;

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
        public static double[] MMult(double[,] m1, double[,] m2)
        {
            if (m1.GetLength(0) == m2.GetLength(0) && m1.GetLength(1) == m2.GetLength(1))
            {
                double[] result = new double[m1.GetLength(0)];
                for (int i = 0; i < m1.GetLength(0); i++)
                {
                    for(int j = 0; j < m1.GetLength(1); j++)
                    {
                        result[j] += m1[i, j] * m2[j, i];
                    }                    
                }
                return result;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }
        public static double Accord_MMult(double[] m1, double[] m2)
        {
            DiagnosticsMenu.StartStopwatch();
            var retVal = Matrix.Dot(m1, m2);
            DiagnosticsMenu.StopStopwatch();
            return retVal;
        }

    }
}
