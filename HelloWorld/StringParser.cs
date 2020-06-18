using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class StringParser
    {
        public dynamic ConvertToNumber(dynamic cellValue)
        {
            if (cellValue != null)
            {
                if (cellValue is string)
                {
                    var returnString = new StringBuilder();
                    foreach (char c in cellValue)
                    {
                        int num = (int)c;
                        returnString.Append(num.ToString());
                    }
                    return returnString.ToString();
                }
                else
                    return cellValue;
            }
            else
                return null;
        }

    }
}
