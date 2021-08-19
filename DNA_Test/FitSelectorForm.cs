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
        private ListBox listBox_fitOptions1 { get; set; } = new ListBox();
        private ListBox listBox_fitOptions2 { get; set; } = new ListBox();
        private ListBox listBox_fitOptions3 { get; set; } = new ListBox();
        public Series TimeSeries { get; set; }
        public Series PdfSeries { get; set; }
        public Series FitSeries { get; set; }
        private int DisplayCount { get; set; }
        
        public FitSelectorForm(List<OptimizationResult> fittedResults)
        {
            InitializeComponent();
            this.comboBox_DisplayCount.Enabled = true;
            this.comboBox_DisplayCount.Items.Add("Display 1 Chart");
            this.comboBox_DisplayCount.Items.Add("Display 2 Charts");
            this.comboBox_DisplayCount.Items.Add("Display 3 Charts");
            //Load the fit options off the parameter
            this.SelectedResults = SelectResults(fittedResults);
            listBox_fitOptions1.SelectedIndexChanged += listBox_FitOptions1_SelectedIndexChanged;
            listBox_fitOptions2.SelectedIndexChanged += listBox_FitOptions2_SelectedIndexChanged;
            listBox_fitOptions3.SelectedIndexChanged += listBox_FitOptions3_SelectedIndexChanged;
            comboBox_DisplayCount.SelectedIndex = 0;
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

        private void PopulateFitOptions(ListBox fitOptions)
        {
            fitOptions.Items.Clear();
            foreach (OptimizationResult result in SelectedResults)
                fitOptions.Items.Add(result.ToString());
            if(fitOptions.Items.Count > 0)
                fitOptions.SelectedIndex = 0;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_SelectFit_Click(object sender, EventArgs e)
        {
            //What happens here?
            
        }

        private void listBox_FitOptions1_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel_Charts.Controls.Remove(timeSeries1);
            timeSeries1 = new TimeSeriesChart(SelectedResults[listBox_fitOptions1.SelectedIndex].BucketedSums, SelectedResults[listBox_fitOptions1.SelectedIndex].RegressionUnderTest);
            flowLayoutPanel_Charts.Controls.Add(timeSeries1);
        }

        private void listBox_FitOptions2_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel_Charts.Controls.Remove(timeSeries2);
            timeSeries2 = new TimeSeriesChart(SelectedResults[listBox_fitOptions2.SelectedIndex].BucketedSums, SelectedResults[listBox_fitOptions2.SelectedIndex].RegressionUnderTest);
            flowLayoutPanel_Charts.Controls.Add(timeSeries2);
        }

        private void listBox_FitOptions3_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel_Charts.Controls.Remove(timeSeries3);
            timeSeries3 = new TimeSeriesChart(SelectedResults[listBox_fitOptions3.SelectedIndex].BucketedSums, SelectedResults[listBox_fitOptions3.SelectedIndex].RegressionUnderTest);
            flowLayoutPanel_Charts.Controls.Add(timeSeries3);
        }

        private void comboBox_DisplayCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Add or remove TimeSeriesCharts from the flowlayout
            if(this.DisplayCount == comboBox_DisplayCount.SelectedIndex + 1)
            {
                return; //Nothing changed
            }
            else
            {
                this.DisplayCount = comboBox_DisplayCount.SelectedIndex + 1;
                switch (DisplayCount)
                {
                    case 1:
                        LoadOneChart();
                        break;
                    case 2:
                        LoadTwoCharts();
                        break;
                    case 3:
                        LoadThreeCharts();
                        break;
                    default:
                        throw new Exception("Unexpected amount of charts");
                }
            }
        }

        private void LoadOneChart()
        {
            TimeSeriesChart.default_chartHeight = flowLayoutPanel_Charts.Height;        //Overwrite the chart's default size -- allows you to not have to reset every time a different fit option is selected
            TimeSeriesChart.default_chartWidth = flowLayoutPanel_Charts.Width;
            PopulateFitOptions(listBox_fitOptions1);
            this.flowLayoutPanel_Options.Controls.Add(listBox_fitOptions1);
            listBox_fitOptions1.Height = this.flowLayoutPanel_Options.Height;
            listBox_fitOptions1.Width = this.flowLayoutPanel_Options.Width;
            this.flowLayoutPanel_Charts.Controls.Add(timeSeries1);
            timeSeries1.Show();
        }
        private void LoadTwoCharts()
        {
            this.flowLayoutPanel_Charts.Controls.Clear();
            this.flowLayoutPanel_Options.Controls.Clear();
            TimeSeriesChart.default_chartHeight = flowLayoutPanel_Charts.Height / 2;        //Overwrite the chart's default size -- allows you to not have to reset every time a different fit option is selected
            TimeSeriesChart.default_chartWidth = flowLayoutPanel_Charts.Width / 2;
            PopulateFitOptions(listBox_fitOptions1);
            PopulateFitOptions(listBox_fitOptions2);

            this.flowLayoutPanel_Options.Controls.Add(listBox_fitOptions1);
            listBox_fitOptions1.Height = this.flowLayoutPanel_Options.Height / 2;
            listBox_fitOptions1.Width = this.flowLayoutPanel_Options.Width;

            this.flowLayoutPanel_Options.Controls.Add(listBox_fitOptions2);
            listBox_fitOptions2.Height = this.flowLayoutPanel_Options.Height / 2;
            listBox_fitOptions2.Width = this.flowLayoutPanel_Options.Width;

            this.flowLayoutPanel_Charts.Controls.Add(timeSeries1);
            this.flowLayoutPanel_Charts.Controls.Add(timeSeries2);
            timeSeries1.Show();
            timeSeries2.Show();
        }
        private void LoadThreeCharts()
        {
            TimeSeriesChart.default_chartHeight = flowLayoutPanel_Charts.Height / 3;        //Overwrite the chart's default size -- allows you to not have to reset every time a different fit option is selected
            TimeSeriesChart.default_chartWidth = flowLayoutPanel_Charts.Width / 3;
            PopulateFitOptions(listBox_fitOptions1);
            PopulateFitOptions(listBox_fitOptions2);
            PopulateFitOptions(listBox_fitOptions3);

            this.flowLayoutPanel_Options.Controls.Add(listBox_fitOptions1);
            listBox_fitOptions1.Height = this.flowLayoutPanel_Options.Height / 3;
            listBox_fitOptions1.Width = this.flowLayoutPanel_Options.Width;

            this.flowLayoutPanel_Options.Controls.Add(listBox_fitOptions2);
            listBox_fitOptions2.Height = this.flowLayoutPanel_Options.Height / 3;
            listBox_fitOptions2.Width = this.flowLayoutPanel_Options.Width;

            this.flowLayoutPanel_Options.Controls.Add(listBox_fitOptions3);
            listBox_fitOptions3.Height = this.flowLayoutPanel_Options.Height / 3;
            listBox_fitOptions3.Width = this.flowLayoutPanel_Options.Width;

            this.flowLayoutPanel_Charts.Controls.Add(timeSeries1);
            this.flowLayoutPanel_Charts.Controls.Add(timeSeries2);
            this.flowLayoutPanel_Charts.Controls.Add(timeSeries3);
            timeSeries1.Show();
            timeSeries2.Show();
            timeSeries3.Show();
        }
    }
}
