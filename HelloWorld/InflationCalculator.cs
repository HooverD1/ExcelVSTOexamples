using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public enum InflationMode
    {
        BY_to_BY,
        BY_to_TY,
        TY_to_BY,
        TY_to_TY
    }
    struct TableID
    {
        public int agency;
        public int category;
    }

    public static class InflationCalculator
    {
        private static Dictionary<TableID, InflationTable> InflationTables { get; set; } //<agencyNumber, agency's table>
        static InflationCalculator()
        {
            InflationTables = new Dictionary<TableID, InflationTable>();   //initialize property
        }
        private static TableID GetTable(int agencyNumber, int thisCategory)
        {
            var tid = new TableID { agency = agencyNumber, category = thisCategory };
            if (!InflationTables.ContainsKey(tid)) //if the table is not yet in the dictionary, retrieve it
            {
                InflationTables.Add(tid, new InflationTable(agencyNumber));
                InflationTables[tid].UpdateTable();
            }
            return new TableID { agency = agencyNumber, category = thisCategory };
        }
        public static void Calculate(int year1, int year2, InflationMode mode, int category, int agency)
        {
            int modeInteger;
            switch (mode)
            {
                case InflationMode.BY_to_BY:
                    modeInteger = 1;
                    break;
                case InflationMode.BY_to_TY:
                    modeInteger = 2;
                    break;
                case InflationMode.TY_to_BY:
                    modeInteger = 3;
                    break;
                case InflationMode.TY_to_TY:
                    modeInteger = 4;
                    break;
                default:
                    throw new KeyNotFoundException();
            }
            Calculate(year1, year2, modeInteger, category, agency);
        }   //Allow enums for inputs
        public static double Calculate(int year1, int year2, int mode, int category, int agency)
        {
            TableID tid = GetTable(agency, category);       //loads the table if it hasn't been loaded yet
            switch (mode)
            {
                case 1:
                    return Calculate_BY_to_BY(year1, year2, tid);
                case 2:
                    return Calculate_BY_to_TY(year1, year2, tid);
                case 3:
                    return Calculate_TY_to_BY(year1, year2, tid);
                case 4:
                    return Calculate_TY_to_TY(year1, year2, tid);
                default:
                    throw new KeyNotFoundException();
            }
        }
        private static double Calculate_BY_to_BY(int year1, int year2, TableID tid)
        {
            return InflationTables[tid].GetRawValue(year1);      //need to complete these calculations
        }
        private static double Calculate_BY_to_TY(int year1, int year2, TableID tid)
        {
            return InflationTables[tid].GetRawValue(year1);
        }
        private static double Calculate_TY_to_BY(int year1, int year2, TableID tid)
        {
            return InflationTables[tid].GetRawValue(year1);
        }
        private static double Calculate_TY_to_TY(int year1, int year2, TableID tid)
        {
            return InflationTables[tid].GetRawValue(year1);
        }
    }
}
