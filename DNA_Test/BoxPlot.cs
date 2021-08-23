using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics;
using System.Windows.Forms.DataVisualization.Charting;

namespace DNA_Test
{
    public class BoxPlot
    {
        private double Min { get; set; }
        private double Q1 { get; set; }
        private double Q2 { get; set; }
        private double Q3 { get; set; }
        private double Max { get; set; }

        public BoxPlot(double minimum, double lower_quartile, double median, double upper_quartile, double maximum)
        {
            this.Min = minimum;
            this.Q1 = lower_quartile;
            this.Q2 = median;
            this.Q3 = upper_quartile;
            this.Max = maximum;
        }

        public BoxPlot(double[] datapoints)
        {
            Min = datapoints.Min();
            Q1 = Measures.LowerQuartile(datapoints);
            Q2 = Measures.Median(datapoints);
            Q3 = Measures.UpperQuartile(datapoints);
            Max = datapoints.Max();
        }

        private double[] GetQuartiles()
        {
            return new double[] { Min, Max, Q1, Q3, Q2 };
        }

        public DataPoint GetBoxPlot(double xValue)
        {
            return new DataPoint(xValue, this.GetQuartiles());
        }
    }
}
