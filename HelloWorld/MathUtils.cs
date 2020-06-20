using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Tools.Excel;

namespace HelloWorld
{
    public static class MathUtils
    {
        public static string GetPrimeFactorization(dynamic in_number)
        {
            UInt64 number;
            try
            {
                number = Convert.ToUInt64(in_number);
            }
            catch (InvalidCastException)
            {
                return in_number+1;
            }
            catch (Exception)
            {
                return in_number;
            }
            
            var factorList = new List<UInt64>();
            factorList.Add(number);
            bool allPrime = false;
            while (allPrime == false)
            {
                var addList = new List<UInt64>();
                var removeList = new List<UInt64>();
                foreach (UInt64 factor in factorList)
                {
                    UInt64 max = (UInt64)Math.Floor(Math.Sqrt(factor));
                    for(UInt64 i = 2; i <= max; i++)
                    {
                        if (factor % i == 0)
                        {
                            addList.Add(i);
                            addList.Add(factor / i);
                            removeList.Add(factor);
                            break;
                        }
                    }
                }
                if (addList.Count == 0)
                    allPrime = true;
                foreach(UInt64 add in addList)
                {
                    factorList.Add(add);
                }
                foreach(UInt64 remove in removeList)
                {
                    factorList.Remove(remove);
                }
            }
            factorList.Sort();
            return string.Join(",", factorList);
        }
        public delegate string MyDelegate(NamedRange range);
        public static string Ones(NamedRange range)
        {
            return "=1";
        }
        public static void AddFormula(string formula)
        {
            //Range myrange = ObjModel.Get(GetOptions.SheetRange, ObjModel.Get(GetOptions.ActiveSheet), "A1");
            //MyDelegate mydel = Ones;
            //myrange.Formula(mydel(myrange));
            var sheet = ObjModel.Get(GetOptions.ActiveSheet);
            sheet.Cells[1, 1].Formula = formula;
        }

        private static double MyFunction(double x, double y)
        {
            return x * y;
        }

        public static double GetGradient()
        {
            Func<double, double, double> f = MyFunction;
            //double firstDeriv = Math.Fir
            return 0;
        }
    }
}
