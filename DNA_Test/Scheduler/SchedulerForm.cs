using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace DNA_Test.Scheduler
{
    public partial class SchedulerForm : Form
    {
        public SchedulerForm()
        {
            //Auto-populate the form from the selection if pertinent
            
            InitializeComponent();
            
            dateTimePicker_Start.Format = DateTimePickerFormat.Custom;
            dateTimePicker_Start.CustomFormat = "MMM d, yyyy HH:mm:ss";
            dateTimePicker_End.Format = DateTimePickerFormat.Custom;
            dateTimePicker_End.CustomFormat = "MMM d, yyyy HH:mm:ss";
            PopulateCombobox();
            if (!AutoPopulate())
            {
                //Defaults
                dateTimePicker_Start.Value = DateTime.Today;
                //dateTimePicker_End.Value = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
                dateTimePicker_End.Value = DateTime.Today.AddDays(1);
                comboBox_IntervalLength.Text = "1";
                comboBox_IntervalType.SelectedIndex = 0;
            }

        }
        public bool VerifySingleDimension()
        {
            //Verify that the selection is only a single column or row
            Excel.Range selection = MyAddin.MyApp.Selection;
            if (selection.Columns.Count > 1 && selection.Rows.Count > 1)
                return false;
            else
                return true;
        }
        private bool AutoPopulate()
        {
            Excel.Range selection = MyAddin.MyApp.Selection;
            object[,] cellValues;
            if (selection.Value != null)
                 cellValues = selection.Value;
            else
            {
                //Nothing to autopopulate from - abort
                return false;
            }
            //Find if the selection is a row or a column
            if (cellValues.GetLength(0) > 1 && cellValues.GetLength(1) > 1)
                throw new Exception("Invalid selection size");      //This should have been caught when the form was launched
            bool isVertical;
            if (cellValues.GetLength(0) > 1)
                isVertical = true;
            else
                isVertical = false;
            object[] oneDimValues;
            if (isVertical)
            {
                //column
                oneDimValues = new object[cellValues.GetLength(0)];
                for(int i = 0; i < cellValues.GetLength(0); i++)
                {
                    oneDimValues[i] = cellValues[i+1, 1];
                }
            }
            else
            {
                //row
                oneDimValues = new object[cellValues.GetLength(1)];
                for (int i = 0; i < cellValues.GetLength(1); i++)
                {
                    oneDimValues[i] = cellValues[1, i+1];
                }
            }
            
            //Determine if the values are datetimes
            bool[] isSchedule = new bool[oneDimValues.Length];      //determine how much of the selection is a schedule and autopop from those
            DateTime[] existingDateTimes = new DateTime[oneDimValues.Length];
            for(int i = 0; i < oneDimValues.Length; i++)
            {
                if(oneDimValues[i] != null && DateTime.TryParse(oneDimValues[i].ToString(), out DateTime dt))
                {
                    isSchedule[i] = true;
                    existingDateTimes[i] = dt;
                }
                else if (oneDimValues[i] != null && Double.TryParse(oneDimValues[i].ToString(), out double dt_double))
                {
                    DateTime dt2 = DateTime.FromOADate(dt_double);
                    if (dt2.Year >= 1950 && dt2.Year <= 2100)
                    {
                        //If the year makes sense, assume it's a date
                        isSchedule[i] = true;
                        existingDateTimes[i] = dt2;
                    }
                    else
                        isSchedule[i] = false;
                }
                else
                {
                    isSchedule[i] = false;
                }
            }

            //CONVERT OneDimValues into an array of midpoints, then construct them with Scheduler.ConstructFromMidpoints
            //Use the constructed Scheduler to fill out the form
            List<DateTime> midpoints = new List<DateTime>();
            int firstScheduleIndex = -1;
            int lastScheduleIndex = -1;
            for (int i = 0; i < oneDimValues.Length; i++)
            {
                //Find the indexes of the first contiguous range of schedule values in oneDimValues
                if (isSchedule[i] && firstScheduleIndex == -1)
                    firstScheduleIndex = i;
                else if (!isSchedule[i] && firstScheduleIndex > -1)
                    lastScheduleIndex = i-1;
            }
            if (firstScheduleIndex > -1 && lastScheduleIndex == -1)
            {
                lastScheduleIndex = oneDimValues.Length - 1;
            }
            if(firstScheduleIndex > -1 && lastScheduleIndex > -1)
            { 
                for (int i = firstScheduleIndex; i <= lastScheduleIndex; i++)
                {
                    midpoints.Add(existingDateTimes[i]);
                }
                Scheduler loadScheduler = Scheduler.ConstructFromMidpoints(midpoints.ToArray());
                this.dateTimePicker_Start.Value = loadScheduler.GetEndpoints().First();
                this.dateTimePicker_End.Value = loadScheduler.GetEndpoints().Last();
                this.comboBox_IntervalLength.Text = loadScheduler.GetIntervalLength().ToString();
                this.comboBox_IntervalType.Text = loadScheduler.GetIntervalTypeString();
                return true;
            }
            else
            {
                return false;
            }
        }
        private void PopulateCombobox()
        {
            comboBox_IntervalType.Items.Add("Days");
            comboBox_IntervalType.Items.Add("Weeks");
            comboBox_IntervalType.Items.Add("Months");
            comboBox_IntervalType.Items.Add("Years");

            for (int i = 1; i <= 365; i++)
                comboBox_IntervalLength.Items.Add(i);

            comboBox_IntervalLength.SelectedIndex = 0;
        }

        #region Load Schedule() UDF defaults
        //THESE ARE USED TO LOAD DEFAULTS FROM UDF PARAMETERS -- DO NOTHING ON ERRORS IN ORDER TO LOAD BLANK STATE
        public void SelectListboxIndex(int index)
        {
            if (comboBox_IntervalType.Items.Count < index)
                return;
            if (index < 1)
                return;
            comboBox_IntervalType.SelectedIndex = index - 1;
        }        
        public void SelectStartDatetimePicker(DateTime dt)
        {
            dateTimePicker_Start.Value = dt;
        }
        public void SelectEndDatetimePicker(DateTime dt)
        {
            if(dt.ToOADate() < dateTimePicker_Start.Value.ToOADate())
            {
                //if end date is before the start date -- privilege the start date by setting the end date equal to the start date
                dateTimePicker_End.Value = dateTimePicker_Start.Value;
            }
            else
            {
                dateTimePicker_End.Value = dt;
            }
        }
        #endregion

        private void button_Insert_Click(object sender, EventArgs e)
        {
            Excel.Range selection = MyAddin.MyApp.Selection;
            Double.TryParse(comboBox_IntervalLength.Text, out double intervalLength);
            DateTime startDate = dateTimePicker_Start.Value;
            DateTime endDate = dateTimePicker_End.Value;
            Scheduler.Interval intervalType;
            switch (comboBox_IntervalType.Text)
            {
                case "Days":
                    intervalType = Scheduler.Interval.Daily;
                    break;
                case "Weeks":
                    intervalType = Scheduler.Interval.Weekly;
                    break;
                case "Months":
                    intervalType = Scheduler.Interval.Monthly;
                    break;
                case "Years":
                    intervalType = Scheduler.Interval.Yearly;
                    break;
                default:
                    throw new Exception("Unknown interval type");
            }

            var st = startDate.ToOADate();
            var en = endDate.ToOADate();
            if (startDate.ToOADate() > endDate.ToOADate())
            {
                MessageBox.Show("End date must be after start date.");
                return;
            }

            Scheduler scheduler = new Scheduler(intervalLength, intervalType, startDate, endDate);
            //Define the array with the correct size.
            object[,] cellValues = new object[selection.Cells.Rows.Count, selection.Cells.Columns.Count];
            //Check the selection to make sure it's a single column or row
            if (cellValues.GetLength(0) > 1 && cellValues.GetLength(1) > 1)
                throw new Exception("Invalid selection size");      //This should have been caught when the form was launched
            int cellsLength;
            bool isVertical;
            if (cellValues.GetLength(0) > 1)
            {
                cellsLength = cellValues.GetLength(0);
                isVertical = true;
            }
            else
            {
                cellsLength = cellValues.GetLength(1);
                isVertical = false;
            }
            int points = scheduler.GetMidpoints().Length;
            //Overwrite cellValues array with the schedule values -- if too many cells are selected, just fill the ones you need
            for(int i = 0; i < Math.Min(cellsLength, points); i++)
            {
                if (isVertical)
                    cellValues[i, 0] = scheduler.GetMidpoints()[i].ToOADate().ToString();
                else
                    cellValues[0, i] = scheduler.GetMidpoints()[i].ToOADate().ToString();
            }
            //Return the array to the .Value property
            selection.Value = cellValues;
            selection.NumberFormat = "MM/dd/yyyy HH:mm:ss";
            this.Close();
            //Check the array to make sure it's big enough to handle the schedule -- warn user if not all values are printed
            if (points > cellsLength)
            {
                MessageBox.Show("Selection was not large enough to place all schedule values");
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
