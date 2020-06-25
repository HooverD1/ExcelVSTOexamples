using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using System.Data;

namespace HelloWorld
{
    public class InflationTable
    {
        private static Dictionary<int, string> agencyNumberToName = new Dictionary<int, string>()
        {
            {1, "US Air Force (AF) 2020"},
            {2, "US Army 2020"},
            {3, "US Bur. Labor Stats. (BLS) 2020"},
            {4, "MDA 2020"},
            {5, "US NASA 2020"},
            {6, "US Navy"},
            {7, "Previous MDA 2019"}
        };
        private enum InflValue
        {
            Weighted,
            Raw
        }
        private struct Entry
        {
            public int Year;
            public int Category;
            public InflValue ValueType;
            public double Value;
        }
        private List<Entry> entries { get; set; }
        public int agencyNumber { get; set; }
        public int category { get; set; }
        private Dictionary<int, double> rawTable { get; set; }      //<year, inflation raw weight>
        private Dictionary<int, double> weightTable { get; set; }   //<year, inflation weighted weight>
        public InflationTable(int agencyNumber)
        {
            this.agencyNumber = agencyNumber;
        }
        public void UpdateTable()
        {
            //Grab table from the excel
            this.entries = new List<Entry>();
            var inflBook = ThisAddIn.MyApp.Workbooks.Open(@"C:\Users\grins\Documents\VSTO Research\InflationTables.xlsx");
            Excel.Worksheet sheet = inflBook.Worksheets[agencyNumberToName[agencyNumber]];
            Excel.Range table = sheet.UsedRange;
            int categories = (table.Columns.Count - 1)/2;
            foreach(Excel.Range cell in table.Columns[0])      //Iterate the cells in the first column
            {
                if (cell.Row < 3)
                    continue;
                for(int i = 1; i <= categories; i++)
                {
                    //create an Entry for each category and year
                    entries.Add(new Entry { Year = cell.Value, Category = i, ValueType = InflValue.Raw, Value = table.Cells[cell.Row, i * 2] });
                    entries.Add(new Entry { Year = cell.Value, Category = i, ValueType = InflValue.Weighted, Value = table.Cells[cell.Row, i * 2 + 1] });
                }
            }
        }
        private DataTable MakeTableFromRange(Excel.Range range)     //converting to a Datatable is an alternative way to store the table
        {
            throw new NotImplementedException();
        }
        public double GetRawValue(int category, int year)
        {
            var returnValue = from Entry e in this.entries
                              where e.Category == category && e.Year == year && e.ValueType == InflValue.Raw
                              select e;
            return returnValue.First().Value;
        }
        public double GetWeightedValue(int category, int year)
        {
            var returnValue = from Entry e in this.entries
                              where e.Category == category && e.Year == year && e.ValueType == InflValue.Weighted
                              select e;
            return returnValue.First().Value;
        }
    }
}
