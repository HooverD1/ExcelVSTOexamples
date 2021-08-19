using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace DNA_Test
{
    public class TimeSeriesChart : Chart
    {
        public static int default_chartHeight = 100;    //Overwritable defaults
        public static int default_chartWidth = 100;
        private ChartArea chart { get; set; }        //The x-Axis -- potentially overwritten for non-uniform
        private Series TimeSeries { get; set; } //The data (potentially bucketed)
        private Series FitSeries { get; set; }  //The regression line
        private Series ErrorSeries_Upper { get; set; }  //The regression line + error band
        private Series ErrorSeries_Lower { get; set; }  //The regression line - error band
        private PDF_Popup pdf_Popup { get; set; }

        public TimeSeriesChart(Dictionary<DateTime, double> timeSeriesDataPoints) : base()
        {
            /*  Set the default xAxis
             *  Load the Chart series'
             */
            this.Height = default_chartHeight;
            this.Width = default_chartWidth;
            chart = new ChartArea();
            this.ChartAreas.Add(chart);
            TimeSeries = GenerateTimeSeries(timeSeriesDataPoints);
            this.Series.Add(TimeSeries);
            FitSeries = GenerateFitSeries();
            this.Series.Add(FitSeries);
            ErrorSeries_Lower = GenerateErrorSeries_Lower();
            this.Series.Add(ErrorSeries_Lower);
            ErrorSeries_Upper = GenerateErrorSeries_Upper();
            this.Series.Add(ErrorSeries_Upper);
        }

        private Series GenerateTimeSeries(Dictionary<DateTime, double> timeSeriesDataPoints)
        {
            Series timeSeries = new Series();
            timeSeries.ChartType = SeriesChartType.Point;
            foreach(KeyValuePair<DateTime, double> datapoint in timeSeriesDataPoints)
            {
                timeSeries.Points.AddXY(datapoint.Key, datapoint.Value);
            }
            return timeSeries;
        }
        private Series GenerateFitSeries()
        {
            Series fitSeries = new Series();
            fitSeries.ChartType = SeriesChartType.Point;
            return fitSeries;
        }
        private Series GenerateErrorSeries_Lower()
        {
            Series errorSeries = new Series();
            errorSeries.ChartType = SeriesChartType.Point;
            return errorSeries;
        }
        private Series GenerateErrorSeries_Upper()
        {
            Series errorSeries = new Series();
            errorSeries.ChartType = SeriesChartType.Point;
            return errorSeries;
        }
    }
}
