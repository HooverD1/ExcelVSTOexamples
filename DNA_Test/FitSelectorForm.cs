using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
        private FlowLayoutPanel flowLayoutPanel_Check1 = new FlowLayoutPanel();
        private FlowLayoutPanel flowLayoutPanel_Check2 = new FlowLayoutPanel();
        private FlowLayoutPanel flowLayoutPanel_Check3 = new FlowLayoutPanel();
        private CheckBox checkBox_timeSeries1 { get; set; } = new CheckBox();
        private CheckBox checkBox_timeSeries2 { get; set; } = new CheckBox();
        private CheckBox checkBox_timeSeries3 { get; set; } = new CheckBox();
        public Series TimeSeries { get; set; }
        public Series PdfSeries { get; set; }
        public Series FitSeries { get; set; }
        private int DisplayCount { get; set; }
        
        public FitSelectorForm(List<OptimizationResult> fittedResults)
        {
            InitializeComponent();

            this.button_SelectFit.Enabled = false;  //Disable until the user has selected a fit
            listBox_fitOptions1.SelectedIndexChanged += listBox_FitOptions1_SelectedIndexChanged;
            listBox_fitOptions2.SelectedIndexChanged += listBox_FitOptions2_SelectedIndexChanged;
            listBox_fitOptions3.SelectedIndexChanged += listBox_FitOptions3_SelectedIndexChanged;
            checkBox_timeSeries1.CheckedChanged += checkBox_timeSeries1_Checked_Changed;
            checkBox_timeSeries2.CheckedChanged += checkBox_timeSeries2_Checked_Changed;
            checkBox_timeSeries3.CheckedChanged += checkBox_timeSeries3_Checked_Changed;

            this.comboBox_DisplayCount.Items.Add("Display 1 Chart");
            this.comboBox_DisplayCount.Items.Add("Display 2 Charts");
            this.comboBox_DisplayCount.Items.Add("Display 3 Charts");
            this.SelectedResults = SelectResults(fittedResults);
            comboBox_DisplayCount.SelectedIndex = 2;
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

        private ListBox PopulateFitOptions(ListBox fitOptions)
        {
            fitOptions.Items.Clear();
            foreach (OptimizationResult result in SelectedResults)
                fitOptions.Items.Add(result.ToString());
            if(fitOptions.Items.Count > 0)
                fitOptions.SelectedIndex = 0;
            return fitOptions;
        }

        private void button_SelectFit_Click(object sender, EventArgs e)
        {
            OptimizationResult selectedResult;
            if (checkBox_timeSeries1.Checked)
            {
                int selInd = listBox_fitOptions1.SelectedIndex;
                selectedResult = SelectedResults[selInd];
            }
            else if (checkBox_timeSeries2.Checked)
            {
                int selInd = listBox_fitOptions2.SelectedIndex;
                selectedResult = SelectedResults[selInd];
            }
            else if (checkBox_timeSeries3.Checked)
            {
                int selInd = listBox_fitOptions3.SelectedIndex;
                selectedResult = SelectedResults[selInd];
            }
            else
            {
                throw new Exception("No series selected");
            }

            //WHAT TO DO WITH THE RESULT?
            MessageBox.Show($"Selected {selectedResult.ToString()}");
        }

        private void listBox_FitOptions1_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel_Charts.Controls.Remove(timeSeries1);
            flowLayoutPanel_Charts.Controls.Remove(timeSeries2);
            flowLayoutPanel_Charts.Controls.Remove(timeSeries3);
            timeSeries1 = new TimeSeriesChart(SelectedResults[listBox_fitOptions1.SelectedIndex].BucketedSums, SelectedResults[listBox_fitOptions1.SelectedIndex].RegressionUnderTest);
            flowLayoutPanel_Charts.Controls.Add(timeSeries1);
            if(DisplayCount >= 2)
                flowLayoutPanel_Charts.Controls.Add(timeSeries2);
            if(DisplayCount == 3)
                flowLayoutPanel_Charts.Controls.Add(timeSeries3);
        }

        private void listBox_FitOptions2_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel_Charts.Controls.Remove(timeSeries1);
            flowLayoutPanel_Charts.Controls.Remove(timeSeries2);
            flowLayoutPanel_Charts.Controls.Remove(timeSeries3);
            timeSeries2 = new TimeSeriesChart(SelectedResults[listBox_fitOptions2.SelectedIndex].BucketedSums, SelectedResults[listBox_fitOptions2.SelectedIndex].RegressionUnderTest);
            flowLayoutPanel_Charts.Controls.Add(timeSeries1);
            if (DisplayCount >= 2)
                flowLayoutPanel_Charts.Controls.Add(timeSeries2);
            if (DisplayCount == 3)
                flowLayoutPanel_Charts.Controls.Add(timeSeries3);
        }

        private void listBox_FitOptions3_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel_Charts.Controls.Remove(timeSeries1);
            flowLayoutPanel_Charts.Controls.Remove(timeSeries2);
            flowLayoutPanel_Charts.Controls.Remove(timeSeries3);
            timeSeries3 = new TimeSeriesChart(SelectedResults[listBox_fitOptions3.SelectedIndex].BucketedSums, SelectedResults[listBox_fitOptions3.SelectedIndex].RegressionUnderTest);
            flowLayoutPanel_Charts.Controls.Add(timeSeries1);
            if (DisplayCount >= 2)
                flowLayoutPanel_Charts.Controls.Add(timeSeries2);
            if (DisplayCount == 3)
                flowLayoutPanel_Charts.Controls.Add(timeSeries3);
        }

        private void checkBox_timeSeries1_Checked_Changed(object sender, EventArgs e)
        {
            if (checkBox_timeSeries1.Checked == true)
            {
                this.button_SelectFit.Enabled = true;
                checkBox_timeSeries2.Checked = false;
                checkBox_timeSeries3.Checked = false;
            }
            else
            {
                if (!(checkBox_timeSeries1.Checked || checkBox_timeSeries2.Checked || checkBox_timeSeries3.Checked))
                {   //If none are checked, disable the selection button
                    this.button_SelectFit.Enabled = false;
                }
            }
        }

        private void checkBox_timeSeries2_Checked_Changed(object sender, EventArgs e)
        {
            if (checkBox_timeSeries2.Checked == true)
            {
                this.button_SelectFit.Enabled = true;
                checkBox_timeSeries1.Checked = false;
                checkBox_timeSeries3.Checked = false;
            }
            else
            {
                if (!(checkBox_timeSeries1.Checked || checkBox_timeSeries2.Checked || checkBox_timeSeries3.Checked))
                {   //If none are checked, disable the selection button
                    this.button_SelectFit.Enabled = false;
                }
            }
        }

        private void checkBox_timeSeries3_Checked_Changed(object sender, EventArgs e)
        {
            if (checkBox_timeSeries3.Checked == true)
            {
                this.button_SelectFit.Enabled = true;
                checkBox_timeSeries1.Checked = false;
                checkBox_timeSeries2.Checked = false;
            }
            else
            {
                if (!(checkBox_timeSeries1.Checked || checkBox_timeSeries2.Checked || checkBox_timeSeries3.Checked))
                {   //If none are checked, disable the selection button
                    this.button_SelectFit.Enabled = false;
                }
            }
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
                switch (comboBox_DisplayCount.SelectedIndex + 1)
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
            this.checkBox_timeSeries1.Checked = true;
            this.checkBox_timeSeries2.Checked = false;
            this.checkBox_timeSeries3.Checked = false;

            this.flowLayoutPanel_Check1.Controls.Clear();
            this.flowLayoutPanel_Check2.Controls.Clear();
            this.flowLayoutPanel_Check3.Controls.Clear();
            //Load the chart and fit options
            this.flowLayoutPanel_Checkboxes.Controls.Clear();
            this.flowLayoutPanel_Charts.Controls.Clear();
            this.flowLayoutPanel_Options.Controls.Clear();
            TimeSeriesChart.default_chartHeight = flowLayoutPanel_Charts.Height;        //Overwrite the chart's default size -- allows you to not have to reset every time a different fit option is selected
            TimeSeriesChart.default_chartWidth = flowLayoutPanel_Charts.Width;
            PopulateFitOptions(listBox_fitOptions1);
            listBox_fitOptions1.Show();

            this.flowLayoutPanel_Options.Controls.Add(listBox_fitOptions1);
            listBox_fitOptions1.Height = this.flowLayoutPanel_Options.Height;
            listBox_fitOptions1.Width = this.flowLayoutPanel_Options.Width;
            this.flowLayoutPanel_Charts.Controls.Add(timeSeries1);
            timeSeries1.Show();
        }
        private void LoadTwoCharts()
        {
            this.checkBox_timeSeries1.Checked = false;
            this.checkBox_timeSeries2.Checked = false;
            this.checkBox_timeSeries3.Checked = false;

            this.flowLayoutPanel_Check1.Controls.Clear();
            this.flowLayoutPanel_Check2.Controls.Clear();
            this.flowLayoutPanel_Check3.Controls.Clear();
            this.flowLayoutPanel_Checkboxes.Controls.Clear();
            this.flowLayoutPanel_Charts.Controls.Clear();
            this.flowLayoutPanel_Options.Controls.Clear();
            TimeSeriesChart.default_chartHeight = flowLayoutPanel_Charts.Height / 2 - 2;        //Overwrite the chart's default size -- allows you to not have to reset every time a different fit option is selected
            TimeSeriesChart.default_chartWidth = flowLayoutPanel_Charts.Width;
            listBox_fitOptions1 = PopulateFitOptions(listBox_fitOptions1);
            listBox_fitOptions2 = PopulateFitOptions(listBox_fitOptions2);
            listBox_fitOptions1.Show();
            listBox_fitOptions2.Show();

            this.flowLayoutPanel_Options.Controls.Add(listBox_fitOptions1);
            listBox_fitOptions1.Height = this.flowLayoutPanel_Options.Height / 2 - 2;
            listBox_fitOptions1.Width = this.flowLayoutPanel_Options.Width;

            this.flowLayoutPanel_Options.Controls.Add(listBox_fitOptions2);
            listBox_fitOptions2.Height = this.flowLayoutPanel_Options.Height / 2 - 2;
            listBox_fitOptions2.Width = this.flowLayoutPanel_Options.Width;

            this.flowLayoutPanel_Charts.Controls.Add(timeSeries1);
            this.flowLayoutPanel_Charts.Controls.Add(timeSeries2);
            timeSeries1.Show();
            timeSeries2.Show();

            this.flowLayoutPanel_Check1.Height = timeSeries1.Height;
            this.flowLayoutPanel_Check2.Height = timeSeries2.Height;
            flowLayoutPanel_Checkboxes.Controls.Add(this.flowLayoutPanel_Check1);
            flowLayoutPanel_Checkboxes.Controls.Add(this.flowLayoutPanel_Check2);

            this.flowLayoutPanel_Check1.Controls.Add(this.checkBox_timeSeries1);
            this.flowLayoutPanel_Check2.Controls.Add(this.checkBox_timeSeries2);
        }
        private void LoadThreeCharts()
        {
            this.checkBox_timeSeries1.Checked = false;
            this.checkBox_timeSeries2.Checked = false;
            this.checkBox_timeSeries3.Checked = false;

            this.flowLayoutPanel_Check1.Controls.Clear();
            this.flowLayoutPanel_Check2.Controls.Clear();
            this.flowLayoutPanel_Check3.Controls.Clear();
            this.flowLayoutPanel_Checkboxes.Controls.Clear();

            this.flowLayoutPanel_Charts.Controls.Clear();
            this.flowLayoutPanel_Options.Controls.Clear();
            TimeSeriesChart.default_chartHeight = flowLayoutPanel_Charts.Height / 3 - 2;        //Overwrite the chart's default size -- allows you to not have to reset every time a different fit option is selected
            TimeSeriesChart.default_chartWidth = flowLayoutPanel_Charts.Width;

            listBox_fitOptions1 = PopulateFitOptions(listBox_fitOptions1);
            listBox_fitOptions2 = PopulateFitOptions(listBox_fitOptions2);
            listBox_fitOptions3 = PopulateFitOptions(listBox_fitOptions3);
            listBox_fitOptions1.Show();
            listBox_fitOptions2.Show();
            listBox_fitOptions3.Show();

            this.flowLayoutPanel_Charts.Controls.Add(timeSeries1);
            timeSeries1.Show();
            this.flowLayoutPanel_Charts.Controls.Add(timeSeries2);
            timeSeries2.Show();
            this.flowLayoutPanel_Charts.Controls.Add(timeSeries3);
            timeSeries3.Show();

            this.flowLayoutPanel_Options.Controls.Add(listBox_fitOptions1);
            listBox_fitOptions1.Height = this.flowLayoutPanel_Options.Height / 3 - 2;
            listBox_fitOptions1.Width = this.flowLayoutPanel_Options.Width;

            this.flowLayoutPanel_Options.Controls.Add(listBox_fitOptions2);
            listBox_fitOptions2.Height = this.flowLayoutPanel_Options.Height / 3 - 2;
            listBox_fitOptions2.Width = this.flowLayoutPanel_Options.Width;

            this.flowLayoutPanel_Options.Controls.Add(listBox_fitOptions3);
            listBox_fitOptions3.Height = this.flowLayoutPanel_Options.Height / 3 - 2;
            listBox_fitOptions3.Width = this.flowLayoutPanel_Options.Width;

            //flowLayoutPanel_Checkboxes.Padding = new Padding(0, 0, 0, timeSeries1.Height);
            this.flowLayoutPanel_Check1.Height = timeSeries1.Height;
            this.flowLayoutPanel_Check2.Height = timeSeries2.Height;
            this.flowLayoutPanel_Check3.Height = timeSeries3.Height;

            flowLayoutPanel_Checkboxes.Controls.Add(this.flowLayoutPanel_Check1);
            flowLayoutPanel_Checkboxes.Controls.Add(this.flowLayoutPanel_Check2);
            flowLayoutPanel_Checkboxes.Controls.Add(this.flowLayoutPanel_Check3);

            this.flowLayoutPanel_Check1.Controls.Add(this.checkBox_timeSeries1);
            this.flowLayoutPanel_Check2.Controls.Add(this.checkBox_timeSeries2);
            this.flowLayoutPanel_Check3.Controls.Add(this.checkBox_timeSeries3);
        }

    }
}
