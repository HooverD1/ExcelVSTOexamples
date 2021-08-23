using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DNA_Test
{
    public partial class TestForm1 : Form
    {
        public TestForm1()
        {
            
            InitializeComponent();
            chart_test.Series.Add(CreateBoxPlot());
        }

        public static Series CreateBoxPlot()
        {
            Series testseries = new Series();
            testseries.ChartType = SeriesChartType.BoxPlot;
            for(int i=1; i<20; i++)
            {
                BoxPlot boxPlot = new BoxPlot(i+1, i+2, i+3, i+4, i+5);
                testseries.Points.Add(boxPlot.GetBoxPlot(i));
            }
            return testseries;
        }
    }
}
