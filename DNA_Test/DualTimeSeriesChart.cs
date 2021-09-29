using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNA_Test
{
    public class DualTimeSeriesChart : TimeSeriesChart
    {
        public Dictionary<DateTime, double> DataPoints2 { get; set; }

        public DualTimeSeriesChart(Dictionary<DateTime, double> dataPoints1, Dictionary<DateTime, double> dataPoints2) : base(dataPoints1)
        {
            this.DataPoints2 = dataPoints2;
        }
    }
}
