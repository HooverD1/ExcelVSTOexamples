﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNA_Test.Scheduler
{
    public class Scheduler
    {
        public enum Interval
        {
            Daily,
            Weekly,
            Monthly,
            Yearly
        }
        private Interval? IntervalType { get; set; } = null;
        private double IntervalLength { get; set; }
        protected DateTime StartDate { get; set; }
        protected DateTime EndDate { get; set; }
        private DateTime[] Midpoints { get; set; }
        private DateTime[] Endpoints { get; set; }

        private Scheduler() { }

        public Scheduler(double intervalLength, Interval intervalType, DateTime startDate, DateTime endDate) : this(intervalLength, intervalType, startDate.ToOADate(), endDate.ToOADate()) { }
        public Scheduler(double intervalLength, Interval intervalType, double startDate, double endDate)
        {
            //What if the user wants it to be midnight?
            StartDate = DateTime.FromOADate(startDate);
            EndDate = DateTime.FromOADate(endDate);
            this.IntervalLength = intervalLength;
            this.IntervalType = intervalType;
            if (DateTime.Compare(StartDate, EndDate) > 0)
            {
                throw new Exception("Start date occurs on or after end date");
            }
            if (!(startDate - Math.Floor(startDate) > 0 || endDate - Math.Floor(endDate) > 0))
            {   //If neither specify a time, set the start to midnight and the stop to one tick before midnight.
                EndDate = EndDate.AddHours(11);
                EndDate = EndDate.AddMinutes(59);
                EndDate = EndDate.AddSeconds(59);
            }
        }

        public static Scheduler ConstructFromMidpoints(DateTime[] midpoints)
        {
            Scheduler newScheduler = new Scheduler();
            newScheduler.Midpoints = midpoints;
            var intervalData = newScheduler.MatchPoints(midpoints);
            newScheduler.IntervalLength = intervalData.Item1;
            newScheduler.IntervalType = intervalData.Item2;
            double difference = (midpoints.Last().ToOADate() - midpoints.First().ToOADate()) / midpoints.Count();
            newScheduler.StartDate = DateTime.FromOADate(midpoints.First().ToOADate() - (difference / 2));
            newScheduler.EndDate = DateTime.FromOADate(midpoints.Last().ToOADate() + (difference / 2));
            newScheduler.GetEndpoints();
            return newScheduler;
        }


        public static Scheduler ConstructFromEndpoints(DateTime[] endpoints)
        {
            Scheduler newScheduler = new Scheduler();
            newScheduler.Endpoints = endpoints;
            newScheduler.StartDate = endpoints.First();
            newScheduler.EndDate = endpoints.Last();

            newScheduler.GetMidpoints();

            ////MIDPOINTS TO ENDPOINTS
            //DateTime[] midpoints = new DateTime[endpoints.Count() + 1];
            ////Construct to aid in autopopulating the form
            //if (endpoints.Count() < 2)
            //    return null;
            //double distance = (midpoints[1].ToOADate() - midpoints[0].ToOADate());
            //for (int i = 0; i < endpoints.Count(); i++)
            //{
            //    endpoints[i] = DateTime.FromOADate((midpoints[0].ToOADate() - (distance / 2)) + (distance * i));
            //}
            //newScheduler.Endpoints = endpoints;

            var intervalData = newScheduler.MatchPoints(newScheduler.Midpoints);
            newScheduler.IntervalLength = intervalData.Item1;
            newScheduler.IntervalType = intervalData.Item2;
            return newScheduler;
        }

        public DateTime[] GetMidpoints()
        {
            if(Midpoints == null || Midpoints.Count() == 0)
            {
                Midpoints = BuildMidpoints();
            }
            return Midpoints;
        }

        protected virtual DateTime[] BuildMidpoints()
        {
            DateTime[] endpoints = GetEndpoints();
            DateTime[] midpoints = new DateTime[GetNumberOfMidpoints()];
            DateTime firstPoint = endpoints[0];
            DateTime secondPoint = endpoints[1];
            double days = (secondPoint - firstPoint).TotalDays;
            //How far apart are the first two points?
            if (this.IntervalType == null)
            {
                //Estimate the intervaltype if need be
                
                if (days % 365 <= 1)
                    this.IntervalType = Interval.Yearly;
                else if (days % 30 <= 1 || days % 28 <= 1)
                    this.IntervalType = Interval.Monthly;
                else if (days % 7 == 0)
                    this.IntervalType = Interval.Weekly;
                else
                    this.IntervalType = Interval.Daily;
            }

            if (this.IntervalType == Interval.Daily || this.IntervalType == Interval.Weekly)
            {
                //Days or Weeks
                midpoints[0] = endpoints[0].AddDays(days / 2);
                for (int ept = 1; ept < endpoints.Length; ept++)
                {
                    midpoints[ept] = midpoints[ept - 1].AddDays(1);
                }
            }
            else if (this.IntervalType == Interval.Monthly)
            {
                //Months
                midpoints[0] = endpoints[0].AddDays(days / 2);
                for (int ept = 1; ept < endpoints.Length; ept++)
                {
                    midpoints[ept] = midpoints[ept - 1].AddDays(1);
                }
            }
            else if (this.IntervalType == Interval.Yearly)
            {
                //Years
                midpoints[0] = endpoints[0].AddDays(days / 2);
                for (int ept = 1; ept < endpoints.Length; ept++)
                {
                    midpoints[ept] = midpoints[ept - 1].AddDays(1);
                }
            }

            //Take the end points and divide them by 2



            return midpoints;
        }

        public DateTime[] GetEndpoints()
        {
            if (Endpoints == null)
            {
                Endpoints = BuildEndpoints();
            }
            return Endpoints;
        }

        private DateTime[] BuildEndpoints()
        {
            DateTime[] endpoints = new DateTime[GetNumberOfEndpoints()];
            for (int pt = 0; pt < endpoints.Length; pt++)
            {
                endpoints[pt] = StartDate.AddDays(pt);
                switch (IntervalType)
                {
                    case Interval.Daily:
                        endpoints[pt] = StartDate.AddDays(pt * IntervalLength);
                        break;
                    case Interval.Weekly:
                        endpoints[pt] = StartDate.AddDays(pt * 7 * IntervalLength);
                        break;
                    case Interval.Monthly:
                        endpoints[pt] = StartDate.AddMonths(Convert.ToInt32(pt * IntervalLength));
                        break;
                    case Interval.Yearly:
                        endpoints[pt] = StartDate.AddYears(Convert.ToInt32(pt * IntervalLength));
                        break;
                    default:
                        throw new Exception("Unknown interval type");
                }
            }
            return endpoints;
        }

        public override string ToString()
        {
            //Send the midpoints to a string
            StringBuilder sb = new StringBuilder();
            DateTime[] midpoints = GetMidpoints();
            foreach(DateTime dt in midpoints)
            {
                if (dt.Hour == 12 && dt.Minute == 00)
                    sb.Append(dt.ToString("MM/dd/yyyy"));
                else
                    sb.Append(dt.ToString("MM/dd/yyyy@HH:mm"));
                sb.Append(","); //Comma delimit
            }
            sb.Length -= 1;     //Remove the final comma
            return sb.ToString();
        }

        private int GetNumberOfEndpoints()      //Daily
        {
            if (DateTime.Compare(StartDate, EndDate) > 0)
            {
                throw new Exception("Start date occurs on or after end date");
            }
            //Use the start and end dates to come up with the number of midpoints
            switch (IntervalType)
            {
                case Interval.Daily:
                    double periods = ((EndDate - StartDate).TotalDays + 1) / IntervalLength;
                    return Convert.ToInt32(Math.Ceiling(periods));
                case Interval.Weekly:
                    double week_periods = ((EndDate - StartDate).TotalDays + 1) / 7;
                    return Convert.ToInt32(Math.Ceiling(week_periods));
                case Interval.Monthly:
                    int years = EndDate.Year - StartDate.Year;
                    int months = EndDate.Month - StartDate.Month;
                    return 12 * years + months + 1;
                case Interval.Yearly:
                    int years2 = EndDate.Year - StartDate.Year + 1;
                    return years2;
                default:
                    throw new Exception("Unknown interval type");
            }
        }

        private int GetNumberOfMidpoints()
        {
            return GetNumberOfEndpoints() - 1;
        }

        private Tuple<double, Interval> MatchPoints(DateTime[] midpoints)
        {
            double intervalLength = 0;
            Scheduler.Interval intervalType;
            //Autopop the fields if you can
            //Attempt to autopop the intervalLength and intervalType
            int points = midpoints.Count();
            DateTime lastPoint = midpoints.Last();
            DateTime firstPoint = midpoints.First();
            //This is not giving the correct number for years.. think 12/31/00 to 01/01/01.
            int days = Convert.ToInt32(Math.Floor((lastPoint - firstPoint).TotalDays));
            int years = 0;
            DateTime checkYears;
            for (int y = 0; y < 1000; y++)
            {
                checkYears = firstPoint.AddYears(y);
                if (checkYears.ToOADate() < lastPoint.ToOADate())
                    continue;
                else if (checkYears.ToOADate() > lastPoint.ToOADate())
                {
                    years = y - 1;
                    break;
                }
                else if (checkYears.ToOADate() == lastPoint.ToOADate())
                {
                    years = y;
                    break;
                }
            }
            int months = 0;
            DateTime checkMonths;
            for (int m = 0; m < 12000; m++)
            {
                checkMonths = firstPoint.AddMonths(m);
                if (checkMonths.ToOADate() < lastPoint.ToOADate())
                    continue;
                if (checkMonths.ToOADate() > lastPoint.ToOADate())
                {
                    months = m - 1;
                    break;
                }
                else if (checkMonths.ToOADate() == lastPoint.ToOADate())
                {
                    months = m;
                    break;
                }
            }
            int weeks = days / 7;
            //Add years to the start date and see if you get the end date
            //THESE ARE COMPARING END DATE TO THE LAST MIDPOINT...
            if (firstPoint.AddYears(years).ToOADate() == lastPoint.ToOADate() && years % points == 0)
            {
                //Timespan is divisible by full years and divisible by the number of periods
                intervalLength = (double)years / points;
                intervalType = Interval.Yearly;
            }
            else if (firstPoint.AddMonths(months).ToOADate() == lastPoint.ToOADate() && months % points == 0)
            {
                //Timespan is divisible by full years and divisible by the number of periods
                intervalLength = (double)months / points;
                intervalType = Interval.Monthly;
            }
            else if (firstPoint.AddDays(weeks * 7).ToOADate() == lastPoint.ToOADate() && weeks % points == 0)
            {
                //Timespan is divisible by full years and divisible by the number of periods
                intervalLength = (double)weeks / points;
                intervalType = Interval.Weekly;
            }
            else if (firstPoint.AddDays(days).ToOADate() == lastPoint.ToOADate() && days % points == 0)
            {
                //Timespan is divisible by full years and divisible by the number of periods
                intervalLength = (double)days / points;
                intervalType = Interval.Daily;
            }
            else
            {
                //Can't resolve -- just load defaults
                intervalLength = 1;
                intervalType = Interval.Daily;
            }
            return new Tuple<double, Interval>(intervalLength, intervalType);
        }

        public double GetIntervalLength()
        {
            return IntervalLength;
        }

        public string GetIntervalTypeString()
        {
            switch (IntervalType)
            {
                case Interval.Daily:
                    return "Days";
                case Interval.Weekly:
                    return "Weeks";
                case Interval.Monthly:
                    return "Months";
                case Interval.Yearly:
                    return "Years";
                default:
                    throw new Exception("Unknown interval type");
            }
        }
    }
}
