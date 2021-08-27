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
        private double Mean { get; set; }

        public BoxPlot(double minimum, double lower_quartile, double median, double upper_quartile, double maximum, double mean)
        {
            this.Min = minimum;
            this.Q1 = lower_quartile;
            this.Q2 = median;
            this.Q3 = upper_quartile;
            this.Max = maximum;
            this.Mean = mean;
        }

        public BoxPlot(double[] datapoints)
        {
            Min = datapoints.Min();
            Q1 = Measures.LowerQuartile(datapoints);
            Q2 = Measures.Median(datapoints);
            Q3 = Measures.UpperQuartile(datapoints);
            Max = datapoints.Max();
            Mean = datapoints.Mean();
        }

        private double[] GetData()
        {
            return new double[] { Min, Max, Q1, Q3, Q2 };
        }

        public DataPoint GetBoxPlot(double xValue)
        {
            double[] data = this.GetData();
            DataPoint pt = new DataPoint(xValue, data);
            pt.ToolTip = $"Mean {this.Mean}\nMin {data[0]}\nMax {data[1]}\n25%tile {data[2]}\n50%tile {data[4]}\n75%tile {data[3]}";
            pt.Tag = "test3";
            return pt;
        }

        

    }
}
