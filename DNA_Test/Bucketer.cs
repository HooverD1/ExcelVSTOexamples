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
        public double OptimalScore { get; set; }        // Ranges 0.0 - 1.0 where 1.0 is perfect.
        public double[] OptimalBuckets { get; set; }
        public Dictionary<string, dynamic> OptimalResult {get;set;}
        //Returns value from 0 - 1 to denote how optimal the schedule-defined buckets are in matching the data
        
        public Tuple<DateTime[], double[]> AutoBucket(DateTime[] dates, double[] values, List<Optimizers.OptimizerFunction> optimizers, bool sort = false)
        {
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
            if (sort)
            {
                Array.Sort(dates, values);      //(Keys, Values)
            }
            sortWatch.Stop();
            DateTime startDate = dates.First();
            DateTime endDate = dates.Last();
            //Take dates and paired values, determine the best fit with the input fitting function??, return a tuple of bucket midpoints and paired bucket sums
            OptimalScore = 0;
            int iterationCount = 0;
            foreach (Scheduler.Scheduler.Interval iType in (DNA_Test.Scheduler.Scheduler.Interval[])Enum.GetValues(typeof(Scheduler.Scheduler.Interval)))
            {
                for (int iLen = 1; iLen <= Scheduler.Scheduler.GetMaximumReasonablePeriods(iType); iLen++)
                {
                    scheduleWatch.Start();
                    Scheduler.Scheduler schedule = new Scheduler.Scheduler(iLen, iType, startDate, endDate);
                    DateTime[] midpoints = schedule.GetMidpoints();
                    scheduleWatch.Stop();
                    if (midpoints.Length < 3)
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
                    List<Dictionary<string, dynamic>> results = new List<Dictionary<string, dynamic>>();
                    foreach(Optimizers.OptimizerFunction optimizer in optimizers)
                    {
                        //Run all the passed in optimizing functions here
                        //Each optimizer tests a regression type against the schedule
                        results.Add(optimizer(midpoints, bucketSums, schedule));
                    }
                    optimizeWatch.Stop();
                    if (!results.Any())
                        throw new Exception("No optimizers loaded");
                    foreach(Dictionary<string, dynamic> result in results)
                    {
                        if (result["Score"] > OptimalScore)
                        {
                            //Save the best result found
                            OptimalResult = result;
                        }
                    }
                }
            }
            
            if (OptimalSchedule == null)
                throw new Exception("No optimal schedule was saved.");
            DateTime[] midpoints_Saved = OptimalSchedule.GetMidpoints();
            double[] bucketSums_Saved = OptimalBuckets;

            totalWatch.Stop();
            MessageBox.Show($"Iterations: {iterationCount}\n" +
                $"Sort time: {sortWatch.ElapsedMilliseconds} ms \n" +
                $"Scheduler time: {scheduleWatch.ElapsedMilliseconds} ms\n" +
                $"Bucketing time: {bucketWatch.ElapsedMilliseconds} ms\n" +
                $"Optimizer time: {optimizeWatch.ElapsedMilliseconds} ms\n" +
                $"Total time: {totalWatch.ElapsedMilliseconds} ms");
            return new Tuple<DateTime[], double[]>(midpoints_Saved, bucketSums_Saved);
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
                if (bucketIndex < midpoints - 1)      //This puts the last point, which is always the end date, into the last bucket
                {
                    
                    while (DateTime.Compare(dates[v], schedule.GetEndpoints()[bucketIndex]) > 0 && bucketIndex < midpoints - 1)
                        bucketIndex++;
                    
                }
                //whileWatch.Stop();
                bucketSum[bucketIndex] += values[v];
            }
            //bucketWatch.Stop();
            //MessageBox.Show($"While Watch: {whileWatch.ElapsedMilliseconds} ms");
            //MessageBox.Show($"Bucket Watch: {bucketWatch.ElapsedMilliseconds} ms");
            return bucketSum;
        }


    }
}
