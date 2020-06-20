using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class StringParser
    {
        private string reference { get; set; }
        public string firstColumn { get; set; }
        public int firstColumnNumber { get; set; }
        public string secondColumn { get; set; }
        public int secondColumnNumber { get; set; }
        public string firstRow { get; set; }
        public int firstRowNumber { get; set; }
        public string secondRow { get; set; }
        public int secondRowNumber { get; set; }

        public StringParser(string reference)       //constructor
        {
            this.reference = reference;
            firstColumn = GetFirstColumn();
            firstColumnNumber = TextToNumber(firstColumn);
            secondColumn = GetSecondColumn();
            secondColumnNumber = TextToNumber(secondColumn);
            firstRow = GetFirstRow();
            firstRowNumber = Convert.ToInt32(firstRow);
            secondRow = GetSecondRow();
            secondRowNumber = Convert.ToInt32(secondRow);
        }

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
        private string GetFirstColumn()     //A1
        {
            //Form: A1:B10
            StringBuilder sb = new StringBuilder();
            foreach(char c in this.reference)
            {
                if (Char.IsLetter(c))
                    sb.Append(c);
                else
                    return sb.ToString();
            }
            return sb.ToString();
        }
        private string GetSecondColumn()
        {
            StringBuilder sb = new StringBuilder();
            string secondText = reference.Split(':')[1];
            foreach(char c in secondText)
            {
                if (Char.IsLetter(c))
                    sb.Append(c);
                else
                    return sb.ToString();
            }
            return sb.ToString();
        }
        private string GetFirstRow()
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.reference)
            {
                if (Char.IsNumber(c))
                    sb.Append(c);
                if (c == ':')
                    return sb.ToString();
            }
            return sb.ToString();
        }
        private string GetSecondRow()
        {
            StringBuilder sb = new StringBuilder();
            string secondText = reference.Split(':')[1];
            foreach (char c in secondText)
            {
                if (Char.IsNumber(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }
        public int GetNumberOfColumns()
        {
            char[] firstCharArray = this.firstColumn.ToCharArray();
            char[] secondCharArray = this.secondColumn.ToCharArray();
            int firstLength = firstCharArray.GetLength(0);
            int secondLength = secondCharArray.GetLength(0);
            string[] numberArray = new string[secondLength];
            for(int i = 0; i < secondLength; i++)
            {
                if(firstLength - 1 >= i)
                {
                    numberArray[i] = ((int)secondCharArray[i] - (int)firstCharArray[i]).ToString();
                }
                else if (firstLength - 1 < i)
                {
                    numberArray[i] = ((int)secondCharArray[i]).ToString();
                }
                else
                    throw new Exception();  //this should never happen

            }
            return Convert.ToInt32(string.Join("", numberArray))+1;
        }
        public int GetNumberOfRows()
        {
            return Convert.ToInt32(secondRow) - Convert.ToInt32(firstRow) + 1;
        }

        //==============================================================UTILITIES================================================
        public string ConvertReferenceA1_R1C1(string A1)
        {
            StringBuilder col = new StringBuilder();
            StringBuilder row = new StringBuilder();
            foreach (char c in A1)
            {
                if (char.IsLetter(c))
                    col.Append(c);
                else if (char.IsNumber(c))
                    row.Append(c);
                else
                    throw new FormatException();
            }
            int colNumber = TextToNumber(col.ToString());
            int rowNumber = Convert.ToInt32(row.ToString());
            return $"R{rowNumber}C{colNumber}";
        }
        public string ConvertRangeA1_R1C1(string A1)
        {
            return $"{ConvertReferenceA1_R1C1(A1.Split(':')[0])}:{ConvertReferenceA1_R1C1(A1.Split(':')[1])}";
        }
        private int TextToNumber(string text)
        {
            return text
                .Select(c => c - 'A' + 1)
                .Aggregate((sum, next) => sum * 26 + next);
        }
        private string ConvertR1C1toA1(string R1C1)
        {
            throw new NotImplementedException();
        }
    }
}
