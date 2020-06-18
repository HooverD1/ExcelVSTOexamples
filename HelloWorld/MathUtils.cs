using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public static class MathUtils
    {
        public static string GetPrimeFactorization(dynamic in_number)
        {
            int number;
            try
            {
                number = Convert.ToInt32(in_number);
            }
            catch (Exception)
            {
                return in_number;
            }
            
            var factorList = new List<int>();
            factorList.Add(number);
            bool allPrime = false;
            while (allPrime == false)
            {
                var addList = new List<int>();
                var removeList = new List<int>();
                foreach (int factor in factorList)
                {
                    int max = (int)Math.Floor(Math.Sqrt(factor));
                    for(int i = 2; i <= max; i++)
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
                foreach(int add in addList)
                {
                    factorList.Add(add);
                }
                foreach(int remove in removeList)
                {
                    factorList.Remove(remove);
                }
            }
            factorList.Sort();
            return string.Join(",", factorList);
        }
    }
}
