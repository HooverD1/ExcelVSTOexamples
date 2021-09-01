using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Accord.Statistics.Distributions.Univariate;

namespace DNA_Test
{
    public class TimeSeriesChart : Chart
    {
        private Point MouseCoords { get; set; }      
        private const double alpha = 0.95;
        public static int default_chartHeight = 100;    //Overwritable defaults
        public static int default_chartWidth = 100;
        public ChartArea chartArea { get; set; }        //The x-Axis -- potentially overwritten for non-uniform
        public Series TimeSeries { get; set; } //The data (potentially bucketed)
        public Series FitSeries { get; set; }  //The regression line
        public Series ErrorSeries_CI_Upper { get; set; }  //The regression line + error band
        public Series ErrorSeries_CI_Lower { get; set; }  //The regression line - error band
        public Series PDF_Series { get; set; }
        public BoxPlotSeries BoxPlot_Series { get; set; }
        public Series MeanSeries { get; set; }
        private double predictAt { get; set; }
        private IRegression FitRegression { get; set; }

        public TimeSeriesChart(Dictionary<DateTime, double> timeSeriesDataPoints, IRegression fitRegression) : base()
        {
            /*  Set the default xAxis
             *  Load the Chart series'
             */
            this.FitRegression = fitRegression;
            this.MouseMove += OnMouseMoved;
            this.Click += OnChartClick;
            this.Height = default_chartHeight;
            this.Width = default_chartWidth;
            chartArea = new ChartArea();
            this.ChartAreas.Add(chartArea);
            chartArea.Position = new ElementPosition(0, 0, 100, 100);
            chartArea.InnerPlotPosition = new ElementPosition(10, 5, 88, 88);
            this.EnableUserSelection();     //Has to come after chartArea
            TimeSeries = GenerateTimeSeries(timeSeriesDataPoints);
            FitSeries = GenerateFitSeries(fitRegression);
            ScaleAxesToFitSeries();
            this.predictAt = (from DataPoint dp in TimeSeries.Points select dp.XValue).Average();
            BoxPlot_Series = GenerateBoxPlotSeries(fitRegression, predictAt);
            ErrorSeries_CI_Lower = GenerateErrorSeries_Lower(fitRegression);
            ErrorSeries_CI_Upper = GenerateErrorSeries_Upper(fitRegression);
                       
            this.Series.Add(ErrorSeries_CI_Lower);
            this.Series.Add(ErrorSeries_CI_Upper);
            this.Series.Add(BoxPlot_Series.PrimarySeries);
            this.Series.Add(BoxPlot_Series.LabelSeries);
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
            //this.chartArea.CursorX.IsUserSelectionEnabled = true;
            //this.chartArea.CursorY.IsUserSelectionEnabled = true;
            this.chartArea.CursorX.Interval = 0.01;
            this.chartArea.CursorY.Interval = 0.01;
        }

