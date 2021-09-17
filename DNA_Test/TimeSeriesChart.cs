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
        private Scheduler.Scheduler Schedule { get; set; }
        private Point MouseCoords { get; set; }      
        private const double alpha = 0.98;
        public static int default_chartHeight = 100;    //Overwritable defaults
        public static int default_chartWidth = 100;
        public ChartArea chartArea { get; set; }        //The x-Axis -- potentially overwritten for non-uniform
        public Series TimeSeries { get; set; } //The data (potentially bucketed)
        public Series FitSeries { get; set; }  //The regression line
        public Series ErrorSeries_CI_Upper { get; set; }  //The regression line + error band
        public Series ErrorSeries_CI_Lower { get; set; }  //The regression line - error band
        public Series PDF_Series { get; set; }
        public BoxPlotSeries BoxPlot_Series { get; set; }
        private double predictAt { get; set; }
        private IRegression FitRegression { get; set; }

        public TimeSeriesChart(Dictionary<DateTime, double> timeSeriesDataPoints, IRegression fitRegression, Scheduler.Scheduler schedule, int predictAtIndex) : base()
        {
            /*  Set the default xAxis
             *  Load the Chart series'
             */
            this.Schedule = schedule;
            this.FitRegression = fitRegression;
            this.MouseMove += OnMouseMoved;
            this.Click += OnChartClick;
            this.Height = default_chartHeight;
            this.Width = default_chartWidth;
            chartArea = new ChartArea();
            this.ChartAreas.Add(chartArea);
            chartArea.Position = new ElementPosition(0, 0, 100, 100);
            chartArea.InnerPlotPosition = new ElementPosition(10, 5, 88, 88);
            SetupXAxisGridlines();
            //chartArea.AxisX.MajorTickMark.IntervalType = DateTimeIntervalType.

            this.EnableUserSelection();     //Has to come after chartArea

            TimeSeries = GenerateTimeSeries(timeSeriesDataPoints);
            this.Series.Add(TimeSeries);

            this.BorderlineDashStyle = ChartDashStyle.Solid;
            this.BorderlineColor = System.Drawing.Color.Black;
            this.BorderlineWidth = 2;
            this.chartArea.AxisX.MajorTickMark.Enabled = false;
            this.chartArea.AxisY.MajorTickMark.Enabled = false;
            this.chartArea.BorderDashStyle = ChartDashStyle.Solid;
            this.chartArea.BorderColor = System.Drawing.Color.Black;
            this.chartArea.BorderWidth = 2;
            

            this.chartArea.AxisY.LabelStyle.Format = "{0:n0}";

            this.SetAxes(TimeSeries.Points.First().XValue, TimeSeries.Points.Last().XValue);
            if (predictAtIndex == 0)
                this.UpdateBoxPlotSeries(Prediction.AtNextInterval);
            if (predictAtIndex == 1)
                this.UpdateBoxPlotSeries(Prediction.AtMean);
        }

        private void SetupXAxisGridlines()
        {
            double duration = (Schedule.GetEndDate() - Schedule.GetStartDate()).TotalDays;
            switch (Schedule.GetIntervalType())
            {
                case Scheduler.Scheduler.Interval.Yearly:
                    this.chartArea.AxisX.IntervalType = DateTimeIntervalType.Years;
                    this.chartArea.AxisX.Interval = 1;
                    break;
                case Scheduler.Scheduler.Interval.Monthly:
                    this.chartArea.AxisX.IntervalType = DateTimeIntervalType.Months;
                    this.chartArea.AxisX.Interval = Math.Round(duration / 30.417 / 10, 0);
                    break;
                case Scheduler.Scheduler.Interval.Weekly:
                    this.chartArea.AxisX.IntervalType = DateTimeIntervalType.Weeks;
                    this.chartArea.AxisX.Interval = Math.Round(duration / 7 / 10, 0);
                    break;
                case Scheduler.Scheduler.Interval.Daily:
                    this.chartArea.AxisX.IntervalType = DateTimeIntervalType.Days;
                    this.chartArea.AxisX.Interval = Math.Round(duration / 10, 0);
                    break;
                default:
                    throw new Exception("Unknown interval type");
            }
        }

        private void EnableUserSelection()
        {
            if (this.chartArea == null)
                throw new Exception("Define chartArea first.");
            this.chartArea.CursorX.IsUserEnabled = true;
            this.chartArea.CursorY.IsUserEnabled = true;
            this.chartArea.CursorX.Interval = 0.01;
            this.chartArea.CursorY.Interval = 0.01;
        }

        private Series GenerateTimeSeries(Dictionary<DateTime, double> timeSeriesDataPoints)
        {
            Series timeSeries = new Series();
            timeSeries.XValueType = ChartValueType.Date;
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
            //Assumes axis max & min have been set correctly. Fills in 100 fit points
            Series fitSeries = new Series();
            fitSeries.XValueType = ChartValueType.Date;
            fitSeries.Name = "FitSeries";
            fitSeries.ChartType = SeriesChartType.Spline;
            /*  Create a series from the regression fit to the data
             */
            double step = (chartArea.AxisX.Maximum - chartArea.AxisX.Minimum) / 100;
            for(int i=0; i <=100; i++)
            {
                double xVal = chartArea.AxisX.Minimum + (step * i);
                fitSeries.Points.AddXY(xVal, fitRegression.GetValue(xVal));
            }
            fitSeries.BorderWidth = 3;
            fitSeries.Color = System.Drawing.Color.Red;
            return fitSeries;
        }
        private Series GenerateErrorSeries_Lower(IRegression fitRegression)
        {
            Series errorSeries = new Series();
            errorSeries.XValueType = ChartValueType.Date;
            errorSeries.Name = "ErrorSeries_CI_Lower";
            errorSeries.ChartType = SeriesChartType.StackedArea;
            double minX = this.chartArea.AxisX.Minimum;
            double maxX = this.chartArea.AxisX.Maximum;
            double step = (maxX - minX) / 100;
            for (int i = 0; i <= 100; i++)
            {
                double xVal = minX + (i * step);
                double yVal = fitRegression.GetConfidenceInterval(xVal, alpha).Min;
                DataPoint newDP = new DataPoint(xVal, yVal);
                errorSeries.Points.Add(newDP);
            }
            errorSeries.Color = System.Drawing.Color.FromArgb(0, errorSeries.Color);
            return errorSeries;
        }
        private Series GenerateErrorSeries_Upper(IRegression fitRegression)
        {
            Series errorSeries = new Series();
            errorSeries.XValueType = ChartValueType.Date;
            errorSeries.Name = "ErrorSeries_CI_Upper";
            errorSeries.ChartType = SeriesChartType.StackedArea;
            double minX = this.chartArea.AxisX.Minimum;
            double maxX = this.chartArea.AxisX.Maximum;
            double step = (maxX - minX) / 100;
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

        private void FixSeriesOrder()
        {
            //Re-order the series so they appear correctly.
            Queue<Series> mySeries = new Queue<Series>();
            if (this.Series.IndexOf("ErrorSeries_CI_Lower") != -1)
                mySeries.Enqueue(this.Series.FindByName("ErrorSeries_CI_Lower"));
            if (this.Series.IndexOf("ErrorSeries_CI_Upper") != -1)
                mySeries.Enqueue(this.Series.FindByName("ErrorSeries_CI_Upper"));
            if (this.Series.IndexOf("TimeSeries") != -1)
                mySeries.Enqueue(this.Series.FindByName("TimeSeries"));
            if (this.Series.IndexOf("BoxPlotSeries_BoxPlots") != -1)
                mySeries.Enqueue(this.Series.FindByName("BoxPlotSeries_BoxPlots"));
            if (this.Series.IndexOf("FitSeries") != -1)
                mySeries.Enqueue(this.Series.FindByName("FitSeries"));
            if (this.Series.IndexOf("BoxPlotSeries_Labels") != -1)
                mySeries.Enqueue(this.Series.FindByName("BoxPlotSeries_Labels"));
            if (this.Series.IndexOf("BoxPlotSeries_Means") != -1)
                mySeries.Enqueue(this.Series.FindByName("BoxPlotSeries_Means"));
            this.Series.Clear();
            while (mySeries.Any())
            {
                this.Series.Add(mySeries.Dequeue());
            }
        }

        public enum Prediction
        {
            AtNextInterval,
            AtMean
        }
        public void UpdateBoxPlotSeries(Prediction prediction)
        {
            double xValue;
            switch (prediction)
            {
                case Prediction.AtNextInterval:
                    //Need to pull the scheduler in as a property and get the next
                    xValue = this.Schedule.GetNextMidpoint().ToOADate();
                    SetAxes(this.TimeSeries.Points.First().XValue, this.Schedule.GetNextEndpoint().ToOADate());
                    break;
                case Prediction.AtMean:
                    xValue = (from DataPoint dp in this.TimeSeries.Points select dp.XValue).Average();
                    SetAxes(this.TimeSeries.Points.First().XValue, this.Schedule.GetMidpoints().Last().ToOADate());
                    break;
                default:
                    throw new Exception("Unknown enum");
            }
            
            UpdateBoxPlotSeries(xValue);

        }
        public void UpdateBoxPlotSeries(double xValue)
        {
            if (BoxPlot_Series != null)
            {
                if (Series.Contains(BoxPlot_Series.PrimarySeries))
                    this.Series.Remove(BoxPlot_Series.PrimarySeries);
                if (Series.Contains(BoxPlot_Series.LabelSeries))
                    this.Series.Remove(BoxPlot_Series.LabelSeries);
                if (Series.Contains(BoxPlot_Series.MeanSeries))
                    this.Series.Remove(BoxPlot_Series.MeanSeries);
            }
            this.BoxPlot_Series = GenerateBoxPlotSeries(this.FitRegression, xValue);
            this.BoxPlot_Series.SetBoxPlotColor_OnFitSeries();
            this.Series.Add(this.BoxPlot_Series.PrimarySeries);
            this.Series.Add(this.BoxPlot_Series.LabelSeries);
            this.Series.Add(this.BoxPlot_Series.MeanSeries);

            //Adapt min/max -- next interval max is endpoint. boxplot goes to midpoint.
            //Redraw fit, error
            ScaleToAxes();
        }
        private BoxPlotSeries GenerateBoxPlotSeries(IRegression fitRegression, double xValue)
        {
            BoxPlotSeries boxPlotSeries = new BoxPlotSeries(this);
            double min = fitRegression.GetConfidenceInterval(xValue, alpha).Min;
            double max = fitRegression.GetConfidenceInterval(xValue, alpha).Max;
            double q2 = fitRegression.GetValue(xValue);
            double q1 = min + (q2 - min) / 2;
            double q3 = q2 + (max - q2) / 2;
            BoxPlot boxPlot = new BoxPlot(min, q1, q2, q3, max);
            boxPlotSeries.SetWidth(TimeSeries.Points.Count());
            boxPlotSeries.Add(xValue, boxPlot, FitRegression.GetValue(xValue));
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
                    this.Series.Add(this.PDF_Series);
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

        public void SetAxes(double xMin, double xMax)
        {
            this.chartArea.AxisX.Minimum = xMin;
            this.chartArea.AxisX.Maximum = xMax;
        }

        private void ScaleToAxes()
        {
            //You have been given the x min and x max.
            //Derive the y min and y max
            //Redraw fit & error for the new axes
            if (this.Series.Contains(this.ErrorSeries_CI_Lower))
                this.Series.Remove(this.ErrorSeries_CI_Lower);
            this.ErrorSeries_CI_Lower = GenerateErrorSeries_Lower(FitRegression);
            this.Series.Add(this.ErrorSeries_CI_Lower);

            if (this.Series.Contains(this.ErrorSeries_CI_Upper))
                this.Series.Remove(this.ErrorSeries_CI_Upper);
            this.ErrorSeries_CI_Upper = GenerateErrorSeries_Upper(FitRegression);
            this.Series.Add(this.ErrorSeries_CI_Upper);

            if (this.Series.Contains(this.FitSeries))
                this.Series.Remove(this.FitSeries);
            this.FitSeries = GenerateFitSeries(FitRegression);
            this.Series.Add(this.FitSeries);
            
            //Search the fitseries for minimum & maximum
            double[] upperPoints = (from DataPoint pt in this.ErrorSeries_CI_Upper.Points select pt.YValues[0]).ToArray();
            double[] lowerPoints = (from DataPoint pt in this.ErrorSeries_CI_Lower.Points select pt.YValues[0]).ToArray();
            double tempMaxY = double.MinValue;
            int maxIndex = -1;
            for (int i=0; i <= 100; i++)
            {   //Add up the stacks & account for negative lowerPoints
                double actualY = lowerPoints[i] + upperPoints[i];
                if (actualY > tempMaxY)
                {
                    tempMaxY = actualY;
                    maxIndex = i;
                }
            }
            double maxY = upperPoints[maxIndex] + lowerPoints[maxIndex];
            double minY = lowerPoints.Min();

            this.chartArea.AxisY.Minimum = minY - (upperPoints[maxIndex] * 0.1);    //Pad 10% of the range (yHigh represents the range)
            this.chartArea.AxisY.Maximum = maxY + (upperPoints[maxIndex] * 0.1);      //Add them due to how yHigh's y-value is calculated for a stacked area
            FixSeriesOrder();
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
