using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

/*
 *  MOST REFPARSER METHODS NEED REWRITTEN TO BUILD OFF OF THE DECOMP DICTIONARY
 */

namespace Utilities
{
    public enum RefType
    {
        A1,
        R1C1
    }

    public class RefParser
    {
        private enum RefParts
        {
            firstCell,
            secondCell,
            foreignSheet
        }
        private Dictionary<RefParts, string> refDecomp { get; set; }
        private Excel.Application MyApp { get; set; }
        private string reference { get; set; }
        public string firstColumn { get; set; }
        public int firstColumnNumber { get; set; }
        public string secondColumn { get; set; }
        public int secondColumnNumber { get; set; }
        public string firstRow { get; set; }
        public int firstRowNumber { get; set; }
        public string secondRow { get; set; }
        public int secondRowNumber { get; set; }
        public string A1_Range { get; set; }
        public string R1C1_Range { get; set; }
        public Excel.Range firstCell { get; set; }
        public Excel.Range secondCell { get; set; }

        public RefParser(string reference, Excel.Application MyApp, RefType refType = RefType.A1)       //constructor
        {
            this.reference = reference;
            this.MyApp = MyApp;
            refDecomp = DeCompReference(reference);       //decompose the reference. Need a different one for R1C1 or a conversion...
            this.firstCell = GetFirstCell();
            this.secondCell = GetSecondCell();
            
            firstColumn = GetFirstColumn();
            firstColumnNumber = TextToNumber(firstColumn);
            secondColumn = GetSecondColumn();
            secondColumnNumber = TextToNumber(secondColumn);
            firstRow = GetFirstRow();
            firstRowNumber = Convert.ToInt32(firstRow);
            secondRow = GetSecondRow();
            secondRowNumber = Convert.ToInt32(secondRow);
            if (refType == RefType.A1)
            {
                this.A1_Range = reference;
                this.R1C1_Range = this.ConvertRangeA1_R1C1(reference);
            }
            else if (refType == RefType.R1C1)
            {
                this.A1_Range = this.ConvertRangeR1C1_A1(reference);
                this.R1C1_Range = reference;
            }
            else
                throw new KeyNotFoundException();
        }

        private Dictionary<RefParts, string> DeCompReference(string reference)  //takes a raw reference and breaks it down into components
        {
            var refDecomp = new Dictionary<RefParts, string>();
            //remove garbage                    //=Sheet1!B12:D14
            reference = reference.Replace("=", "");
            reference = reference.Replace("$", "");
            if (reference.Contains('!'))        //Sheet1!B12:D14
            {
                //contains a foreignSheet
                string[] split_string = reference.Split('!');
                if(split_string.Length > 2)
                {
                    //Malformed reference handling
                    return null;
                }
                else
                {
                    refDecomp.Add(RefParts.foreignSheet, split_string[0]);
                    reference = split_string[1];
                }
            }
            if (reference.Contains(':'))   //B12:D14
            {
                //contains a second cell
                string[] split_string = reference.Split(':');
                if(split_string.Length != 2)
                {
                    //Malformed reference handling
                    return null;
                }
                else
                {
                    refDecomp.Add(RefParts.secondCell, split_string[1]);
                    reference = split_string[0];
                }
            }
            refDecomp.Add(RefParts.firstCell, reference);   //B12

            return refDecomp;
        }

        private Excel.Range GetFirstCell()
        {
            Excel.Worksheet refSheet;
            if (refDecomp.ContainsKey(RefParts.foreignSheet))
                refSheet = MyApp.Worksheets[refDecomp[RefParts.foreignSheet]];
            else
                refSheet = MyApp.ActiveSheet;

            if (refDecomp.ContainsKey(RefParts.firstCell))
            {
                //use refDecomp first cell to get row and column indexes
                string firstCellRef = refDecomp[RefParts.firstCell];
                var col_builder = new StringBuilder();
                var row_builder = new StringBuilder();
                foreach(char c in firstCellRef)
                {
                    if (Char.IsLetter(c))
                        col_builder.Append(c);
                    else if (Char.IsNumber(c))
                        row_builder.Append(c);
                }
                int column = TextToNumber(col_builder.ToString());
                int row = Convert.ToInt32(row_builder.ToString());
                return refSheet.Cells[row, column];
            }
            else
            {
                //this should always be here
                throw new Exception();
            }
            
        }

        private Excel.Range GetSecondCell()
        {
            Excel.Worksheet refSheet;
            if (refDecomp.ContainsKey(RefParts.foreignSheet))
                refSheet = MyApp.Worksheets[refDecomp[RefParts.foreignSheet]];
            else
                refSheet = MyApp.ActiveSheet;

            if (refDecomp.ContainsKey(RefParts.secondCell))
            {
                //use refDecomp first cell to get row and column indexes
                string secondCellRef = refDecomp[RefParts.secondCell];
                var col_builder = new StringBuilder();
                var row_builder = new StringBuilder();
                foreach (char c in secondCellRef)
                {
                    if (Char.IsLetter(c))
                        col_builder.Append(c);
                    else if (Char.IsNumber(c))
                        row_builder.Append(c);
                }
                int column = TextToNumber(col_builder.ToString());
                int row = Convert.ToInt32(row_builder.ToString());
                return refSheet.Cells[row, column];
            }
            else
            {
                return this.firstCell;        //no second cell reference given - so the range is 1 cell - return the first cell
            }
        }

        private string GetFirstColumn()     //A1
        {
            //Form: A1:B10
            StringBuilder sb = new StringBuilder();
            foreach(char c in this.reference)
            {
                if (c == '$')
                    continue;
                else if (Char.IsLetter(c))
                    sb.Append(c);
                else
                    return sb.ToString();
            }
            return sb.ToString();
        }
        private string GetSecondColumn()
        {
            if (!reference.Contains(":"))
                return "";
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
            if (!reference.Contains(":"))
                return "-1";
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
                if (c == '$' || c == '=')
                    continue;
                else if (char.IsLetter(c))
                    col.Append(c);
                else if (char.IsNumber(c))
                    row.Append(c);
                else { }
                    //throw new FormatException();
            }
            int colNumber = TextToNumber(col.ToString());
            int rowNumber = Convert.ToInt32(row.ToString());
            return $"R{rowNumber}C{colNumber}";
        }
        public string ConvertRangeA1_R1C1(string A1)
        {
            if (A1.Contains(":"))
                return $"{ConvertReferenceA1_R1C1(A1.Split(':')[0])}:{ConvertReferenceA1_R1C1(A1.Split(':')[1])}";
            else
                return ConvertReferenceA1_R1C1(A1);
        }
        public string ConvertReferenceR1C1_A1(string R1C1)
        {
            var splitString = R1C1.Split('C');
            splitString[0].TrimStart('R');
            string columnLetter = NumberToText(Convert.ToInt32(splitString[1]));
            return $"{columnLetter}{splitString[0]}";
        }
        public string ConvertRangeR1C1_A1(string R1C1)
        {
            return $"{ConvertReferenceR1C1_A1(R1C1.Split(':')[0])}:{ConvertReferenceR1C1_A1(R1C1.Split(':')[1])}";
        }
        private string NumberToText(int colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            int mod = 0;

            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }
            return colLetter;
        }
        private int TextToNumber(string text)
        {
            if (text == "")
                return -1;
            return text
                .Select(c => c - 'A' + 1)
                .Aggregate((sum, next) => sum * 26 + next);
        }
        
    }
}
