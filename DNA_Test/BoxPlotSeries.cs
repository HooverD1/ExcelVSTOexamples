using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace DNA_Test
{
    public class BoxPlotSeries : Series
    {
        public BoxPlotSeries(Tuple<double, double[]>[] boxPlotPoints)
        {
            this.ChartType = SeriesChartType.BoxPlot;
            foreach (Tuple<double, double[]> point in boxPlotPoints)
            {
                BoxPlot boxPlot = new BoxPlot(point.Item2[0], point.Item2[1], point.Item2[2], point.Item2[3], point.Item2[4]);
                this.Points.Add(boxPlot.GetBoxPlot(point.Item1));
            }
        }
        public BoxPlotSeries() { }

        public void Add(double x, BoxPlot boxPlot)
        {
            this.Points.Add(boxPlot.GetBoxPlot(x));
        }
    }
}
