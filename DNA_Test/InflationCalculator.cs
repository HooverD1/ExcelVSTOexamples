﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DNA_Test
{
    public enum InflationMode
    {
        BY_to_BY,
        BY_to_TY,
        TY_to_BY,
        TY_to_TY
    }

    public static class InflationCalculator
    {
        private static Dictionary<int, InflationTable> InflationTables { get; set; }    //<agencyNumber, agency's table>
        static InflationCalculator()
        {
            InflationTables = new Dictionary<int, InflationTable>();                    //initialize property
        }
        private static void GetTable(int agencyNumber, int thisCategory)
        {
            if (!InflationTables.ContainsKey(agencyNumber))                                          //if the table is not yet in the dictionary, retrieve it
            {
                InflationTable newTable = new InflationTable();
                InflationTables.Add(agencyNumber, newTable);
                newTable.SetAgency(agencyNumber);
                if(!File.Exists($@"C:\Users\grins\source\repos\HelloWorld\HelloWorld\Agency{agencyNumber}_xml.xml"))    //if xml file does not exist, use updateTable
                    InflationTables[agencyNumber].UpdateTable();
                else
                {
                    InflationTables[agencyNumber] = newTable.DeserializeTable(); //if xml file does exist, use it instead
                }
            }
            
        }
        public static void SerializeTables()
        {
            InflationTables[4].SerializeTable();
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
        private static double Calculate_BY_to_BY(int year1, int year2, int category, int agency)
        {
            double returnVal = InflationTables[agency].GetRawValue(category, year1);      //need to complete these calculations
            return returnVal;
        }
        private static double Calculate_BY_to_TY(int year1, int year2, int category, int agency)
        {
            double returnVal = InflationTables[agency].GetRawValue(category, year1);      //need to complete these calculations
            return returnVal;
        }
        private static double Calculate_TY_to_BY(int year1, int year2, int category, int agency)
        {
            double returnVal = InflationTables[agency].GetRawValue(category, year1);      //need to complete these calculations
            return returnVal;
        }
        private static double Calculate_TY_to_TY(int year1, int year2, int category, int agency)
        {
            double returnVal = InflationTables[agency].GetRawValue(category, year1);      //need to complete these calculations
            return returnVal;
        }
    }
}