using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNA_Test
{
    public class SingleTimeSeriesChart : TimeSeriesChart
    {
        public SingleTimeSeriesChart(Dictionary<DateTime, double> dataPoints) : base(dataPoints)
        {

        }
    }
}
