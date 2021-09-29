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
        protected Title Description { get; set; }
        protected Point MouseCoords { get; set; }       //for identifying controls being clicked
        public ChartArea chartArea { get; set; }        //The x-Axis -- potentially overwritten for non-uniform
        public static int default_chartHeight = 100;    //Overwritable defaults
        public static int default_chartWidth = 100;

        public DataChart()
        {
            chartArea = new ChartArea();
            this.ChartAreas.Add(chartArea);

            this.Height = default_chartHeight;
            this.Width = default_chartWidth;
            chartArea.Position = new ElementPosition(0, 0, 100, 100);
            chartArea.InnerPlotPosition = new ElementPosition(10, 5, 88, 88);
        }

        public virtual void PrintChartImageToFile(string path)
        {
            //Setup events
            this.MouseMove += OnMouseMoved;
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
