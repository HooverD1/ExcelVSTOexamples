using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using Accord.Statistics.Distributions;

namespace DNA_Test
{
    public class PDF_Popup
    {
        private int orientation { get; set; } = -1;     // -1 for a leftward pdf, 1 for a rightward pdf
        private double xValue { get; set; }
        private double pdf_Height { get; set; }
        private const double heightScalar = 0.15;        //Percent of the width of the parent chart that the PDF should take up
        private double yMin { get; set; }
        private double yMax { get; set; }
        private double xWidth { get; set; }
        private IUnivariateDistribution PDF { get; set; }

        public PDF_Popup(double xValue, double xWidth, double yMin, double yMax)
        {
            // xValue of the selected point
            // xWidth width of the x-valued axis (not pixels)
            // yMin of the y axis
            // yMax of the y axis
            /*  Need to feed this the x-axis context of the chart it will appear in:
             *  xValue: value of the point on the x-axis that the chart appears at
             *  pdf_Builder: helper class for providing PDF data
             *  pdf_Height: the height of the PDF in pixels scaled to the parent
             */
            this.xWidth = xWidth;
            this.xValue = xValue;
            this.pdf_Height = xWidth * heightScalar;
            this.yMin = yMin;
            this.yMax = yMax;
            
        }

        public Series GetSeries(IUnivariateDistribution pdfDistribution)
        {
            Series popUpSeries = new Series();
            popUpSeries.ChartType = SeriesChartType.Spline;
            popUpSeries.Name = "PopUpSeries";
            popUpSeries.Color = System.Drawing.Color.Black;
            popUpSeries.BorderWidth = 4;
            //popUpSeries.MarkerStyle = MarkerStyle.Circle;
            //popUpSeries.MarkerSize = 6;
            double maxHeight = this.GetMaxHeight(yMin, yMax, 0, pdfDistribution);
            double yStep = (yMax - yMin) / 100;
            for (int i=0; i<=100; i++)
            {
                //Divide the vertical distance up into percentages
                double y = yMin + yStep * i;
                double pdfVal = pdfDistribution.ProbabilityFunction(y);
                double pdfRatio = pdfVal / maxHeight;
                DataPoint dp = new DataPoint(xValue + (orientation * pdfRatio * pdf_Height), y);   
                popUpSeries.Points.Add(dp);
            }
            return popUpSeries;
        }
        
        private double TransformX(double x)
        {
            throw new NotImplementedException();
        }   
        private double TransformY(double y)
        {
            throw new NotImplementedException();
        }

        private double GetMaxHeight(double startX, double stopX, double maxProb, IUnivariateDistribution pdf)
        {
            //Try to get the max rate of change on the cdf?
            double lastStep = pdf.DistributionFunction(startX);
            double xStep = (stopX - startX) / 100;
            bool updated = false;
            double leftBound = startX;
            for(int x = 1; x <= 100; x++)
            {
                //Percentile search
                double pdfVal = pdf.ProbabilityFunction(startX + (xStep * x));
                if (maxProb < pdfVal)
                {
                    maxProb = pdfVal;
                    updated = true;
                    leftBound = startX + (xStep * x);
                }
            }
            if (updated)
                return GetMaxHeight(leftBound, leftBound + xStep, maxProb, pdf);
            else
                return pdf.ProbabilityFunction(leftBound);
        }

        public void SetLeftOrientation()
        {
            this.orientation = -1;
        }
        public void SetRightOrientation()
        {
            this.orientation = 1;
        }
    }
}
