using System;
using System.Linq;

namespace DNA_Test
{
    public static class Optimizers
    {
        public delegate OptimizationResult OptimizerFunction(double[] bucketedSums, Scheduler.Scheduler schedule);

        private static Random rando { get; set; } = new Random();
        public static OptimizationResult SimpleLinear_ScheduleOptimizer(double[] bucketSums, Scheduler.Scheduler schedule)
        {
            IRegression regression = new Regression_SimpleLinear(schedule.GetMidpoints(), bucketSums);
            double score;
            //NEEDS IMPLEMENTED -- RETURNING RANDOM FOR THE SAKE OF TESTING
            if (bucketSums.Count() <= 1)
                score = 0;
            else
                score = regression.Score();
            return new OptimizationResult(score, bucketSums, schedule, SimpleLinear_ScheduleOptimizer, regression);
        }
    }
}
