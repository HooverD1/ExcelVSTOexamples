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
    public partial class FitSelectorForm : Form
    {
        public OptimizationResult[] SelectedResults { get; set; }
        public Series TimeSeries { get; set; }
        public Series PdfSeries { get; set; }
        public Series FitSeries { get; set; }
        
        public FitSelectorForm(List<OptimizationResult> fittedResults)
        {
            InitializeComponent();
            //Load the fit options off the parameter
            this.Chart_FitDisplay.Series.Clear();
            this.SelectedResults = SelectResults(fittedResults);
            PopulateFitOptions();
            
            //The TimeSeries should always be the same -- the fit series is what changes when the selected listbox_FitOption changes
            //Does storing the bucketed sums against each result cost time/memory? -- I don't THINK so because it should only be a reference, but it depends how it's created
            //The bucketed sums could be split out into their own parameter...
            TimeSeries = BuildTimeSeries(SelectedResults.First().BucketedSums);
            this.Chart_FitDisplay.Series.Add(TimeSeries);
            this.Chart_FitDisplay.Series.Add(FitSeries);
        }

        private OptimizationResult[] SelectResults(IEnumerable<OptimizationResult> fittedResults)
        {
            OptimizationResult[] enumeratedResults = fittedResults.OrderByDescending(x => x.Score).ToArray();
            IEnumerable<OptimizationResult> restrictedResults;
            if (enumeratedResults.Length > 5)
            {
                double fifthScore = enumeratedResults[4].Score;
                if (fifthScore < 0.8)
                {
                    //Show top five if their fit falls off quickly
                    restrictedResults = from OptimizationResult or in enumeratedResults where or.Score > fifthScore select or;
                }
                else
                {
                    //Show everything above 0.8
                    restrictedResults = from OptimizationResult or in enumeratedResults where or.Score >= 0.8 select or;
                }
            }
            else
            {
                //Show everything if there are less than 5
                restrictedResults = enumeratedResults;
            }
            return restrictedResults.ToArray();
            
        }

        private void PopulateFitOptions()
        {
            foreach (OptimizationResult result in SelectedResults)
            {
                this.listBox_FitOptions.Items.Add(result.ToString());
            }
            if(listBox_FitOptions.Items.Count > 0)
                listBox_FitOptions.SelectedIndex = 0;
        }

        private Series BuildTimeSeries(Dictionary<DateTime, double> bucketedSums)
        {
            Series timeSeries = new Series();
            timeSeries.ChartType = SeriesChartType.Point;
            foreach(KeyValuePair<DateTime, double> period in bucketedSums)
            {
                timeSeries.Points.AddXY(period.Key.ToOADate(), period.Value);
            }
            return timeSeries;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_SelectFit_Click(object sender, EventArgs e)
        {
            //What happens here?
            
        }

        private void listBox_FitOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            FitSeries = BuildFitSeries(SelectedResults[listBox_FitOptions.SelectedIndex]);
        }

        private Series BuildFitSeries(OptimizationResult selectedResult)
        {
            Series fitSeries = new Series();
            fitSeries.ChartType = SeriesChartType.Line;
            foreach(DateTime dt in selectedResult.Schedule.GetMidpoints())
            {
                //Requires Regression implementation
                //fitSeries.Points.AddXY(dt.ToOADate(), selectedResult.Regression.GetValue(dt));
            }
            return fitSeries;
        }
    }
}
