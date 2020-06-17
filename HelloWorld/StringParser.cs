using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class StringParser
    {
        public string ConvertToNumber(string cellValue)
        {
            var returnString = new StringBuilder();
            foreach (char c in cellValue)
            {
                int num = Math.Abs(c - '0');
                returnString.Append(num.ToString());
            }
            return returnString.ToString();
        }

    }
}
