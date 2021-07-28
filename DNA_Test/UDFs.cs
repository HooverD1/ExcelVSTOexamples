using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelDna.Integration;

namespace Primer
{
    public static class UDFs
    {
        private static InflationTable inflTable { get; set; }
        private static InflationCalculator inflCalc { get; set; }
        //All public static functions will be registered as UDFs
        [ExcelFunction(Description = "Inflation calculation function", Category = "ROSE Primer")]
        public static int Testo()
        {
            return 1;
        }

        [ExcelFunction(Description = "Inflation calculation function", Category = "ROSE Primer")]
        public static dynamic INFL(int fromYear, int toYear, int mode, int category, int agency=4)
        {
            if(inflCalc == null)
            {
                inflCalc = new InflationCalculator();
            }
            if (inflTable == null)
            {
                inflTable = new InflationTable();
                inflTable.SetAgency(4);
                inflTable.UpdateTable();
            }
            try
            {
                return inflCalc.Calculate(fromYear, toYear, mode, category, agency);
            }
            catch
            {
                return ExcelError.ExcelErrorValue;
            }            
        }

        //[ExcelFunction(Name = "SCHEDULE", Description = "Produces dates for the chosen interval and time period. Use [Ctrl-F5] for menu.")]
        //public static dynamic Schedule([ExcelArgument(Name = "Interval Index", Description = "Index for the interval type. [Ctrl-F5] for a list")]int intervalType,
        //    [ExcelArgument(Name = "Start Date", Description = "First date in schedule (MM/DD/YYYY)")]dynamic startDate, 
        //    [ExcelArgument(Name = "End Date", Description = "Last date in schedule (MM/DD/YYYY)")]dynamic endDate)
        //{
        //    try
        //    {
        //        double sDate;
        //        double eDate;
        //        if (startDate is DateTime)
        //        {
        //            sDate = ((DateTime)startDate).ToOADate();
        //        }
        //        else if (DateTime.TryParse(startDate.ToString(), out DateTime datetime_start))
        //        {
        //            sDate = datetime_start.ToOADate();
        //        }
        //        else
        //        {
        //            if(!Double.TryParse(startDate.ToString(), out sDate))
        //                throw new FormatException("Start date is malformed");
        //        }
        //        if (endDate is DateTime)
        //        {
        //            eDate = ((DateTime)endDate).ToOADate();
        //        }
        //        else if (DateTime.TryParse(endDate.ToString(), out DateTime datetime_end))
        //        {
        //            eDate = datetime_end.ToOADate();
        //        }
        //        else
        //        {
        //            if(!Double.TryParse(endDate.ToString(), out eDate))
        //                throw new FormatException("End date is malformed");
        //        }

        //        switch (intervalType)
        //        {
        //            case 1:     //Daily
        //                return new DNA_Test.Scheduler.Scheduler_Daily(sDate, eDate).ToString();
        //            case 2:     
        //                return new DNA_Test.Scheduler.Scheduler_Day_x2(sDate, eDate).ToString();
        //            case 3:
        //                return new DNA_Test.Scheduler.Scheduler_Day_x3(sDate, eDate).ToString();
        //            case 4:
        //                return new DNA_Test.Scheduler.Scheduler_Day_x4(sDate, eDate).ToString();
        //            case 5:
        //                return new DNA_Test.Scheduler.Scheduler_Day_x5(sDate, eDate).ToString();
        //            case 6:
        //                return new DNA_Test.Scheduler.Scheduler_Day_x6(sDate, eDate).ToString();
        //            case 7:     //Weekly
        //                return new DNA_Test.Scheduler.Scheduler_Weekly(sDate, eDate).ToString();
        //            case 8:
        //                return new DNA_Test.Scheduler.Scheduler_Week_x2(sDate, eDate).ToString();
        //            case 9:
        //                return new DNA_Test.Scheduler.Scheduler_Week_x3(sDate, eDate).ToString();
        //            case 10:
        //                return new DNA_Test.Scheduler.Scheduler_Week_x4(sDate, eDate).ToString();
        //            case 11:
        //                return new DNA_Test.Scheduler.Scheduler_Week_x5(sDate, eDate).ToString();
        //            case 12:    //Monthly
        //                return new DNA_Test.Scheduler.Scheduler_Monthly(sDate, eDate).ToString();
        //            case 13:
        //                return new DNA_Test.Scheduler.Scheduler_Month_x2(sDate, eDate).ToString();
        //            case 14:
        //                return new DNA_Test.Scheduler.Scheduler_Month_x3(sDate, eDate).ToString();
        //            case 15:
        //                return new DNA_Test.Scheduler.Scheduler_Month_x4(sDate, eDate).ToString();
        //            case 16:
        //                return new DNA_Test.Scheduler.Scheduler_Month_x5(sDate, eDate).ToString();
        //            case 17:
        //                return new DNA_Test.Scheduler.Scheduler_Month_x6(sDate, eDate).ToString();
        //            case 18:    //Yearly
        //                return new DNA_Test.Scheduler.Scheduler_Yearly(sDate, eDate).ToString();
        //            case 19:
        //                return new DNA_Test.Scheduler.Scheduler_Year_x2(sDate, eDate).ToString();
        //            case 20:
        //                return new DNA_Test.Scheduler.Scheduler_Year_x3(sDate, eDate).ToString();
        //            case 21:
        //                return new DNA_Test.Scheduler.Scheduler_Year_x4(sDate, eDate).ToString();
        //            case 22:
        //                return new DNA_Test.Scheduler.Scheduler_Year_x5(sDate, eDate).ToString();
        //            case 23:
        //                return new DNA_Test.Scheduler.Scheduler_Year_x10(sDate, eDate).ToString();
        //            default:
        //                return ExcelError.ExcelErrorValue;
        //        }
        //    }
        //    catch(Exception e)   //If any exception occurs, return an Excel error.
        //    {
        //        switch (e.Message)
        //        {
        //            case "Start date occurs on or after end date":
        //                return ExcelError.ExcelErrorNum;
        //            case "Start date is malformed":
        //                return ExcelError.ExcelErrorNum;
        //            case "End date is malformed":
        //                return ExcelError.ExcelErrorNum;
        //            default:
        //                return ExcelError.ExcelErrorValue;
        //        }
        //    }
        //}

    }
}