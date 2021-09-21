using System;
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

        private const int MaxDays = 13;    //1 - 6 & 8 - 13
        private const int MaxWeeks = 52;
        private const int MaxMonths = 23;       //1 - 11 && 13 - 23
        private const int MaxYears = 20;

        private Interval? IntervalType { get; set; } = null;
        private double IntervalLength { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        private DateTime[] Midpoints { get; set; }
        private DateTime[] Endpoints { get; set; }

        private Scheduler() { }

        public Scheduler(double intervalLength, Interval intervalType, DateTime startDate, DateTime endDate) : this(intervalLength, intervalType, startDate.ToOADate(), endDate.ToOADate()) { }
        public Scheduler(double intervalLength, Interval intervalType, double startDate, double endDate)
        {
            //What if the user wants it to be midnight?
            StartDate = DateTime.FromOADate(startDate);
            //This should not form the end date..
            DateTime actualEndDate = StartDate;
            DateTime givenEndDate = DateTime.FromOADate(endDate);
            while (DateTime.Compare(actualEndDate, givenEndDate) <= 0)
            {
                switch (intervalType)
                {
                    case Scheduler.Interval.Daily:
                        actualEndDate = actualEndDate.AddDays(intervalLength);
                        break;
                    case Interval.Weekly:
                        actualEndDate = actualEndDate.AddDays(intervalLength * 7);
                        break;
                    case Interval.Monthly:
                        actualEndDate = actualEndDate.AddMonths(Convert.ToInt32(intervalLength));
                        break;
                    case Interval.Yearly:
                        actualEndDate = actualEndDate.AddYears(Convert.ToInt32(intervalLength));
                        break;
                    default:
                        throw new Exception("Unknown interval type");
                }
            }
            EndDate = actualEndDate;
            this.IntervalLength = intervalLength;
            this.IntervalType = intervalType;
            if(IntervalType != Interval.Daily && intervalLength % 1 != 0)
            {
                throw new Exception("Cannot use decimal interval lengths for non-daily interval types");
            }
            if (DateTime.Compare(StartDate, EndDate) > 0)
            {
                throw new Exception("Start date occurs on or after end date");
            }
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
            if (endpoints.Count() == 1)
            {
                midpoints = endpoints;
                return midpoints;
            }
            else
            {
                DateTime firstPoint = endpoints[0];
                DateTime secondPoint = endpoints[1];
                for (int i = 0; i < midpoints.Length; i++)
                {
                    midpoints[i] = DateTime.FromOADate((endpoints[i].ToOADate() + endpoints[i + 1].ToOADate()) / 2);
                }
                return midpoints;
            }
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

        public string ToIntervalString()
        {
            switch (this.IntervalType)
            {
                case Interval.Daily:
                    return $"{this.IntervalLength}-Day Interval: ";
                case Interval.Weekly:
                    return $"{this.IntervalLength}-Week Interval: ";
                case Interval.Monthly:
                    return $"{this.IntervalLength}-Month Interval: ";
                case Interval.Yearly:
                    return $"{this.IntervalLength}-Year Interval: ";
                default:
                    throw new Exception("Unexpected interval type");
            }
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
                    double week_periods = ((EndDate - StartDate).TotalDays + 1) / 7 / IntervalLength;
                    return Convert.ToInt32(Math.Ceiling(week_periods));
                case Interval.Monthly:
                    int years = EndDate.Year - StartDate.Year;
                    int months = EndDate.Month - StartDate.Month;
                    return (12 * years + months) / (int)IntervalLength + 1;
                case Interval.Yearly:
                    int years2 = EndDate.Year - StartDate.Year + 1;
                    return years2 / (int)IntervalLength;
                default:
                    throw new Exception("Unknown interval type");
            }
        }

        private int GetNumberOfMidpoints()
        {
            return GetEndpoints().Length - 1;
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

        public DateTime GetNextMidpoint()
        {
            DateTime endDate = this.GetEndpoints().Last();
            switch (IntervalType)
            {
                case Interval.Daily:
                    return DateTime.FromOADate((endDate.AddDays(IntervalLength).ToOADate() + endDate.ToOADate())/2);
                case Interval.Weekly:
                    return DateTime.FromOADate((endDate.AddDays(IntervalLength * 7).ToOADate() + endDate.ToOADate())/2);
                case Interval.Monthly:
                    return DateTime.FromOADate((endDate.AddMonths(Convert.ToInt32(IntervalLength)).ToOADate() + endDate.ToOADate())/2);
                case Interval.Yearly:
                    return DateTime.FromOADate((endDate.AddYears(Convert.ToInt32(IntervalLength)).ToOADate() + endDate.ToOADate())/2);
                default:
                    throw new Exception("Unknown enum (GetNextInterval())");
            }
        }

        public DateTime GetNextEndpoint()
        {
            DateTime endDate = this.GetEndpoints().Last();
            switch (IntervalType)
            {
                case Interval.Daily:
                    return endDate.AddDays(IntervalLength);
                case Interval.Weekly:
                    return endDate.AddDays(IntervalLength * 7);
                case Interval.Monthly:
                    return endDate.AddMonths(Convert.ToInt32(IntervalLength));
                case Interval.Yearly:
                    return endDate.AddYears(Convert.ToInt32(IntervalLength));
                default:
                    throw new Exception("Unknown enum (GetNextInterval())");
            }
        }

        public double GetIntervalLength()
        {
            return IntervalLength;
        }

        public Interval? GetIntervalType()
        {
            return IntervalType;
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

        public static int GetMaximumReasonablePeriods(Interval intType)
        {
            switch (intType)
            {
                case Interval.Daily:
                    return MaxDays;
                case Interval.Weekly:
                    return MaxWeeks;
                case Interval.Monthly:
                    return MaxMonths;
                case Interval.Yearly:
                    return MaxYears;
                default:
                    throw new Exception("Unknown interval type.");
            }
        }

        public bool IsIntervalEqual(Scheduler schedule)
        {
            if(this.IntervalLength == schedule.IntervalLength)
            {
                if(this.IntervalType == schedule.IntervalType)
                {
                    return true;
                }
            }
            return false;
        }

        public void ShiftDurationByIntervals(int intervalShift)
        {
            if(intervalShift > 0)
            {
                //Endpoints
                List<DateTime> newEndPoints = this.Endpoints.ToList();
                List<DateTime> newMidPoints = this.Midpoints.ToList();
                for(int i=0; i<intervalShift; i++)
                {
                    newEndPoints.Add(GetNextEndpoint());
                    this.Endpoints = newEndPoints.ToArray();
                }
                this.EndDate = this.Endpoints.Last();
            }
            else if(intervalShift < 0)
            {
                //Endpoints
                DateTime[] newEndPoints = new DateTime[Endpoints.Length + intervalShift];
                for(int i=0; i<Endpoints.Length+intervalShift; i++)
                {
                    newEndPoints[i] = Endpoints[i];
                }
                Endpoints = newEndPoints;
                this.EndDate = this.Endpoints.Last();
            }
            this.Midpoints = null;
            this.Midpoints = GetMidpoints();
        }
        
        public DateTime GetStartDate() { return StartDate; }
        public DateTime GetEndDate() { return EndDate; }
    }
}
