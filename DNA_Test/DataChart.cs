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
        public Series FitSeries { get; set; }  //The regression line
        public Series ErrorSeries_CI_Upper { get; set; }  //The regression line + error band
        public Series ErrorSeries_CI_Lower { get; set; }  //The regression line - error band
        public BoxPlotSeries BoxPlot_Series { get; set; }
        public Series PDF_Series { get; set; }      //Pop-up on-click PDF

        protected Title Description { get; set; }
        protected Point MouseCoords { get; set; }       //for identifying controls being clicked
        public ChartArea chartArea { get; set; }        //The x-Axis -- potentially overwritten for non-uniform
        public static int default_chartHeight = 100;    //Overwritable defaults
        public static int default_chartWidth = 100;

        public DataChart()
        {
            //Setup events
            this.MouseMove += OnMouseMoved;
            //Setup elements
            SetupChartArea();

            this.BorderlineDashStyle = ChartDashStyle.Solid;
            this.BorderlineColor = System.Drawing.Color.Black;
            this.BorderlineWidth = 2;
        }

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
            this.Description.Visible = true;
            //Test if the path is valid
            //Print image
            this.SaveImage($"{path}/test_save_file.png", ChartImageFormat.Png);
            //Remove the Description label
            this.Description.Visible = false;
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

    }
}
