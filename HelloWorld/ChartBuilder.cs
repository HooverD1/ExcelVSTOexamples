using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Tools = Microsoft.Office.Tools.Excel;
using System.Windows.Forms;
using Charting = System.Windows.Forms.DataVisualization.Charting;

namespace HelloWorld
{
    public class ChartBuilder
    {
        public enum Template
        {
            Chart1,
            Chart2,
            Chart3
        }

        private Dictionary<Template, string> TemplateDictionary = new Dictionary<Template, string>
        {
            { Template.Chart1, @"C:\Users\grins\Documents\Custom Office Templates\Chart1.crtx"},
            { Template.Chart2, @"C:\Users\grins\Documents\Custom Office Templates\Chart2.crtx"},
            { Template.Chart3, @"C:\Users\grins\Documents\Custom Office Templates\Chart3.crtx"}
        };

        private Random random = new Random();
        private double[] Data { get; set; }

        private double[] GenerateTestData(Random random, int n)
        {
            double[] data = new double[n];
            for(int i = 0; i < n; i++)
            {
                data[i] = random.NextDouble();
            }
            return data;
        }

        public void AddChart(Tools.Worksheet worksheet, Excel.Range range, Template template)
        {
            
            var charts = worksheet.Application.ActiveWorkbook.Charts;
            Tools.Chart chart;
            if (worksheet.Controls.Contains("chart"))
            {
                chart = (Tools.Chart)worksheet.Controls["chart"];
                Excel.SeriesCollection sc = (Excel.SeriesCollection)chart.SeriesCollection();
                while(sc.Count > 0)
                {
                    sc.Item(1).Delete();
                }
            }
            else
                chart = worksheet.Controls.AddChart(range, "chart");
                
            //chart.ChartType = Excel.XlChartType.xlLine;            
            Excel.SeriesCollection seriesCollection = (Excel.SeriesCollection)chart.SeriesCollection();
            var series = seriesCollection.NewSeries();

            series.Values = GenerateTestData(this.random, 100);
            //series.XValues = new string[] { "A", "B", "C", "D" };
            series.Name = "Series Name";

            chart.ApplyChartTemplate(TemplateDictionary[template]);

        }
    }
}
