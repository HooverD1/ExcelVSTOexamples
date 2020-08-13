using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Primer
{
    public enum InflationMode
    {
        BY_to_BY,
        BY_to_TY,
        TY_to_BY,
        TY_to_TY
    }

    public class InflationCalculator
    {
        private Dictionary<int, InflationTable> InflationTables { get; set; }    //<agencyNumber, agency's table>
        public InflationCalculator()
        {
            InflationTables = new Dictionary<int, InflationTable>();                    //initialize property
        }
        private void GetTable(int agencyNumber, int thisCategory)
        {
            if (!InflationTables.ContainsKey(agencyNumber))                                          //if the table is not yet in the dictionary, retrieve it
            {
                InflationTable newTable = new InflationTable();
                InflationTables.Add(agencyNumber, newTable);
                if (!newTable.SetAgency(agencyNumber))      //If this fails, kill GetTable()
                    return;
                InflationTables[agencyNumber].UpdateTable();
            }
            
        }

        public void Calculate(int year1, int year2, InflationMode mode, int category, int agency)
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
        public double Calculate(int year1, int year2, int mode, int category, int agency)
        {
            GetTable(agency, category);       //loads the table if it hasn't been loaded yet
            switch (mode)
            {
                case 1:
                    return Calculate_BY_to_BY(year1, year2, category, agency);
                case 2:
                    return Calculate_BY_to_TY(year1, year2, category, agency);
                case 3:
                    return Calculate_TY_to_BY(year1, year2, category, agency);
                case 4:
                    return Calculate_TY_to_TY(year1, year2, category, agency);
                default:
                    throw new KeyNotFoundException();
            }            
        }
        private double Calculate_BY_to_BY(int year1, int year2, int category, int agency)
        {
            double raw1 = InflationTables[agency].GetRawValue(category, year1);
            double raw2 = InflationTables[agency].GetRawValue(category, year2);
            return raw2 / raw1;
        }
        private double Calculate_BY_to_TY(int year1, int year2, int category, int agency)
        {
            double raw1 = InflationTables[agency].GetRawValue(category, year1);
            double weighted2 = InflationTables[agency].GetWeightedValue(category, year2);
            return weighted2 / raw1;
        }
        private double Calculate_TY_to_BY(int year1, int year2, int category, int agency)
        {
            double weighted1 = InflationTables[agency].GetWeightedValue(category, year1);
            double raw2 = InflationTables[agency].GetRawValue(category, year2);     
            return raw2 / weighted1;
        }
        private double Calculate_TY_to_TY(int year1, int year2, int category, int agency)
        {
            double weighted1 = InflationTables[agency].GetWeightedValue(category, year1);      
            double weighted2 = InflationTables[agency].GetWeightedValue(category, year2);      
            return weighted2 / weighted1;
        }
    }
}
