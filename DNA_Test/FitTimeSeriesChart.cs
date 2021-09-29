using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Windows.Forms;
using Accord.Statistics.Distributions.Univariate;

namespace DNA_Test
{
    public class FitTimeSeriesChart : TimeSeriesChart
    {
        public Scheduler.Scheduler Schedule { get; set; }
        private double predictAt { get; set; }

        public FitTimeSeriesChart(Dictionary<DateTime, double> timeSeriesDataPoints, IRegression fitRegression, Scheduler.Scheduler schedule, int predictAtIndex) : base(timeSeriesDataPoints, fitRegression)
        {            
            //Change the fit options and it loads a new time series chart
            //Each time series chart can have multiple time series, fit series -- and thus regressions & boxplot series.
            //Currently only works with one of each, but will need adapted at some point.

            /*  Set the default xAxis
             *  Load the Chart series'
             */
            this.Schedule = schedule;
            SetupXAxisGridlines();
            TimeSeries = GenerateTimeSeries(timeSeriesDataPoints);
            this.Series.Add(TimeSeries);

            this.SetAxes(TimeSeries.Points.First().XValue, TimeSeries.Points.Last().XValue);
            if (predictAtIndex == 0)
                this.UpdateBoxPlotSeries(Prediction.AtNextInterval);
            if (predictAtIndex == 1)
                this.UpdateBoxPlotSeries(Prediction.AtMean);
        }

