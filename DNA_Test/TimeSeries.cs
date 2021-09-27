using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace DNA_Test
{
    public class TimeSeries
    {
        //Each instance is a bucketing of data into .DataPoints sum-buckets
        //Each instance therefore has one schedule (bucketing-scheme)
        //Each instance can be fit with multiple regressions

        //Bundle datapoints, schedule, and regression -- used as parameter for TimeSeriesChart
        public double[] DataPoints { get; set; }
        public Scheduler.Scheduler Schedule { get; set; }
        public IRegression[] Regressions { get; set; }      

        public TimeSeries()
        {

        }
    }
}
