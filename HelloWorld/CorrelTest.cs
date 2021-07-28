using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class CorrelTest
    {
        public Dictionary<string, int> FieldDict { get; set; }
        private object[,] MainArray { get; set; }
        private object[,] PartialArray { get; set; }
        private object[,] SecondaryArray { get; set; }
        private int Midpoint { get; set; }
        private bool IsEven { get; set; }
        private int FieldCount { get; set; }

        public CorrelTest(Excel.Range myRange)
        {
            this.FieldCount = myRange.Columns.Count;
            this.IsEven = Even(this.FieldCount);
            this.Midpoint = GetMidpoint(this.FieldCount, this.IsEven);
            MainArray = GetMainRange(myRange);
            SecondaryArray = GetSecondaryRange(myRange);
            FieldDict = GetFieldDict(myRange);
        }

        private bool Even(int fieldCount)
        {
            if (fieldCount % 2 == 0)
                return true;
            else
                return false;
            
        }
        private int GetMidpoint(int fieldCount, bool isEven)
        {
            if (isEven)
                return fieldCount / 2;
            else
                return fieldCount / 2 + 1;
        }

        private object[,] GetMainRange(Excel.Range myRange)
        {
            Excel.Range mainRange;
            Excel.Range mainFirstCell;
            Excel.Range mainLastCell;
            if (this.IsEven == true)
            {                
                mainFirstCell = myRange.Cells[1, this.Midpoint];
                mainLastCell = myRange.Cells[this.Midpoint, this.FieldCount];
            }
            else
            {
                mainFirstCell = myRange.Offset[0, this.Midpoint + 1];
                mainLastCell = myRange.Offset[this.Midpoint - 1, this.FieldCount];
            }
            mainRange = myRange.Range[mainFirstCell, mainLastCell];
            return mainRange.Value2;
        }

        private object[,] GetSecondaryRange(Excel.Range myRange)
        {
            Excel.Range secondRange;
            Excel.Range secondFirstCell;
            Excel.Range secondLastCell;
            if (this.IsEven == true)
            {
                secondFirstCell = myRange.Cells[1, 1];
                secondLastCell = myRange.Cells[this.Midpoint, this.Midpoint];
            }
            else
            {
                secondFirstCell = myRange.Offset[1, 1];
                secondLastCell = myRange.Offset[this.Midpoint, this.Midpoint];
            }
            secondRange = myRange.Range[secondFirstCell, secondLastCell];
            object[,] topLeft = secondRange.Value2;
            if (this.IsEven == true)
            {
                secondFirstCell = myRange.Cells[this.Midpoint+1, this.Midpoint+1];
                secondLastCell = myRange.Cells[this.FieldCount, this.FieldCount];
            }
            else
            {
                secondFirstCell = myRange.Offset[this.Midpoint+1, this.Midpoint+1];
                secondLastCell = myRange.Offset[this.FieldCount, this.FieldCount];
            }
            secondRange = myRange.Range[secondFirstCell, secondLastCell];
            object[,] bottomRight = secondRange.Value2;
            for(int row = this.Midpoint + 1; row<this.FieldCount; row++)
            {
                for(int col = row + 1; col< this.FieldCount; col++)
                {
                    var coords = this.TransformField(row, col);
                    topLeft[coords.Item2, coords.Item3] = bottomRight[row - this.Midpoint, col - this.Midpoint];
                }
            }
            return topLeft;
        }

        private Dictionary<string, int> GetFieldDict(Excel.Range myRange)
        {
            FieldDict = new Dictionary<string, int>();
            Excel.Range fieldStart = myRange.Offset[-1, 0];
            Excel.Range fieldEnd = myRange.Offset[myRange.Columns.Count, 0];
            Excel.Range fieldRange = myRange.Worksheet.Range[fieldStart, fieldEnd];
            object[,] fieldStrings = new object[1, this.FieldCount];
            fieldStrings = fieldRange.Value2;
            for (int i = 1; i <= this.FieldCount; i++)
            {
                FieldDict.Add(fieldStrings[1, i].ToString(), i);
            }
            return FieldDict;
        }
        
        public double AccessArray(string row, string col)
        {
            Tuple<Array, int, int> coords = TransformField(FieldDict[row], FieldDict[col]);
            if (coords.Item1 == Array.Main)
                return (double)MainArray[coords.Item2, coords.Item3];
            else if (coords.Item1 == Array.Secondary)
                return (double)SecondaryArray[coords.Item2, coords.Item3];
            else
                throw new Exception();
        }
        private enum Array
        {
            Main,
            Secondary
        }
        private Tuple<Array, int, int> TransformField(int rowIndex, int colIndex)
        {
            //Take a field name, check it's index with the dictionary, and transform it if need be

            if (this.IsEven == true)
            {
                if (rowIndex < this.Midpoint)
                {
                    if(colIndex > this.Midpoint)
                    {
                        //top right quadrant
                        return new Tuple<Array, int, int>(Array.Main, rowIndex, colIndex);
                    }
                    else
                    {
                        //top left quadrant
                        return new Tuple<Array, int, int>(Array.Secondary, rowIndex, colIndex);
                    }
                }
                else
                {
                    if(colIndex > this.Midpoint)
                    {
                        //bottom right quadrant
                        int newRowIndex = this.FieldCount - rowIndex;
                        int newColIndex = this.FieldCount - colIndex;
                        return new Tuple<Array, int, int>(Array.Secondary, newRowIndex, newColIndex);
                    }
                    else
                    {
                        //bottom left quadrant
                        //Convert to top right
                        return new Tuple<Array, int, int>(Array.Main, this.FieldCount - rowIndex, this.FieldCount - colIndex);
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