        protected override void SetupXAxisGridlines()
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
            for (int i = 0; i <= 100; i++)
            {
                double xVal = chartArea.AxisX.Minimum + (step * i);
                fitSeries.Points.AddXY(xVal, fitRegression.GetValue(xVal));
            }
            fitSeries.BorderWidth = 3;
            fitSeries.Color = System.Drawing.Color.Red;
            return fitSeries;
        }
        protected override Series GenerateErrorSeries_Lower()
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
                double yVal = FitRegression.GetConfidenceInterval(xVal, alpha).Min;
                DataPoint newDP = new DataPoint(xVal, yVal);
                errorSeries.Points.Add(newDP);
            }
            errorSeries.Color = System.Drawing.Color.FromArgb(0, errorSeries.Color);
            return errorSeries;
        }
        protected override Series GenerateErrorSeries_Upper()
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
                double xVal = minX + (i * ((maxX - minX) / 100));
                double yVal = FitRegression.GetConfidenceInterval(xVal, alpha).Max;
                double offset = FitRegression.GetConfidenceInterval(xVal, alpha).Min;
                //Because this is a stacked area, we need to subtract off the min to align the upper boundary with the CI max
                DataPoint newDP = new DataPoint(xVal, yVal - offset);
                errorSeries.Points.Add(newDP);
            }
            errorSeries.Color = System.Drawing.Color.FromArgb(50, System.Drawing.Color.Gray);
            return errorSeries;
        }

        public enum Prediction
        {
            AtNextInterval,
            AtMean,
            AtValue
        }
        public void UpdateBoxPlotSeries(Prediction prediction)
        {
            double xValue;
            switch (prediction)
            {
                case Prediction.AtNextInterval:
                    //Need to pull the scheduler in as a property and get the next
                    xValue = this.Schedule.GetNextMidpoint().ToOADate();
                    break;
                case Prediction.AtMean:
                    xValue = (from DataPoint dp in this.TimeSeries.Points select dp.XValue).Average();
                    break;
                case Prediction.AtValue:
                    throw new Exception("Must provide xValue parameter");
                default:
                    throw new Exception("Unknown enum");
            }

            UpdateBoxPlotSeries(prediction, xValue);

        }
        public void UpdateBoxPlotSeries(Prediction predictAt, double xValue)
        {
            switch (predictAt)
            {
                case Prediction.AtNextInterval:
                    //Need to pull the scheduler in as a property and get the next
                    SetAxes(this.TimeSeries.Points.First().XValue, this.Schedule.GetNextEndpoint().ToOADate());
                    break;
                case Prediction.AtMean:
                    SetAxes(this.TimeSeries.Points.First().XValue, this.Schedule.GetMidpoints().Last().ToOADate());
                    break;
                case Prediction.AtValue:

                    SetAxes(this.TimeSeries.Points.First().XValue, this.Schedule.GetEndpoints().Last().ToOADate());
                    break;
                default:
                    throw new Exception("Unknown enum");
            }

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
            Scheduler.Scheduler valueSchedule = new Scheduler.Scheduler(Schedule.GetIntervalLength(), (Scheduler.Scheduler.Interval)Schedule.GetIntervalType(), Schedule.GetStartDate(), DateTime.FromOADate(xValue));
            double max = Math.Max(this.chartArea.AxisX.Maximum, valueSchedule.GetEndpoints().Last().ToOADate());
            SetAxes(this.chartArea.AxisX.Minimum, max);

            this.Series.Add(this.BoxPlot_Series.PrimarySeries);
            this.Series.Add(this.BoxPlot_Series.LabelSeries);
            this.Series.Add(this.BoxPlot_Series.MeanSeries);

            //Adapt min/max -- next interval max is endpoint. boxplot goes to midpoint.
            //Redraw fit, error
            ScaleToAxes();
        }
        protected override BoxPlotSeries GenerateBoxPlotSeries(IRegression fitRegression, double xValue)
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

        protected override void OnChartClick(object sender, EventArgs e)
        {
            SelectedPoint sp;

            //Make the selection -- Requires MouseCoords to be set by OnMouseMoved event handler
            HitTestResult htr = this.HitTest(MouseCoords.X, MouseCoords.Y);
            if (htr.ChartElementType == ChartElementType.DataPoint)
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
            else if (sp.parent.Name == "BoxPlotSeries_BoxPlots" || sp.parent.Name == "BoxPlotSeries_Labels")
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
            else if (sp.parent.Name == "TimeSeries")
            {
                if (sp.datapoint.Label != "")
                {   //Not blank, then clear it
                    sp.datapoint.Label = "";
                }
                else
                {   //If it is blank, clear all other labels and print this one
                    foreach (DataPoint dp in this.TimeSeries.Points)
                        dp.Label = "";
                    sp.LoadDataLabel("date");
                }
            }
        }
        
        private void ScaleToAxes()
        {
            //You have been given the x min and x max.
            //Derive the y min and y max
            //Redraw fit & error for the new axes
            if (this.Series.Contains(this.ErrorSeries_CI_Lower))
                this.Series.Remove(this.ErrorSeries_CI_Lower);
            this.ErrorSeries_CI_Lower = GenerateErrorSeries_Lower();
            this.Series.Add(this.ErrorSeries_CI_Lower);

            if (this.Series.Contains(this.ErrorSeries_CI_Upper))
                this.Series.Remove(this.ErrorSeries_CI_Upper);
            this.ErrorSeries_CI_Upper = GenerateErrorSeries_Upper();
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
            for (int i = 0; i <= 100; i++)
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
            float scalar = this.chartArea.InnerPlotPosition.Width / 100;
            double chartAreaWidth = scalar * this.Width;
            double minX = this.chartArea.AxisX.Minimum;
            double maxX = this.chartArea.AxisX.Maximum;
            double xRange = maxX - minX;
            return xRange / chartAreaWidth;
        }
        public double Get_Y_Coords_Per_Pixel()
        {
            //Need to account for the small gap between the datapoint and the end?
            float scalar = (this.chartArea.Position.Height / 100) * (this.chartArea.InnerPlotPosition.Height / 100);
            double chartAreaHeight = scalar * this.Height;
            double minY = this.chartArea.AxisY.Minimum;
            double maxY = this.chartArea.AxisY.Maximum;
            double yRange = maxY - minY;
            return yRange / chartAreaHeight;
        }

        protected override void SetupDescription()
        {
            Description = new Title();
            Description.Visible = false;
            
            Description.BackColor = System.Drawing.Color.White;
            //Description.BorderColor = System.Drawing.Color.Black;
            double scale = 528 / this.Height * 4;      //528 is the pixel height of the single chart. this.height = 528. 10 is the relative height of the single chart
            Description.Position = new ElementPosition(chartArea.InnerPlotPosition.X, (float)0.3/*chartArea.InnerPlotPosition.Y*/, 88, Convert.ToInt32(scale));
            this.Description.Text = $"{FitRegression.ToString()};  Start: {Schedule.GetStartDate().ToString()};  End: {Schedule.GetEndDate().ToString()}";
            this.Titles.Add(Description);
        }
    }
}
