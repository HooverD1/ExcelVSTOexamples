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
        public delegate Dictionary<string, dynamic> OptimizerFunction(DateTime[] dt, double[] db, Scheduler.Scheduler schedule);

        private static Random rando { get; set; } = new Random();
        public static Dictionary<string, dynamic> ScheduleOptimizer(DateTime[] midpoints, double[] bucketSums, Scheduler.Scheduler schedule)
        {
            var result = new Dictionary<string, dynamic>();
            //NEEDS IMPLEMENTED -- RETURNING RANDOM FOR THE SAKE OF TESTING
            if (bucketSums.Count() <= 1)
            {
                result.Add("Score", 0);
            }
            Random rando = new Random();
            result.Add("Score", rando.NextDouble());
            return result;
        }
    }
}