        private Series GenerateTimeSeries(Dictionary<DateTime, double> timeSeriesDataPoints)
        {
            Series timeSeries = new Series();
            timeSeries.Name = "TimeSeries";
            timeSeries.ChartType = SeriesChartType.Line;
            timeSeries.MarkerStyle = MarkerStyle.Circle;
            foreach(KeyValuePair<DateTime, double> datapoint in timeSeriesDataPoints)
            {
                DataPoint dp = new DataPoint(datapoint.Key.ToOADate(), datapoint.Value);
                dp.ToolTip = $"({datapoint.Key.ToShortDateString()}, {datapoint.Value})";
                timeSeries.Points.Add(dp);
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
            fitSeries.Name = "FitSeries";
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
            errorSeries.Name = "ErrorSeries_CI_Lower";
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
            errorSeries.Name = "ErrorSeries_CI_Upper";
            errorSeries.ChartType = SeriesChartType.StackedArea;
            double minX = (from DataPoint point in TimeSeries.Points select point.XValue).Min();
            double maxX = (from DataPoint point in TimeSeries.Points select point.XValue).Max();
            for (int i = 0; i <= 100; i++)
            {
                double xVal = minX + (i*((maxX-minX)/100));
                double yVal = fitRegression.GetConfidenceInterval(xVal, alpha).Max;
                double offset = fitRegression.GetConfidenceInterval(xVal, alpha).Min;
                //Because this is a stacked area, we need to subtract off the min to align the upper boundary with the CI max
                DataPoint newDP = new DataPoint(xVal, yVal - offset);
                errorSeries.Points.Add(newDP);
            }
            errorSeries.Color = System.Drawing.Color.FromArgb(50, System.Drawing.Color.Gray);
            return errorSeries;
        }
        private BoxPlotSeries GenerateBoxPlotSeries(IRegression fitRegression, double xValue)
        {
            BoxPlotSeries boxPlotSeries = new BoxPlotSeries(this);
            double min = fitRegression.GetConfidenceInterval(xValue, alpha).Min;
            double max = fitRegression.GetConfidenceInterval(xValue, alpha).Max;
            double q2 = fitRegression.GetValue(xValue);
            double q1 = min + (q2 - min) / 2;
            double q3 = q2 + (max - q2) / 2;
            BoxPlot boxPlot = new BoxPlot(min, q1, q2, q3, max, q2);
            boxPlotSeries.SetWidth(TimeSeries.Points.Count());
            boxPlotSeries.Add(xValue, boxPlot);
            return boxPlotSeries;
        }

        private void OnMouseMoved(object sender, MouseEventArgs e)
        {
            MouseCoords = new Point(e.X, e.Y);
        }

        private void OnChartClick(object sender, EventArgs e)
        {
            SelectedPoint sp;

            //Make the selection -- Requires MouseCoords to be set by OnMouseMoved event handler
            HitTestResult htr = this.HitTest(MouseCoords.X, MouseCoords.Y);
            if(htr.ChartElementType == ChartElementType.DataPoint)
            {
                sp = new SelectedPoint((DataPoint)htr.Object, htr.Series);
            }
            else
            {
                sp = null;
            }
            
            //Handle the selection
            if (sp == null)
                return;
            else if(sp.parent.Name == "BoxPlotSeries_BoxPlots" || sp.parent.Name == "BoxPlotSeries_Labels")
            {
                //Load the pop-up PDF
                if (this.Series.Contains(PDF_Series))
                {
                    this.Series.Remove(this.PDF_Series);
                    this.Series.Add(this.BoxPlot_Series.LabelSeries);
                }
                else
                {
                    double xMin = this.chartArea.AxisX.Minimum;
                    double xMax = this.chartArea.AxisX.Maximum;
                    double xWidth = xMax - xMin;
                    PDF_Popup pdfPopUp = new PDF_Popup(sp.datapoint.XValue, xWidth, this.chartArea.AxisY.Minimum, this.chartArea.AxisY.Maximum);
                    this.PDF_Series = pdfPopUp.GetSeries(new NormalDistribution(sp.datapoint.YValues[4], sp.datapoint.YValues[4] / 3));
                    this.Series.Add(PDF_Series);
                    this.Series.Remove(this.BoxPlot_Series.LabelSeries);
                }
            }
            else if(sp.parent.Name == "TimeSeries")
            {
                sp.LoadDataLabel();
            }
        }

        private void ScaleAxesToFitSeries()
        {
            IEnumerable<double> xVals = from DataPoint x in this.FitSeries.Points select x.XValue;
            IEnumerable<double> yVals = from DataPoint y in this.FitSeries.Points select y.YValues[0];

            double minX = xVals.Min();
            double maxX = xVals.Max();
            
            double minY = yVals.Min();
            double maxY = yVals.Max();
            //Search the fitseries for minimum
            DataPoint yLow = (from DataPoint pt in this.FitSeries.Points where pt.YValues[0] == minY select pt).First();
            double plotMinY = FitRegression.GetConfidenceInterval(yLow.XValue, alpha).Min;
            DataPoint yHigh = (from DataPoint pt in this.FitSeries.Points where pt.YValues[0] == maxY select pt).First();
            double plotMaxY = FitRegression.GetConfidenceInterval(yHigh.XValue, alpha).Max;
            double yRange = plotMaxY - plotMinY;

            this.chartArea.AxisX.Minimum = minX;
            this.chartArea.AxisX.Maximum = maxX;

            this.chartArea.AxisY.Minimum = plotMinY - (yRange * 0.1);   //Pad 50% of the range above and below
            this.chartArea.AxisY.Maximum = plotMaxY + (yRange * 0.1);
        }

        public double Get_X_Coords_Per_Pixel()
        {
            //Need to account for the small gap between the datapoint and the end?
            float scalar = this.chartArea.InnerPlotPosition.Width/100;
            double chartAreaWidth = scalar * this.Width;
            double minX = this.chartArea.AxisX.Minimum;
            double maxX = this.chartArea.AxisX.Maximum;
            double xRange = maxX - minX;
            return xRange / chartAreaWidth;
        }
        public double Get_Y_Coords_Per_Pixel()
        {
            //Need to account for the small gap between the datapoint and the end?
            float scalar = (this.chartArea.Position.Height/100) * (this.chartArea.InnerPlotPosition.Height / 100);
            double chartAreaHeight = scalar * this.Height;
            double minY = this.chartArea.AxisY.Minimum;
            double maxY = this.chartArea.AxisY.Maximum;
            double yRange = maxY - minY;
            return yRange / chartAreaHeight;
        }

    }
}
