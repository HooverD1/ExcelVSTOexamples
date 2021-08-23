using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DNA_Test
{
    public class Bucketer
    {
        public Scheduler.Scheduler OptimalSchedule { get; set; } = null;
        //public double OptimalScore { get; set; }        // Ranges 0.0 - 1.0 where 1.0 is perfect.
        //public double[] OptimalBuckets { get; set; }
        //public Dictionary<string, dynamic> OptimalResult {get;set;}
        
        //Returns value from 0 - 1 to denote how optimal the schedule-defined buckets are in matching the data
        
        public List<OptimizationResult> AutoBucket(DateTime[] dates, double[] values, List<Optimizers.OptimizerFunction> optimizers, bool sort = false)
        {
            //BUCKET THE DATA INTO ALL PERTINENT COMBINATIONS
            //Bucketing time: ~14ms per BucketToSchedule -- ~7,000ms at 100,000 datapoints and 10 year duration
            //Schedule time: ~3 - 5ms @ 10 years -- This varies by the duration of the period (calculating mid/endpoints)
            //Sort time: 10 - 15ms @ 100,000 -- Will vary by the amount of data
            System.Diagnostics.Stopwatch bucketWatch = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch scheduleWatch = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch sortWatch = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch optimizeWatch = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch totalWatch = new System.Diagnostics.Stopwatch();
            totalWatch.Start();
            sortWatch.Start();
            var Results = new List<OptimizationResult>();
            if (sort)
            {
                Array.Sort(dates, values);      //(Keys, Values)
            }
            sortWatch.Stop();
            DateTime startDate = dates.First();
            DateTime endDate;
            //Take dates and paired values, determine the best fit with the input fitting function??, return a tuple of bucket midpoints and paired bucket sums
            int iterationCount = 0;
            foreach (Scheduler.Scheduler.Interval iType in (DNA_Test.Scheduler.Scheduler.Interval[])Enum.GetValues(typeof(Scheduler.Scheduler.Interval)))
            {
                for (int iLen = 1; iLen <= Scheduler.Scheduler.GetMaximumReasonablePeriods(iType); iLen++)
                {
                    switch (iType)
                    {
                        case Scheduler.Scheduler.Interval.Daily:
                            if (iLen == 7)
                                continue;
                            endDate = dates.Last().AddDays(1);
                            break;
                        case Scheduler.Scheduler.Interval.Weekly:
                            endDate = dates.Last().AddDays(1);
                            break;
                        case Scheduler.Scheduler.Interval.Monthly:
                            if (iLen == 12)
                                continue;
                            endDate = dates.Last().AddDays(1);
                            break;
                        case Scheduler.Scheduler.Interval.Yearly:
                            endDate = dates.Last().AddDays(1);
                            break;
                        default:
                            throw new Exception("Unknown interval type");
                    }
                    
                    scheduleWatch.Start();
                    if(iLen == 40 && iType == Scheduler.Scheduler.Interval.Weekly) { }
                    Scheduler.Scheduler schedule = new Scheduler.Scheduler(iLen, iType, startDate, endDate);
                    scheduleWatch.Stop();
                    if (schedule.GetMidpoints().Length < 3)
                    {
                        break;
                        //Once the interval is so long that it puts everything in one bucket, checking longer lengths is not productive...
                        //...Break the inner loop and move on to the next iType
                    }
                    
                    bucketWatch.Start();
                    double[] bucketSums = BucketToSchedule(dates, values, schedule);
                    bucketWatch.Stop();
                    iterationCount++;
                    optimizeWatch.Start();
                    foreach(Optimizers.OptimizerFunction optimizer in optimizers)
                    {
                        //Run all the passed in optimizing functions here
                        //Each optimizer tests a regression type against the schedule
                        Results.Add(optimizer(bucketSums, schedule));
                    }
                    optimizeWatch.Stop();
                    if (!Results.Any())
                        throw new Exception("No optimizers loaded");
                }
                if (!Results.Any())
                    throw new Exception("Data over too short a time period");
            }
            totalWatch.Stop();
            /*MessageBox.Show($"Iterations: {iterationCount}\n" +
                $"Sort time: {sortWatch.ElapsedMilliseconds} ms \n" +
                $"Scheduler time: {scheduleWatch.ElapsedMilliseconds} ms\n" +
                $"Bucketing time: {bucketWatch.ElapsedMilliseconds} ms\n" +
                $"Optimizer time: {optimizeWatch.ElapsedMilliseconds} ms\n" +
                $"Total time: {totalWatch.ElapsedMilliseconds} ms");
            */
            return Results;
        }

        public static double[] BucketToSchedule(DateTime[] dates, double[] values, Scheduler.Scheduler schedule, bool sort=false)
        {
            //System.Diagnostics.Stopwatch whileWatch = new System.Diagnostics.Stopwatch();
            //System.Diagnostics.Stopwatch bucketWatch = new System.Diagnostics.Stopwatch();
            //bucketWatch.Start();
            //This function runs in roughly 20 - 40ms
            //Take dates and values and bucket them according to a schedule
            int midpoints = schedule.GetMidpoints().Count();
            double[] bucketSum = new double[midpoints];
            if(sort)
            {
                Array.Sort(dates, values);      //(Keys, Values)
                // Do not sort at this level if running AutoBucket -- set the sort flag when calling AutoBucket instead if necessary
            }
            int bucketIndex = 0;
            for(int v = 0; v < values.Length; v++)
            {
                //Assumes the data is ordered
                //whileWatch.Start();
                if (bucketIndex < midpoints)      //This puts the last point, which is always the end date, into the last bucket
                {
                    DateTime dataDate = dates[v];
                    DateTime endDate = schedule.GetEndpoints()[bucketIndex + 1];
                    while (DateTime.Compare(dataDate, endDate) >= 0 && bucketIndex < midpoints - 1) //Second condition leaves the end date in the last bucket
                    {
                        //var test = DateTime.Compare(dataDate, endDate);
                        endDate = schedule.GetEndpoints()[++bucketIndex + 1];
                        //if (bucketIndex == bucketSum.Length - 1) { }
                    }
                }
                //If daily: This is loading the last day and the day before into the same bucket.
                //If the chosen dates are at midnight.. 
                //  |  m1  |  m2  |  m3  | lines at midnight; m = midpoint
                //  s                    e s = start, e = end
                //  d1     d2     d3     d4
                // d1 --> m1, d2 --> m2, d3 --> m3, d4 --> m3
                // Should the end date be pushed 1 more intervalLength out?
                // I tried this, and now the last point is sometimes very low.
                // When it's days and there's no real width to the bucket, it works to push out.
                // When it's months and it's only the last day that could be extra in the last bucket, pushing out isolates that day in its own bucket
                // What to do here?
                // Always push 1 day?
                
                bucketSum[bucketIndex] += values[v];
            }
            //bucketWatch.Stop();
            //MessageBox.Show($"While Watch: {whileWatch.ElapsedMilliseconds} ms");
            //MessageBox.Show($"Bucket Watch: {bucketWatch.ElapsedMilliseconds} ms");
            return bucketSum;
        }

        
    }
}
