using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace DNA_Test
{
    public class PDF_Popup
    {
        private Series PopupSeries { get; set; } = null;
        private double xValue { get; set; }
        private PDF_Builder pdf_Builder { get; set; }
        private double pdf_Height { get; set; }
        private const double heightScalar = 0.1;        //Percent of the width of the parent chart that the PDF should take up
        private double yMin { get; set; }
        private double yMax { get; set; }

        public PDF_Popup(double xValue, double xWidth, double yMin, double yMax)
        {
            /*  Need to feed this the x-axis context of the chart it will appear in:
             *  xValue: value of the point on the x-axis that the chart appears at
             *  pdf_Builder: helper class for providing PDF data
             *  pdf_Height: the height of the PDF in pixels scaled to the parent
             */
            this.pdf_Height = xWidth * heightScalar;
            this.yMin = yMin;
            this.yMax = yMax;
        }

        public Series GetSeries()
        {
            if(PopupSeries == null)
            {
                this.PopupSeries = BuildSeries();
            }
            return this.PopupSeries;
        }
        private Series BuildSeries()
        {
            Series popSeries = new Series();
            popSeries.ChartType = SeriesChartType.Spline;
            int steps = Convert.ToInt32(yMax - yMin);
            for(int x = Convert.ToInt32(yMin); x < Convert.ToInt32(yMin) + steps; x++)
                popSeries.Points.AddXY(pdf_Builder.GetHeightAtX(x)*heightScalar, x);
            return popSeries;
        }        
    }
}
