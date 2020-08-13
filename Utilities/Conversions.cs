using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Conversions
    {
        public static double[,] ConvertObjectArrayToDouble(object[,] objArray)
        {
            double[,] correlArray = new double[objArray.GetLength(0), objArray.GetLength(1)];
            for (int i = 1; i <= objArray.GetLength(0); i++)
            {
                for (int j = 1; j <= objArray.GetLength(1); j++)
                {
                    correlArray[i - 1, j - 1] = (double)objArray[i, j];
                }
            }
            return correlArray;
        }
    }
}
