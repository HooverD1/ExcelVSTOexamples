using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNA_Test
{
    public class OptimizationResult
    {
        //Structure to hold the results of running optimizers on a bucketing scheme
        //Work with different types of bucketing? (Periods, ...?)

        public double Score { get; set; }
        public Dictionary<DateTime, double> BucketedSums { get; set; }
        public Scheduler.Scheduler Schedule { get; set; }
        public Optimizers.OptimizerFunction OptimizerUsed { get; set; }
        public string DistributionUnderTest { get; set; }
        public IRegression RegressionUnderTest { get; set; }
        
        public OptimizationResult(double score, double[] bucketedSums, Scheduler.Scheduler schedule, Optimizers.OptimizerFunction optimizer, IRegression regression)
        {
            this.Score = score;
            this.Schedule = schedule;
            this.BucketedSums = new Dictionary<DateTime, double>();
            for(int i=0;i<schedule.GetMidpoints().Length; i++)
            {
                BucketedSums.Add(schedule.GetMidpoints()[i], bucketedSums[i]);
            }

            /*  The regression type should be a parameter. OptimizationFunction(Data, Regression) = OptimizationResult
             */
            this.RegressionUnderTest = regression;
        }

        public override string ToString()
        {
            return $"Schedule: {this.Schedule.GetIntervalLength()} {this.Schedule.GetIntervalTypeString()}, Fit: {Math.Round(Score, 2)}; {DistributionUnderTest}";
        }
    }
}
