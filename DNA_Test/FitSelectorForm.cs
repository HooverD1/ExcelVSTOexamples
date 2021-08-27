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
        private TreeView fitOptions1 { get; set; } = new TreeView();
        private TreeView fitOptions2 { get; set; } = new TreeView();
        private TreeView fitOptions3 { get; set; } = new TreeView();
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
            fitOptions1.AfterSelect += fitOptions1_SelectedIndexChanged;
            fitOptions2.AfterSelect += fitOptions2_SelectedIndexChanged;
            //fitOptions3.AfterSelect += fitOptions3_SelectedIndexChanged;

            checkBox_timeSeries1.CheckedChanged += checkBox_timeSeries1_Checked_Changed;
            checkBox_timeSeries2.CheckedChanged += checkBox_timeSeries2_Checked_Changed;
            //checkBox_timeSeries3.CheckedChanged += checkBox_timeSeries3_Checked_Changed;
            
            this.comboBox_DisplayCount.Items.Add("Display 1 Chart");
            this.comboBox_DisplayCount.Items.Add("Display 2 Charts");
            //this.comboBox_DisplayCount.Items.Add("Display 3 Charts");
            this.SelectedResults = SelectResults(fittedResults);
            comboBox_DisplayCount.SelectedIndex = 0;

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

        private TreeView PopulateFitOptions(TreeView fitOptions)
        {
            fitOptions.Nodes.Clear();
            //Need a list of distinct schedules to iterate
            IEnumerable<Scheduler.Scheduler> allSchedules = from OptimizationResult or in SelectedResults select or.Schedule;
            IEnumerable < Scheduler.Scheduler > distinctSchedules = GetDistinctSchedules(allSchedules);

            foreach (Scheduler.Scheduler schedule in distinctSchedules)
            {
                IEnumerable<TreeNode> childNodes = from OptimizationResult or in SelectedResults
                                             where or.Schedule.IsIntervalEqual(schedule)
                                             select new TreeNode(or.RegressionUnderTest.ToString());

                //Need to find all the results with the same bucketing scheme, but different regressions
                TreeNode parent_node = new TreeNode(schedule.ToIntervalString(), childNodes.ToArray());
                
                fitOptions.Nodes.Add(parent_node);
            }
            if (fitOptions.Nodes.Count > 0)
                fitOptions.SelectedNode = fitOptions.Nodes[0].Nodes[0];
            fitOptions.ExpandAll();
            return fitOptions;
        }

        private IEnumerable<Scheduler.Scheduler> GetDistinctSchedules(IEnumerable<Scheduler.Scheduler> schedules)
        {
            List<Scheduler.Scheduler> distinctSchedules = new List<Scheduler.Scheduler>();
            foreach (Scheduler.Scheduler s1 in schedules)
            {
                //Remove repeated schedules from the list
                bool isOnList = false;
                foreach (Scheduler.Scheduler s2 in distinctSchedules)
                {
                    if (s1.IsIntervalEqual(s2))
                    {
                        isOnList = true;
                        break;
                    }
                }
                if (!isOnList)
                    distinctSchedules.Add(s1);
            }
            return distinctSchedules;
        }

        private void button_SelectFit_Click(object sender, EventArgs e)
        {
            OptimizationResult selectedResult;
            if (checkBox_timeSeries1.Checked)
            {
                int selInd = fitOptions1.SelectedNode.Index;
                selectedResult = SelectedResults[selInd];
            }
            else if (checkBox_timeSeries2.Checked)
            {
                int selInd = fitOptions2.SelectedNode.Index;
                selectedResult = SelectedResults[selInd];
            }
            else if (checkBox_timeSeries3.Checked)
            {
                int selInd = fitOptions3.SelectedNode.Index;
                selectedResult = SelectedResults[selInd];
            }
            else
            {
                throw new Exception("No series selected");
            }

            //WHAT TO DO WITH THE RESULT?
            MessageBox.Show($"Selected {selectedResult.ToString()}");
        }

        private void fitOptions1_SelectedIndexChanged(object sender, EventArgs e)
        {
            NodeChanged(fitOptions1, 1);
        }

        private void fitOptions2_SelectedIndexChanged(object sender, EventArgs e)
        {
            NodeChanged(fitOptions2, 2);
        }

        private void NodeChanged(TreeView fitOptions, int chartIndex)
        {
            TreeNode node = fitOptions.SelectedNode;
            if (node.Nodes.Count > 0)       //Don't load off parent nodes
                return;
            flowLayoutPanel_Charts.Controls.Remove(timeSeries1);
            flowLayoutPanel_Charts.Controls.Remove(timeSeries2);
            string parentText = fitOptions.SelectedNode.Parent.Text;
            string selectedText = fitOptions.SelectedNode.Text;
            OptimizationResult selectedResult = (from OptimizationResult or in SelectedResults
                                 where or.Schedule.ToIntervalString() == parentText && or.RegressionUnderTest.ToString() == selectedText
                                 select or).First();
            if (chartIndex == 1)
                timeSeries1 = new TimeSeriesChart(selectedResult.BucketedSums, selectedResult.RegressionUnderTest);
            else if (chartIndex == 2)
                timeSeries2 = new TimeSeriesChart(selectedResult.BucketedSums, selectedResult.RegressionUnderTest);
            else
                throw new Exception("Unexpected TimeSeriesChart index");

            flowLayoutPanel_Charts.Controls.Add(timeSeries1);
            if (DisplayCount >= 2)
                flowLayoutPanel_Charts.Controls.Add(timeSeries2);
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
            TimeSeriesChart.default_chartHeight = flowLayoutPanel_Charts.Height - 3;        //Overwrite the chart's default size -- allows you to not have to reset every time a different fit option is selected
            TimeSeriesChart.default_chartWidth = flowLayoutPanel_Charts.Width - 3;
            PopulateFitOptions(fitOptions1);
            NodeChanged(fitOptions1, 1);
            fitOptions1.Show();

            this.flowLayoutPanel_Options.Controls.Add(fitOptions1);
            fitOptions1.Height = this.flowLayoutPanel_Options.Height;
            fitOptions1.Width = this.flowLayoutPanel_Options.Width;
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
            fitOptions1 = PopulateFitOptions(fitOptions1);
            NodeChanged(fitOptions1, 1);
            fitOptions2 = PopulateFitOptions(fitOptions2);
            NodeChanged(fitOptions2, 2);
            fitOptions1.Show();
            fitOptions2.Show();

            this.flowLayoutPanel_Options.Controls.Add(fitOptions1);
            fitOptions1.Height = this.flowLayoutPanel_Options.Height / 2 - 2;
            fitOptions1.Width = this.flowLayoutPanel_Options.Width;

            this.flowLayoutPanel_Options.Controls.Add(fitOptions2);
            fitOptions2.Height = this.flowLayoutPanel_Options.Height / 2 - 2;
            fitOptions2.Width = this.flowLayoutPanel_Options.Width;

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

            fitOptions1 = PopulateFitOptions(fitOptions1);
            fitOptions2 = PopulateFitOptions(fitOptions2);
            fitOptions3 = PopulateFitOptions(fitOptions3);
            fitOptions1.Show();
            fitOptions2.Show();
            fitOptions3.Show();

            this.flowLayoutPanel_Charts.Controls.Add(timeSeries1);
            timeSeries1.Show();
            this.flowLayoutPanel_Charts.Controls.Add(timeSeries2);
            timeSeries2.Show();
            this.flowLayoutPanel_Charts.Controls.Add(timeSeries3);
            timeSeries3.Show();

            this.flowLayoutPanel_Options.Controls.Add(fitOptions1);
            fitOptions1.Height = this.flowLayoutPanel_Options.Height / 3 - 2;
            fitOptions1.Width = this.flowLayoutPanel_Options.Width;

            this.flowLayoutPanel_Options.Controls.Add(fitOptions2);
            fitOptions2.Height = this.flowLayoutPanel_Options.Height / 3 - 2;
            fitOptions2.Width = this.flowLayoutPanel_Options.Width;

            this.flowLayoutPanel_Options.Controls.Add(fitOptions3);
            fitOptions3.Height = this.flowLayoutPanel_Options.Height / 3 - 2;
            fitOptions3.Width = this.flowLayoutPanel_Options.Width;

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
