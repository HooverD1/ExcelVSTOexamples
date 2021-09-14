using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;


namespace DNA_Test
{
    public class BoxPlotSeries
    {
        private TimeSeriesChart Parent { get; set; }
        public Series PrimarySeries { get; }
        public Series LabelSeries { get; }
        public Series MeanSeries { get; }

        //public BoxPlotSeries(Tuple<double, double[]>[] boxPlotPoints)
        //{   //To build from data
        //    PrimarySeries = new Series();
        //    PrimarySeries.ChartType = SeriesChartType.BoxPlot;
        //    foreach (Tuple<double, double[]> point in boxPlotPoints)
        //    {
        //        BoxPlot boxPlot = new BoxPlot(point.Item2[0], point.Item2[1], point.Item2[2], point.Item2[3], point.Item2[4], point.Item2[5]);
        //        PrimarySeries.Points.Add(boxPlot.GetBoxPlot(point.Item1));
        //    }
        //}
        public BoxPlotSeries(TimeSeriesChart parent)
        {
            this.Parent = parent;
            LabelSeries = new Series();
            LabelSeries.Name = "BoxPlotSeries_Labels";
            LabelSeries.ChartType = SeriesChartType.Point;
            LabelSeries.MarkerStyle = MarkerStyle.None;
            LabelSeries.MarkerColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            LabelSeries.SmartLabelStyle.Enabled = true;

            PrimarySeries = new Series();
            PrimarySeries.Name = "BoxPlotSeries_BoxPlots";
            PrimarySeries.ChartType = SeriesChartType.BoxPlot;
            PrimarySeries.Color = System.Drawing.Color.Orange;
            
            PrimarySeries.BorderColor = System.Drawing.Color.Black;
            PrimarySeries.BorderWidth = 2;

            MeanSeries = new Series();
            MeanSeries.Name = "BoxPlotSeries_Means";
            MeanSeries.ChartType = SeriesChartType.Point;
            MeanSeries.MarkerStyle = MarkerStyle.Square;
            MeanSeries.MarkerColor = System.Drawing.Color.Black;
            MeanSeries.MarkerSize = 10;
        }

        public void SetWidth(int points)
        {
            PrimarySeries["PixelPointWidth"] = "20";
        }

        public void Add(double x, BoxPlot boxPlot, double mean)
        {
            //Add boxPlot at x-value x
            DataPoint plotPoint = boxPlot.GetBoxPlot(x);
            PrimarySeries.Points.Add(plotPoint);
            MeanSeries.Points.AddXY(x, mean);
            string valueString;
            valueString = "Min: " + Math.Round(plotPoint.YValues[0]).ToString();
            DataPoint minimum = new DataPoint(TransformX(x, valueString.Length, true), TransformY(plotPoint.YValues[0]));
            minimum.Label = valueString;
            valueString = "Q1: " + Math.Round(plotPoint.YValues[2]).ToString();
            DataPoint q1 = new DataPoint(TransformX(x, valueString.Length, false), TransformY(plotPoint.YValues[2]));
            q1.Label = valueString;
            valueString = "Median: " + Math.Round(plotPoint.YValues[4]).ToString();
            DataPoint q2 = new DataPoint(TransformX(x, valueString.Length, true), TransformY(plotPoint.YValues[4]));
            q2.Label = valueString;
            valueString = "Q3: " + Math.Round(plotPoint.YValues[3]).ToString();
            DataPoint q3 = new DataPoint(TransformX(x, valueString.Length, false), TransformY(plotPoint.YValues[3]));
            q3.Label = valueString;
            valueString = "Max: " + Math.Round(plotPoint.YValues[1]).ToString();
            DataPoint maximum = new DataPoint(TransformX(x, valueString.Length, true), TransformY(plotPoint.YValues[1]));
            maximum.Label = valueString;
            LabelSeries.Points.Add(minimum);
            LabelSeries.Points.Add(q1);
            LabelSeries.Points.Add(q2);
            LabelSeries.Points.Add(q3);
            LabelSeries.Points.Add(maximum);


        }

        public void SetBoxPlotColor_OnFitSeries()
        {
            if (Parent.FitSeries != null)
            {
                int red = Parent.FitSeries.Color.R + (256 - Parent.FitSeries.Color.R) / 2;
                int green = Parent.FitSeries.Color.G + (256 - Parent.FitSeries.Color.G) / 2;
                int blue = Parent.FitSeries.Color.B + (256 - Parent.FitSeries.Color.B) / 2;
                PrimarySeries.Color = System.Drawing.Color.FromArgb(red, green, blue);
            }
        }

        private int GetLengthOfValue(double value)
        {
            return value.ToString().Length;
        }

        private double TransformX(double x, int length, bool isDisplayedOnLeft)
        {
            //Have to move from PixelPointWidth --> x-coords
            //How far in x-coords is one pixel?
            double xPerPixel = Parent.Get_X_Coords_Per_Pixel();
            if (!int.TryParse(PrimarySeries["PixelPointWidth"].ToString(), out int width))
                throw new Exception("PixelPointWidth cannot be converted to integer. Make sure to set width before adding points.");
            double offset_pix = width / 2;
            if (isDisplayedOnLeft)
                return x - (offset_pix * xPerPixel) - (4 * length * xPerPixel);     //Additional offset to move the label outside the boxplot
            else
                return x + (offset_pix * xPerPixel) + (4 * length * xPerPixel);
        }
        private double TransformY(double y)
        {
            double yPerPixel = Parent.Get_Y_Coords_Per_Pixel();
            const double offset_pix = 15;     //The amount of pixels you need to drop to align the labels vertically.
            return y - (offset_pix * yPerPixel);
        }

    }
}
