﻿using System;
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
    public abstract class TimeSeriesChart : DataChart
    {
        public Dictionary<DateTime, double> DataPoints { get; set; }
        protected IRegression FitRegression { get; set; }
        protected const double alpha = 0.98;

        public TimeSeriesChart(Dictionary<DateTime, double> dataPoints)
        {
            //Set up the datapoints
            this.DataPoints = dataPoints;
            this.Click += OnChartClick;           

        }

        public TimeSeriesChart(Dictionary<DateTime, double> dataPoints, IRegression fitRegression) : this(dataPoints)
        {
            //Call the constructor that doesn't add fit series then tack on the fit
            this.FitRegression = fitRegression;
        }

        protected Series GenerateTimeSeries(Dictionary<DateTime, double> timeSeriesDataPoints)
        {
            Series timeSeries = new Series();
            timeSeries.XValueType = ChartValueType.Date;
            timeSeries.Name = "TimeSeries";
            timeSeries.ChartType = SeriesChartType.Line;
            timeSeries.MarkerStyle = MarkerStyle.Circle;
            foreach (KeyValuePair<DateTime, double> datapoint in timeSeriesDataPoints)
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
        protected virtual BoxPlotSeries GenerateBoxPlotSeries(IRegression fitRegression, double xValue)
        {
            throw new NotImplementedException();
        }
    }
}
