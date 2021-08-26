﻿using System;
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
        public Series ErrorSeries_Upper { get; set; }  //The regression line + error band
        public Series ErrorSeries_Lower { get; set; }  //The regression line - error band
        private PDF_Popup pdf_Popup { get; set; }

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
            this.Series.Add(TimeSeries);
            FitSeries = GenerateFitSeries(fitRegression);
            this.Series.Add(FitSeries);
            ErrorSeries_Lower = GenerateErrorSeries_Lower(fitRegression);
            this.Series.Add(ErrorSeries_Lower);
            ErrorSeries_Upper = GenerateErrorSeries_Upper(fitRegression);
            this.Series.Add(ErrorSeries_Upper);
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
            fitSeries.Color = System.Drawing.Color.Orange;
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
                double yVal = fitRegression.GetRegressionConfidenceInterval(xVal, 0.95).Min;
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
                double yVal = fitRegression.GetRegressionConfidenceInterval(xVal, 0.95).Max;
                DataPoint newDP = new DataPoint(xVal, yVal);
                errorSeries.Points.Add(newDP);
            }
            errorSeries.Color = System.Drawing.Color.FromArgb(50, System.Drawing.Color.Gray);
            return errorSeries;
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
