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
        private TimeSeriesChart timeSeries1 { get; set; }
        private TimeSeriesChart timeSeries2 { get; set; }
        private TimeSeriesChart timeSeries3 { get; set; }
        public Series TimeSeries { get; set; }
        public Series PdfSeries { get; set; }
        public Series FitSeries { get; set; }
        
        public FitSelectorForm(List<OptimizationResult> fittedResults)
        {
            InitializeComponent();
            
            //Load the fit options off the parameter
            this.SelectedResults = SelectResults(fittedResults);
            TimeSeriesChart.default_chartHeight = flowLayoutPanel_Charts.Height;        //Overwrite the chart's default size
            TimeSeriesChart.default_chartWidth = flowLayoutPanel_Charts.Width;
            PopulateFitOptions();
            this.flowLayoutPanel_Charts.Controls.Add(timeSeries1);
            timeSeries1.Show();
            
            
            //The TimeSeries should always be the same -- the fit series is what changes when the selected listbox_FitOption changes
            //Does storing the bucketed sums against each result cost time/memory? -- I don't THINK so because it should only be a reference, but it depends how it's created
            //The bucketed sums could be split out into their own parameter...
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
            //Replace the timeSeries# chart when the fit option changes

            //Do I have to remove/add or will it just update?
            flowLayoutPanel_Charts.Controls.Remove(timeSeries1);
            timeSeries1 = new TimeSeriesChart(SelectedResults[listBox_FitOptions.SelectedIndex].BucketedSums);
            flowLayoutPanel_Charts.Controls.Add(timeSeries1);
        }

    }
}
