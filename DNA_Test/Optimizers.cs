using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Distributions;
using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics.Testing;

namespace DNA_Test
{
    public static class Optimizers
    {
        private static Random rando { get; set; } = new Random();
        public static double ScheduleOptimizer(DateTime[] midpoints, double[] bucketSums, Scheduler.Scheduler schedule)
        {
            //NEEDS IMPLEMENTED -- RETURNING RANDOM FOR THE SAKE OF TESTING
            if (bucketSums.Count() <= 1)
                return 0;
            Random rando = new Random();
            return rando.NextDouble();
        }
    }
}
