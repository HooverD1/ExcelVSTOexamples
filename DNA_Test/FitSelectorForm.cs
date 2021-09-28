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
        private bool CancelPredictAtChangedEvent { get; set; } = false;
        public OptimizationResult[] SelectedResults { get; set; }
        private FitTimeSeriesChart timeSeries1 { get; set; }
        private FitTimeSeriesChart timeSeries2 { get; set; }
        private FitTimeSeriesChart timeSeries3 { get; set; }
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

            this.datePicker_PredictAt.Visibility = System.Windows.Visibility.Hidden;        //Default to hidden
            this.datePicker_PredictAt.PreviewMouseLeftButtonDown += AllowPredictAtEvent;
            this.datePicker_PredictAt.Picker.SelectedDateChanged += DatePicker_Changed;


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
            comboBox_PredictAt.SelectedIndex = 1;
            //timeSeries1.UpdateBoxPlotSeries(TimeSeriesChart.Prediction.AtMean);
            
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
            UpdateBoxPlot(timeSeries1);
            SyncChartAxes();
        }

        private void fitOptions2_SelectedIndexChanged(object sender, EventArgs e)
        {
            NodeChanged(fitOptions2, 2);
            UpdateBoxPlot(timeSeries2);
            SyncChartAxes();
        }

        private void NodeChanged(TreeView fitOptions, int chartIndex)
        {
            TreeNode node = fitOptions.SelectedNode;
            foreach(TreeNode n in fitOptions.Nodes)
            {
                n.BackColor = System.Drawing.Color.White;
                foreach (TreeNode n2 in n.Nodes)
                {
                    n2.BackColor = System.Drawing.Color.White;
                }
            }
            fitOptions.SelectedNode.BackColor = System.Drawing.Color.FromArgb(200, 200, 200);  //Set selection to grey
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
                timeSeries1 = new FitTimeSeriesChart(selectedResult.BucketedSums, selectedResult.RegressionUnderTest, selectedResult.Schedule, comboBox_PredictAt.SelectedIndex);
            else if (chartIndex == 2)
                timeSeries2 = new FitTimeSeriesChart(selectedResult.BucketedSums, selectedResult.RegressionUnderTest, selectedResult.Schedule, comboBox_PredictAt.SelectedIndex);
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
                        UpdateBoxPlots();
                        break;
                    case 2:
                        LoadTwoCharts();
                        UpdateBoxPlots();
                        break;
                    default:
                        throw new Exception("Unexpected amount of charts");
                }
            }
        }

        private void LoadOneChart()
        {
            this.timeSeries2 = null;
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
            FitTimeSeriesChart.default_chartHeight = flowLayoutPanel_Charts.Height - 3;        //Overwrite the chart's default size -- allows you to not have to reset every time a different fit option is selected
            FitTimeSeriesChart.default_chartWidth = flowLayoutPanel_Charts.Width - 3;
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
            FitTimeSeriesChart.default_chartHeight = flowLayoutPanel_Charts.Height / 2 - 6;        //Overwrite the chart's default size -- allows you to not have to reset every time a different fit option is selected
            FitTimeSeriesChart.default_chartWidth = flowLayoutPanel_Charts.Width;
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

            //Make axes line up between chart controls
            SyncChartAxes();

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

        private void SyncChartAxes()
        {
            if (timeSeries1 != null && timeSeries2 != null)
            {
                if (!Double.IsNaN(timeSeries1.chartArea.AxisX.Minimum) &&
                    !Double.IsNaN(timeSeries1.chartArea.AxisX.Maximum) &&
                    !Double.IsNaN(timeSeries2.chartArea.AxisX.Minimum) &&
                    !Double.IsNaN(timeSeries2.chartArea.AxisX.Maximum) &&
                    !Double.IsNaN(timeSeries2.chartArea.AxisY.Minimum) &&
                    !Double.IsNaN(timeSeries2.chartArea.AxisY.Maximum) &&
                    !Double.IsNaN(timeSeries2.chartArea.AxisY.Minimum) &&
                    !Double.IsNaN(timeSeries2.chartArea.AxisY.Maximum))
                {
                    double min_x = Math.Min(timeSeries1.chartArea.AxisX.Minimum, timeSeries2.chartArea.AxisX.Minimum);
                    double max_x = Math.Max(timeSeries1.chartArea.AxisX.Maximum, timeSeries2.chartArea.AxisX.Maximum);
                    double min_y = Math.Min(timeSeries1.chartArea.AxisY.Minimum, timeSeries2.chartArea.AxisY.Minimum);
                    double max_y = Math.Max(timeSeries1.chartArea.AxisY.Maximum, timeSeries2.chartArea.AxisY.Maximum);
                    timeSeries1.chartArea.AxisX.Minimum = min_x;
                    timeSeries1.chartArea.AxisX.Maximum = max_x;
                    timeSeries2.chartArea.AxisX.Minimum = min_x;
                    timeSeries2.chartArea.AxisX.Maximum = max_x;
                    timeSeries1.chartArea.AxisY.Minimum = min_y;
                    timeSeries1.chartArea.AxisY.Maximum = max_y;
                    timeSeries2.chartArea.AxisY.Minimum = min_y;
                    timeSeries2.chartArea.AxisY.Maximum = max_y;
                }
                //double axStep = Axis_Step(timeSeries1.chartArea.AxisY.Maximum - timeSeries1.chartArea.AxisY.Minimum, 5);
                //timeSeries1.chartArea.AxisY.Interval = axStep;
                //timeSeries2.chartArea.AxisY.Interval = axStep;
            }
        }

        private void UpdateBoxPlots()
        {
            if (timeSeries1 != null)
                UpdateBoxPlot(timeSeries1);
            if (timeSeries2 != null)
                UpdateBoxPlot(timeSeries2);
        }

        private void UpdateBoxPlot(TimeSeriesChart ts)
        {
            //0 = Predict at next interval
            //1 = Predict at mean
            //2 = Predict at value
            switch (comboBox_PredictAt.SelectedIndex)
            {
                case 0:
                    this.datePicker_PredictAt.Visibility = System.Windows.Visibility.Hidden;
                    this.timeSeries1.UpdateBoxPlotSeries(FitTimeSeriesChart.Prediction.AtNextInterval);
                    if(this.timeSeries2 != null)
                        this.timeSeries2.UpdateBoxPlotSeries(FitTimeSeriesChart.Prediction.AtNextInterval);
                    break;
                case 1:
                    this.datePicker_PredictAt.Visibility = System.Windows.Visibility.Hidden;
                    this.timeSeries1.UpdateBoxPlotSeries(FitTimeSeriesChart.Prediction.AtMean);
                    if(this.timeSeries2 != null)
                        this.timeSeries2.UpdateBoxPlotSeries(FitTimeSeriesChart.Prediction.AtMean);
                    break;
                case 2:
                    this.datePicker_PredictAt.Visibility = System.Windows.Visibility.Visible;
                    if (this.datePicker_PredictAt.Picker.SelectedDate == null)
                        return;
                    this.timeSeries1.UpdateBoxPlotSeries(FitTimeSeriesChart.Prediction.AtValue, ((DateTime)this.datePicker_PredictAt.Picker.SelectedDate).ToOADate());
                    if(this.timeSeries2 != null)
                        this.timeSeries2.UpdateBoxPlotSeries(FitTimeSeriesChart.Prediction.AtValue, ((DateTime)this.datePicker_PredictAt.Picker.SelectedDate).ToOADate());
                    break;
            }
        }
        private void comboBox_PredictAt_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBoxPlots();
        }

        private void AllowPredictAtEvent(object sender, EventArgs e)
        {
            //Only allow the predictAt date picker selected date change event after a mouse click.
            CancelPredictAtChangedEvent = false;
        }

        private void DatePicker_Changed(object sender, EventArgs e)
        {
            if (CancelPredictAtChangedEvent)
            {
                return;
            }
            if (datePicker_PredictAt.Picker.SelectedDate == null)
                return;
            //Move to nearest midpoint in the schedule
            var schedule = new Scheduler.Scheduler(this.timeSeries1.Schedule.GetIntervalLength(), (Scheduler.Scheduler.Interval)timeSeries1.Schedule.GetIntervalType(), timeSeries1.Schedule.GetStartDate(), (DateTime)datePicker_PredictAt.Picker.SelectedDate);
            double minDistance = double.MaxValue;
            int minDex = -1;
            for(int i=0; i < schedule.GetMidpoints().Length; i++)
            {
                double distance = Math.Abs((schedule.GetMidpoints()[i] - (DateTime)datePicker_PredictAt.Picker.SelectedDate).TotalSeconds);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    minDex = i;
                }
            }
            if(minDex == -1)
            {
                throw new Exception("Find smallest date distance error");
            }
            else
            {
                //Event fires twice in a row unexpectedly here... first time is handled, second time screws it up.
                datePicker_PredictAt.Picker.SelectedDate = schedule.GetMidpoints()[minDex];
            }
            //Update
            UpdateBoxPlots();
            CancelPredictAtChangedEvent = true;
        }

        private void button_TestPrint_Click(object sender, EventArgs e)
        {
            //Test print method
            this.timeSeries1.PrintChartImageToFile(@"C:\Users\grins\Desktop");
        }

        public double Axis_Step(double dRange, double dDesired)
        {
            double max = 0;
            double dDistance;
            if (dRange == 0)
                return 0;
            else if (dRange < 0)
                dRange = dRange * -1;
            if (dDesired < 4)
                dDesired = 4;
            else if (dDesired > 20)
                dDesired = 20;

            //Minimize dDistance to the desired step size -- dDistance is the number of steps we can actually do - hopefully the same as dDesired, but not always
            //
            double d10 = dRange / (10 * Math.Pow(10, Math.Log(dRange / 10)) / Math.Log(10));
            if (d10 < 2) d10 = 1000;
            if(d10 <= dDesired + 2)
            {
                max = 10;
                dDistance = Math.Abs(dDesired - d10);
            }

            double d2 = dRange / (10 * Math.Pow(10, Math.Log(dRange / 2)) / Math.Log(10));
            if (d2 < 2) d2 = 1000;
            if (d2 <= Math.Abs(dDesired - d2) && d2 <= dDesired + 2)
            {
                max = 2;
                dDistance = Math.Abs(dDesired - d2);
            }

            double d2_5 = dRange / (10 * Math.Pow(10, Math.Log(dRange / 2.5)) / Math.Log(10));
            if (d2_5 < 2) d2_5 = 1000;
            if (d2_5 <= Math.Abs(dDesired - d2_5) && d2_5 <= dDesired + 2)
            {
                max = 2.5;
                dDistance = Math.Abs(dDesired - d2_5);
            }

            double d20 = dRange / (10 * Math.Pow(10, Math.Log(dRange / 20)) / Math.Log(10));
            if (d20 < 2) d20 = 1000;
            if (d20 <= Math.Abs(dDesired - d20) && d20 <= dDesired + 2)
            {
                max = 20;
                dDistance = Math.Abs(dDesired - d20);
            }

            double d25 = dRange / (10 * Math.Pow(10, Math.Log(dRange / 25)) / Math.Log(10));
            if (d25 < 2) d25 = 1000;
            if (d25 <= Math.Abs(dDesired - d25) && d25 <= dDesired + 2)
            {
                max = 25;
                dDistance = Math.Abs(dDesired - d25);
            }

            double d5 = dRange / (10 * Math.Pow(10, Math.Log(dRange / 5)) / Math.Log(10));
            if (d5 < 2) d5 = 1000;
            if (d5 <= Math.Abs(dDesired - d5) && d5 <= dDesired + 2)
            {
                max = 5;
                dDistance = Math.Abs(dDesired - d5);
            }

            double d50 = dRange / (10 * Math.Pow(10, Math.Log(dRange / 50)) / Math.Log(10));
            if (d50 < 2) d50 = 1000;
            if (d50 <= Math.Abs(dDesired - d50) && d50 <= dDesired + 2)
            {
                max = 50;
                dDistance = Math.Abs(dDesired - d50);
            }

            double d100 = dRange / (10 * Math.Pow(10, Math.Log(dRange / 100)) / Math.Log(10));
            if (d100 < 2) d100 = 1000;
            if (d100 <= Math.Abs(dDesired - d100) && d100 <= dDesired + 2)
            {
                max = 100;
                dDistance = Math.Abs(dDesired - d100);
            }

            //MAX SET
            if(max == 100)
            {
                return 10 * Math.Pow(10, Math.Log(dRange / 100) / Math.Log(10));
            }
            else if(max == 10)
            {
                return 10 * Math.Pow(10, Math.Log(dRange / 10) / Math.Log(10));
            }
            else if (max == 20)
            {
                return 2 * Math.Pow(10, Math.Log(dRange / 20) / Math.Log(10));
            }
            else if (max == 2)
            {
                return 2 * Math.Pow(10, Math.Log(dRange / 2) / Math.Log(10));
            }
            else if (max == 2.5)
            {
                return 2.5 * Math.Pow(10, Math.Log(dRange / 2.5) / Math.Log(10));
            }
            else if (max == 25)
            {
                return 2.5 * Math.Pow(10, Math.Log(dRange / 25) / Math.Log(10));
            }
            else if (max == 50)
            {
                return 5 * Math.Pow(10, Math.Log(dRange / 50) / Math.Log(10));
            }
            else if (max == 5)
            {
                return 5 * Math.Pow(10, Math.Log(dRange / 5) / Math.Log(10));
            }
            else
            {
                throw new Exception("Axis Step calculation error");
            }
        }
    }
}
