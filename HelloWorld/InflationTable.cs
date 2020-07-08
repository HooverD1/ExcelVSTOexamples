using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using System.Data;
using System.Windows.Forms;
using System.Runtime.Serialization;


namespace HelloWorld
{
    [Serializable]
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
        public enum InflValue
        {
            Weighted,
            Raw
        }
        public struct Entry
        {
            public int Year;
            public int Category;
            public InflValue ValueType;
            public double Value;
        }
        public List<Entry> entries { get; set; }
        public int agencyNumber { get; set; }
        public int category { get; set; }
        //protected Dictionary<int, double> rawTable { get; set; }      //<year, inflation raw weight>
        //protected Dictionary<int, double> weightTable { get; set; }   //<year, inflation weighted weight>
        
        public void SetAgency(int agencyNumber)
        {
            this.agencyNumber = agencyNumber;
        }
        public void UpdateTable()       //pulls the table in from the Excel, sets up the table object, and pushes it to xml
        {
            MessageBox.Show("Loading table");
            //Grab table from the excel
            this.entries = new List<Entry>();

            if (!File.Exists(@"C:\Users\grins\source\repos\HelloWorld\HelloWorld\inflation_table.xlsx"))
                throw new FileNotFoundException();
            Excel.Workbook inflBook = Utilities.OpenWorkbook(@"C:\Users\grins\source\repos\HelloWorld\HelloWorld\inflation_table.xlsx", true);
            var sheetsCount = inflBook.Worksheets.Count;
            Excel.Worksheet sheet = inflBook.Worksheets[agencyNumberToName[agencyNumber]];
            object[,] table = (object[,])sheet.UsedRange.Value2;
            //object[,] values = (object[,])range.Value2;
            int categories = (table.GetLength(1) - 1) / 2;
            int rows = table.GetLength(0);
            for(int j=3;j<=rows;j++)      //Iterate the range via indexes instead
            {
                for (int i = 1; i <= categories; i++)
                {
                    //create an Entry for each category and year
                    entries.Add(new Entry { Year = Convert.ToInt32(table[j,1]), Category = i, ValueType = InflValue.Raw, Value = Convert.ToDouble(table[j, i * 2]) });
                    entries.Add(new Entry { Year = Convert.ToInt32(table[j, 1]), Category = i, ValueType = InflValue.Weighted, Value = Convert.ToDouble(table[j, i * 2+1]) });
                }
            }
            inflBook.Close();
            this.SerializeTable();      
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

        public void SerializeTable()
        {
            Serializer.SerializeObject<InflationTable>(this, @"C:\Users\grins\source\repos\HelloWorld\HelloWorld\test_xml.xml");
        }
        public InflationTable DeserializeTable()
        {
            return Serializer.ReadXML<InflationTable>(@"C:\Users\grins\source\repos\HelloWorld\HelloWorld\test_xml.xml");
        }
    }
}
