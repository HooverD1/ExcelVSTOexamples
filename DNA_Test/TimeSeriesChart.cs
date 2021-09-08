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
        private Scheduler.Scheduler Schedule { get; set; }

        public TimeSeriesChart(Dictionary<DateTime, double> timeSeriesDataPoints, Scheduler.Scheduler schedule, IRegression fitRegression) : base()
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
            this.EnableUserSelection();     //Has to come after chartArea

            //These series go where they need to go
            TimeSeries = GenerateTimeSeries(timeSeriesDataPoints);
            BoxPlot_Series = GenerateBoxPlotSeries(fitRegression, predictAt);
            SetAxis_X();

            //These series adapt to what is in the time series and box plot series
            FitSeries = GenerateFitSeries();
            ErrorSeries_CI_Lower = GenerateErrorSeries_Lower();
            ErrorSeries_CI_Upper = GenerateErrorSeries_Upper();
            SetAxis_Y();

            this.Series.Add(TimeSeries);
            this.Series.Add(FitSeries);
            this.Series.Add(BoxPlot_Series.PrimarySeries);
            this.Series.Add(BoxPlot_Series.LabelSeries);
            this.Series.Add(ErrorSeries_CI_Lower);
            this.Series.Add(ErrorSeries_CI_Upper);

            FixSeriesOrdering();

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
        private Series GenerateFitSeries()     //Feed this the regression used to fit the time series
        {
            Series fitSeries = new Series();
            fitSeries.Name = "FitSeries";
            fitSeries.ChartType = SeriesChartType.Spline;
            /*  Create a series from the regression fit to the data
             */

            double xMin = this.chartArea.AxisX.Minimum;
            double xMax = this.chartArea.AxisX.Maximum;
            double step = (xMax - xMin) / 100;
            for(int i=0; i<=100;i++)
            {
                double xVal = xMin + (step * i);
                //DateTime xDate = DateTime.FromOADate(xPoint.XValue);        //Unsure if OADate is the form that datetime points are being stored as
                fitSeries.Points.AddXY(xVal, FitRegression.GetValue(xVal));
            }
            fitSeries.BorderWidth = 3;
            fitSeries.Color = System.Drawing.Color.Red;
            return fitSeries;
        }
        private Series GenerateErrorSeries_Lower()
        {
            Series errorSeries = new Series();
            errorSeries.Name = "ErrorSeries_CI_Lower";
            errorSeries.ChartType = SeriesChartType.StackedArea;
            double minX = this.chartArea.AxisX.Minimum;
            double maxX = this.chartArea.AxisX.Maximum;
            for (int i = 0; i <= 100; i++)
            {
                double xVal = minX + (i * ((maxX - minX) / 100));
                double yVal = FitRegression.GetConfidenceInterval(xVal, 0.95).Min;
                DataPoint newDP = new DataPoint(xVal, yVal);
                errorSeries.Points.Add(newDP);
            }
            errorSeries.Color = System.Drawing.Color.FromArgb(0, errorSeries.Color);
            return errorSeries;
        }
        private Series GenerateErrorSeries_Upper()
        {
            Series errorSeries = new Series();
            errorSeries.Name = "ErrorSeries_CI_Upper";
            errorSeries.ChartType = SeriesChartType.StackedArea;
            double minX = this.chartArea.AxisX.Minimum;
            double maxX = this.chartArea.AxisX.Maximum;
            for (int i = 0; i <= 100; i++)
            {
                double xVal = minX + (i*((maxX-minX)/100));
                double yVal = FitRegression.GetConfidenceInterval(xVal, alpha).Max;
                double offset = FitRegression.GetConfidenceInterval(xVal, alpha).Min;
                //Because this is a stacked area, we need to subtract off the min to align the upper boundary with the CI max
                DataPoint newDP = new DataPoint(xVal, yVal - offset);
                errorSeries.Points.Add(newDP);
            }
            errorSeries.Color = System.Drawing.Color.FromArgb(50, System.Drawing.Color.Gray);
            return errorSeries;
        }

        private void SetAxis_X()
        {
            double maxTimePoint = (from DataPoint dp in this.TimeSeries.Points select dp.XValue).Max();
            double minTimePoint = (from DataPoint dp in this.TimeSeries.Points select dp.XValue).Min();

            double maxBoxPoint = (from DataPoint dp in this.BoxPlot_Series.PrimarySeries.Points select dp.XValue).Max();
            double minBoxPoint = (from DataPoint dp in this.BoxPlot_Series.PrimarySeries.Points select dp.XValue).Min();

            double maxPoint = Math.Max(maxTimePoint, maxBoxPoint);
            double minPoint = Math.Min(minTimePoint, minBoxPoint);

            this.chartArea.AxisX.Minimum = minPoint;
            this.chartArea.AxisX.Maximum = maxPoint;
        }
        private void SetAxis_Y()
        {
            DataPoint[] errorUpper = (from DataPoint dp in this.ErrorSeries_CI_Upper.Points select dp).ToArray();
            DataPoint[] errorLower = (from DataPoint dp in this.ErrorSeries_CI_Lower.Points select dp).ToArray();
            double[] errorSum = new double[errorUpper.Length];
            for(int i=0; i<errorUpper.Length; i++)
            {
                errorSum[i] = errorLower[i].YValues[0] + errorUpper[i].YValues[0];
            }

            double maxErrorPoint = errorSum.Max();
            double minErrorpoint = (from DataPoint dp in errorLower select dp.YValues[0]).Min();

            double maxTimePoint = (from DataPoint dp in this.TimeSeries.Points select dp.XValue).Max();
            double minTimePoint = (from DataPoint dp in this.TimeSeries.Points select dp.XValue).Min();

            double minPoint = Math.Min(minTimePoint, minErrorpoint);
            double maxPoint = Math.Max(maxTimePoint, maxErrorPoint);

            this.chartArea.AxisY.Minimum = minPoint;
            this.chartArea.AxisY.Maximum = maxPoint;
        }

        //public void FitAxesToSeries()
        //{
        //    double maxTimePoint = (from DataPoint dp in this.TimeSeries.Points select dp.XValue).Max();
        //    double minTimePoint = (from DataPoint dp in this.TimeSeries.Points select dp.XValue).Min();
        //    if(Double.IsNaN(this.chartArea.AxisX.Minimum) || Double.IsNaN(this.chartArea.AxisX.Maximum))
        //    {
        //        //No bounds set yet - set to the time series min/max and return
        //        this.chartArea.AxisX.Minimum = minTimePoint;
        //        this.chartArea.AxisX.Maximum = maxTimePoint;
        //        return;
        //    }

        //    //Fix the axis for boxplots if they exist
        //    if (this.BoxPlot_Series == null)
        //        return;

        //    //Alter the x-axis to fit in predictors outside of the schedule duration
        //    double maxBoxPoint = (from DataPoint dp in this.BoxPlot_Series.PrimarySeries.Points select dp.XValue).Max();
        //    double maxPoint = Math.Max(maxBoxPoint, maxTimePoint);
        //    this.chartArea.AxisX.Maximum = maxPoint;

        //    double minBoxPoint = (from DataPoint dp in this.BoxPlot_Series.PrimarySeries.Points select dp.XValue).Min();
        //    double minPoint = Math.Min(minBoxPoint, minTimePoint);
        //    this.chartArea.AxisX.Minimum = minPoint;
            
        //    //Redraw the fit and error series to extend them to the new max/min points
        //    this.Series.Remove(FitSeries);
        //    FitSeries = GenerateFitSeries();
        //    this.Series.Add(FitSeries);

        //    this.Series.Remove(ErrorSeries_CI_Lower);
        //    this.Series.Remove(ErrorSeries_CI_Upper);
        //    ErrorSeries_CI_Lower = GenerateErrorSeries_Lower();
        //    ErrorSeries_CI_Upper = GenerateErrorSeries_Upper();
        //    this.Series.Add(ErrorSeries_CI_Lower);
        //    this.Series.Add(ErrorSeries_CI_Upper);

        //    ScaleAxesToFitSeries();
                
        //}
        
        public void UpdateBoxPlotSeries(double predictAt)
        {
            if (BoxPlot_Series != null && BoxPlot_Series.PrimarySeries != null)
            {
                this.Series.Remove(BoxPlot_Series.PrimarySeries);
            }
            if (BoxPlot_Series != null && BoxPlot_Series.LabelSeries != null)
            {
                this.Series.Remove(BoxPlot_Series.LabelSeries);
            }
            if (PDF_Series != null)
            {
                this.Series.Remove(PDF_Series);
            }

            this.BoxPlot_Series = GenerateBoxPlotSeries(this.FitRegression, predictAt);
            this.Series.Add(BoxPlot_Series.PrimarySeries);
            this.Series.Add(BoxPlot_Series.LabelSeries);

            //SetAxis_X();
            //SetAxis_Y();
            FixSeriesOrdering();
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

        //private void ScaleAxesToFitSeries()
        //{
        //    IEnumerable<double> xVals = from DataPoint x in this.FitSeries.Points select x.XValue;
        //    IEnumerable<double> yVals = from DataPoint y in this.FitSeries.Points select y.YValues[0];

        //    double minX = this.chartArea.AxisX.Minimum;//xVals.Min();
        //    double maxX = this.chartArea.AxisX.Maximum;//xVals.Max();
            
        //    double minY = yVals.Min();
        //    double maxY = yVals.Max();
        //    //Search the fitseries for minimum
        //    DataPoint yLow = (from DataPoint pt in this.FitSeries.Points where pt.YValues[0] == minY select pt).First();
        //    double plotMinY = FitRegression.GetConfidenceInterval(yLow.XValue, alpha).Min;
        //    DataPoint yHigh = (from DataPoint pt in this.FitSeries.Points where pt.YValues[0] == maxY select pt).First();
        //    double plotMaxY = FitRegression.GetConfidenceInterval(yHigh.XValue, alpha).Max;
        //    double yRange = plotMaxY - plotMinY;

        //    this.chartArea.AxisX.Minimum = minX;
        //    this.chartArea.AxisX.Maximum = maxX;

        //    this.chartArea.AxisY.Minimum = plotMinY - (yRange * 0.1);   //Pad 50% of the range above and below
        //    this.chartArea.AxisY.Maximum = plotMaxY + (yRange * 0.1);
        //}

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
        public double GetTimeSeriesMean_X()
        {
            var xValues = from DataPoint dp in TimeSeries.Points select dp.XValue;
            if (xValues.Any())
                return xValues.Average();
            else
                throw new Exception("No points in TimeSeries");
        }
        public DateTime GetNextInterval()
        {
            if (Schedule == null)
                throw new Exception("No schedule found");
            if (!Schedule.GetMidpoints().Any())
                throw new Exception("Failed to create endpoints");

            double intLength = Schedule.GetIntervalLength();
            Scheduler.Scheduler.Interval intType = Schedule.GetIntervalType();
            DateTime endDate = Schedule.GetEndpoints().Last();
            
            switch (intType)
            {
                case Scheduler.Scheduler.Interval.Daily:
                    return DateTime.FromOADate((endDate.AddDays(intLength).ToOADate() + endDate.ToOADate())/2);
                case Scheduler.Scheduler.Interval.Weekly:
                    return DateTime.FromOADate((endDate.AddDays(intLength * 7).ToOADate() + endDate.ToOADate())/2);
                case Scheduler.Scheduler.Interval.Monthly:
                    return DateTime.FromOADate((endDate.AddMonths(Convert.ToInt32(intLength)).ToOADate() + endDate.ToOADate())/2);
                case Scheduler.Scheduler.Interval.Yearly:
                    return DateTime.FromOADate((endDate.AddYears(Convert.ToInt32(intLength)).ToOADate() + endDate.ToOADate())/2);
                default:
                    throw new Exception("Unexpected Interval Type");
            }
        }

        public void FixSeriesOrdering()
        {
            Dictionary<string, Series> seriesDict = new Dictionary<string, Series>();
            foreach(Series s in this.Series)
            {
                seriesDict.Add(s.Name, s);
            }
            this.Series.Clear();
            if (seriesDict.ContainsKey("ErrorSeries_CI_Lower"))
                this.Series.Add(seriesDict["ErrorSeries_CI_Lower"]);
            if (seriesDict.ContainsKey("ErrorSeries_CI_Upper"))
                this.Series.Add(seriesDict["ErrorSeries_CI_Upper"]);
            if (seriesDict.ContainsKey("BoxPlotSeries_BoxPlots"))
                this.Series.Add(seriesDict["BoxPlotSeries_BoxPlots"]);
            if (seriesDict.ContainsKey("BoxPlotSeries_Labels"))
                this.Series.Add(seriesDict["BoxPlotSeries_Labels"]);
            if (seriesDict.ContainsKey("TimeSeries"))
                this.Series.Add(seriesDict["TimeSeries"]);
            if (seriesDict.ContainsKey("FitSeries"))
                this.Series.Add(seriesDict["FitSeries"]);

        }
    }
}
