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
        private const double alpha = 0.95;
        public static int default_chartHeight = 100;    //Overwritable defaults
        public static int default_chartWidth = 100;
        public ChartArea chartArea { get; set; }        //The x-Axis -- potentially overwritten for non-uniform
        public Series TimeSeries { get; set; } //The data (potentially bucketed)
        public Series FitSeries { get; set; }  //The regression line
        public Series ErrorSeries_CI_Upper { get; set; }  //The regression line + error band
        public Series ErrorSeries_CI_Lower { get; set; }  //The regression line - error band
        public BoxPlotSeries BoxPlot_Series { get; set; }
        public Series MeanSeries { get; set; }
        private PDF_Popup pdf_Popup { get; set; }
        private double predictAt { get; set; }

        public TimeSeriesChart(Dictionary<DateTime, double> timeSeriesDataPoints, IRegression fitRegression) : base()
        {
            /*  Set the default xAxis
             *  Load the Chart series'
             */
            
            this.Click += OnChartClick;
            this.Height = default_chartHeight;
            this.Width = default_chartWidth;
            chartArea = new ChartArea();
            this.ChartAreas.Add(chartArea);
            this.EnableUserSelection();     //Has to come after chartArea

            TimeSeries = GenerateTimeSeries(timeSeriesDataPoints);
            this.predictAt = (from DataPoint dp in TimeSeries.Points select dp.XValue).Average();
            BoxPlot_Series = GenerateBoxPlotSeries(fitRegression, predictAt);
            FitSeries = GenerateFitSeries(fitRegression);
            ErrorSeries_CI_Lower = GenerateErrorSeries_Lower(fitRegression);
            ErrorSeries_CI_Upper = GenerateErrorSeries_Upper(fitRegression);

            this.Series.Add(ErrorSeries_CI_Lower);
            this.Series.Add(ErrorSeries_CI_Upper);
            this.Series.Add(BoxPlot_Series);
            this.Series.Add(TimeSeries);
            this.Series.Add(FitSeries);
            

            this.BorderlineDashStyle = ChartDashStyle.Solid;
            this.BorderlineColor = System.Drawing.Color.Black;
            this.BorderlineWidth = 2;
            this.chartArea.AxisX.MajorTickMark.Enabled = false;
            this.chartArea.AxisY.MajorTickMark.Enabled = false;
            this.chartArea.BorderDashStyle = ChartDashStyle.Solid;
            this.chartArea.BorderColor = System.Drawing.Color.Black;
            this.chartArea.BorderWidth = 2;
        }

        private void EnableUserSelection()
        {
            if (this.chartArea == null)
                throw new Exception("Define chartArea first.");
            this.chartArea.CursorX.IsUserEnabled = true;
            this.chartArea.CursorY.IsUserEnabled = true;
            this.chartArea.CursorX.IsUserSelectionEnabled = true;
            this.chartArea.CursorY.IsUserSelectionEnabled = true;
            this.chartArea.CursorX.Interval = 0.01;
            this.chartArea.CursorY.Interval = 0.01;
        }

        private Series GenerateTimeSeries(Dictionary<DateTime, double> timeSeriesDataPoints)
        {
            Series timeSeries = new Series();
            timeSeries.ChartType = SeriesChartType.Line;
            timeSeries.MarkerStyle = MarkerStyle.Circle;
            foreach(KeyValuePair<DateTime, double> datapoint in timeSeriesDataPoints)
            {
                timeSeries.Points.AddXY(datapoint.Key, datapoint.Value);
            }
            timeSeries.MarkerBorderColor = System.Drawing.Color.Black;
            timeSeries.MarkerSize = 12;     
            timeSeries.MarkerColor = System.Drawing.Color.Blue;
            timeSeries.BorderWidth = 2;     //Line thickness
            timeSeries.Color = System.Drawing.Color.Black;
            return timeSeries;
        }
        private Series GenerateFitSeries(IRegression fitRegression)     //Feed this the regression used to fit the time series
        {
            Series fitSeries = new Series();
            fitSeries.ChartType = SeriesChartType.Spline;
            /*  Create a series from the regression fit to the data
             */
            foreach(DataPoint xPoint in TimeSeries.Points)
            {
                //DateTime xDate = DateTime.FromOADate(xPoint.XValue);        //Unsure if OADate is the form that datetime points are being stored as
                fitSeries.Points.AddXY(xPoint.XValue, fitRegression.GetValue(xPoint.XValue));
            }
            fitSeries.BorderWidth = 3;
            fitSeries.Color = System.Drawing.Color.Red;
            return fitSeries;
        }
        private Series GenerateErrorSeries_Lower(IRegression fitRegression)
        {
            Series errorSeries = new Series();
            errorSeries.ChartType = SeriesChartType.StackedArea;
            double minX = (from DataPoint point in TimeSeries.Points select point.XValue).Min();
            double maxX = (from DataPoint point in TimeSeries.Points select point.XValue).Max();
            for (int i = 0; i <= 100; i++)
            {
                double xVal = minX + (i * ((maxX - minX) / 100));
                double yVal = fitRegression.GetConfidenceInterval(xVal, 0.95).Min;
                DataPoint newDP = new DataPoint(xVal, yVal);
                errorSeries.Points.Add(newDP);
            }
            errorSeries.Color = System.Drawing.Color.FromArgb(0, errorSeries.Color);
            return errorSeries;
        }
        private Series GenerateErrorSeries_Upper(IRegression fitRegression)
        {
            Series errorSeries = new Series();
            errorSeries.ChartType = SeriesChartType.StackedArea;
            double minX = (from DataPoint point in TimeSeries.Points select point.XValue).Min();
            double maxX = (from DataPoint point in TimeSeries.Points select point.XValue).Max();
            for (int i = 0; i <= 100; i++)
            {
                double xVal = minX + (i*((maxX-minX)/100));
                double yVal = fitRegression.GetConfidenceInterval(xVal, 0.95).Max;
                DataPoint newDP = new DataPoint(xVal, yVal);
                errorSeries.Points.Add(newDP);
            }
            errorSeries.Color = System.Drawing.Color.FromArgb(50, System.Drawing.Color.Gray);
            return errorSeries;
        }
        private BoxPlotSeries GenerateBoxPlotSeries(IRegression fitRegression, double xValue)
        {
            BoxPlotSeries boxPlotSeries = new BoxPlotSeries();
            double min = fitRegression.GetConfidenceInterval(xValue, alpha).Min;
            double max = fitRegression.GetConfidenceInterval(xValue, alpha).Max;
            double q2 = fitRegression.GetValue(xValue);
            double q1 = min + (q2 - min) / 2;
            double q3 = q2 + (max - q2) / 2;
            BoxPlot boxPlot = new BoxPlot(min, q1, q2, q3, max, q2);
            boxPlotSeries.Points.Add(boxPlot.GetBoxPlot(xValue));
            boxPlotSeries.Color = System.Drawing.Color.Orange;
            boxPlotSeries.BorderColor = System.Drawing.Color.Black;
            boxPlotSeries.BorderWidth = 2;
            //this.chartArea.Position = new ElementPosition((float)0.2, (float)0.2, (float)0.8 * this.Width, (float)0.8 * this.Height);
            int ppw = Convert.ToInt32(500 / TimeSeries.Points.Count());
            boxPlotSeries["PixelPointWidth"] = ppw.ToString();
            return boxPlotSeries;
        }

        private void OnChartClick(object sender, EventArgs e)
        {
            //Select the nearest timeseries datapoint
            double cursorX = this.chartArea.CursorX.Position;
            double cursorY = this.chartArea.CursorY.Position;
            SelectedPoint sp = ChartUtilities.SelectDataPointNearToXY(cursorX, cursorY, TimeSeries);
            if (sp == null)
                return;
            else
            {
                sp.LoadDataLabel();
            }
        }
    }
}
