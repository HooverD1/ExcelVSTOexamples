using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace DNA_Test
{
    public static class ChartUtilities
    {

        public static SelectedPoint SelectDataPointNearToXY(double x, double y, Series series)
        {
            if (series.Points.Count() == 0)
                return null;

            var distances = (from DataPoint dp in series.Points select new Tuple<DataPoint, double>(dp, GetDistance(dp, x, y))).OrderBy(t => t.Item2);
            DataPoint closestDataPoint = distances.First().Item1;
            double nearestDistance = distances.First().Item2;
            double distanceRatio = nearestDistance / GetDistance(distances.First().Item1, distances.Last().Item1);
            if (distanceRatio <= 0.15)
                return new SelectedPoint(closestDataPoint, series);
            else
                return null;
        }


        private static double GetDistance(DataPoint dp, double x, double y)
        {
            double dp_x = dp.XValue;
            double dp_y = dp.YValues.First();
            
            double distance_x = x - dp_x;
            double distance_y = y - dp_y;

            return Math.Sqrt(distance_x * distance_x + distance_y * distance_y); //Pythagorean theorem!
        }

        private static double GetDistance(DataPoint dp1, DataPoint dp2)
        {
            double dp1x = dp1.XValue;
            double dp1y = dp1.YValues.First();
            double dp2x = dp2.XValue;
            double dp2y = dp2.YValues.First();
            double distance_x = dp2x - dp1x;
            double distance_y = dp2y - dp1y;
            return Math.Sqrt(distance_x * distance_x + distance_y * distance_y);
        }
    }
}
