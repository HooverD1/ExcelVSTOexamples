using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DNA_Test
{
    public class DataChart : Chart
    {
        //Not all DataCharts will have a FitSeries & FitRegression, but they all should be capable of having one.
        public Series FitSeries { get; set; }  //The regression line
        public IRegression FitRegression { get; set; }
        public Series ErrorSeries_CI_Upper { get; set; }  //The regression line + error band
        public Series ErrorSeries_CI_Lower { get; set; }  //The regression line - error band
        public BoxPlotSeries BoxPlot_Series { get; set; }
        public Series PDF_Series { get; set; }      //Pop-up on-click PDF

        protected Title Description { get; set; }
        protected Point MouseCoords { get; set; }       //for identifying controls being clicked
        public ChartArea chartArea { get; set; }        //The x-Axis -- potentially overwritten for non-uniform
        public static int default_chartHeight = 100;    //Overwritable defaults
        public static int default_chartWidth = 100;

        public DataChart()      //Chart with no regression
        {
            //Setup events
            this.MouseMove += OnMouseMoved;
            //Setup elements
            SetupChartArea();

            this.BorderlineDashStyle = ChartDashStyle.Solid;
            this.BorderlineColor = System.Drawing.Color.Black;
            this.BorderlineWidth = 2;
        }

        public DataChart(IRegression fitRegression) : this()    //Chart with a regression
        {
            //Call the constructor that doesn't add fit series then tack on the fit
            this.FitRegression = fitRegression;
        }

        protected virtual void ScaleAxesToY() { throw new NotImplementedException(); }
        protected virtual void SetupDescription() { throw new NotImplementedException(); }
        protected virtual void SetupChartArea() { throw new NotImplementedException(); }
        protected virtual Series GenerateErrorSeries_Lower() { throw new NotImplementedException(); }
        protected virtual Series GenerateErrorSeries_Upper() { throw new NotImplementedException(); }
        protected virtual void SetupXAxisGridlines() { throw new NotImplementedException(); }

        protected virtual void FixSeriesOrder()
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

        public virtual void PrintChartImageToFile(string path)
        {
            //Place the Description label
            if (this.Description != null && this.Description is Title)
            {
                this.Description.Visible = true;
                //Description is null?
                //Test if the path is valid
                //Print image
                this.SaveImage($"{path}/test_save_file.png", ChartImageFormat.Png);
                //Remove the Description label
                this.Description.Visible = false;
            }
            else
            {
                throw new Exception("Description is null");
            }
        }

        protected void OnMouseMoved(object sender, MouseEventArgs e)
        {
            MouseCoords = new Point(e.X, e.Y);
        }
        
        protected virtual void OnChartClick(object sender, EventArgs e)
        {
            HitTestResult htr = this.HitTest(MouseCoords.X, MouseCoords.Y);
            //Handle what happens if a generic element is clicked
        }

        protected virtual Series GenerateFitSeries()     //Feed this the regression used to fit the time series
        {
            if (FitRegression == null)
                throw new Exception("FitRegression is null");
            if(chartArea == null)
                throw new Exception("chartArea is null");
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
                fitSeries.Points.AddXY(xVal, FitRegression.GetValue(xVal));
            }
            fitSeries.BorderWidth = 3;
            fitSeries.Color = System.Drawing.Color.Red;
            return fitSeries;
        }

        public double Get_X_Coords_Per_Pixel()
        {
            if (chartArea == null)
                throw new Exception("ChartArea is null");
            if (chartArea.AxisX == null)
                throw new Exception("Axis is null");
            //Need to account for the small gap between the datapoint and the end?
            float scalar = this.chartArea.InnerPlotPosition.Width / 100;
            double chartAreaWidth = scalar * this.Width;
            if (chartAreaWidth <= 0)
                throw new Exception("Scaled chart width must be positive");
            double minX = this.chartArea.AxisX.Minimum;
            double maxX = this.chartArea.AxisX.Maximum;
            double xRange = maxX - minX;
            return xRange / chartAreaWidth;
        }
        public double Get_Y_Coords_Per_Pixel()
        {
            if (chartArea == null)
                throw new Exception("ChartArea is null");
            if (chartArea.AxisY == null)
                throw new Exception("Axis is null");
            //Need to account for the small gap between the datapoint and the end?
            float scalar = (this.chartArea.Position.Height / 100) * (this.chartArea.InnerPlotPosition.Height / 100);
            double chartAreaHeight = scalar * this.Height;
            if (chartAreaHeight <= 0)
                throw new Exception("Scaled chart height must be positive");
            double minY = this.chartArea.AxisY.Minimum;
            double maxY = this.chartArea.AxisY.Maximum;
            double yRange = maxY - minY;
            return yRange / chartAreaHeight;
        }
    }
}
