using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Input;

namespace DNA_Test.Scheduler
{
    public partial class SchedulerForm : Form
    {
        private bool specifyDate { get; set; } = true;
        private DatePicker DatePicker_Start { get; set; }
        private DatePicker DatePicker_End { get; set; }
        public SchedulerForm()
        {
            //Auto-populate the form from the selection if pertinent
            InitializeComponent();

            DatePicker_Start = datePicker_Custom1.Picker;
            DatePicker_End = datePicker_Custom2.Picker;
            //Need to set up events for the custom wpf datepicker
            DatePicker_Start.SelectedDateChanged += DatePicker_Start_SelectedDateChanged;
            DatePicker_End.SelectedDateChanged += DatePicker_End_SelectedDateChanged;
            DatePicker_End.GotFocus += DatePicker_End_Focus;

            PopulateCombobox();
            
            if (!AutoPopulate())
            {
                //Defaults
                DatePicker_Start.SelectedDate = DateTime.Today;
                //dateTimePicker_End.Value = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
                DatePicker_End.SelectedDate = DateTime.Today.AddDays(1);
            }
            comboBox_IntervalLength.Text = "";
            comboBox_IntervalType.SelectedIndex = 0;

            textBox_Periods.Text = (DatePicker_End.SelectedDate - DatePicker_Start.SelectedDate).Value.TotalDays.ToString();
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
            try
            {
                Excel.Range selection = DNA_Test.MyAddin.MyApp.Selection;
                //Autopop the datetimepickers
                object[,] cellValues = selection.Value;
                object[] values = new object[selection.Cells.Count];
                int i = 0;
                foreach (object cv in cellValues)
                {
                    values[i] = cv;
                    i++;
                }
                int firstIndex = values.Length;
                int lastIndex = 0;
                DateTime d1 = DateTime.FromOADate(0);
                DateTime d2 = DateTime.FromOADate(0);
                bool inDateRegion = false;
                for (int j = 0; j < values.Length; j++)
                {
                    if (values[j] == null)
                        continue;
                    if (DateTime.TryParse(values[j].ToString(), out DateTime dt))
                    {
                        inDateRegion = true;
                        if (firstIndex == values.Length)
                        {
                            d1 = dt;
                            firstIndex = j;
                        }
                        if (firstIndex != values.Length)
                        {
                            d2 = dt;
                            lastIndex = j;
                        }
                    }
                    else if (inDateRegion == true)
                    {
                        break;
                    }
                }
                if (d1.ToOADate() != 0 && d2.ToOADate() != 0 && d2.ToOADate() >= d1.ToOADate())
                {
                    DatePicker_Start.SelectedDate = d1;
                    DatePicker_End.SelectedDate = d2;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
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
            DatePicker_Start.SelectedDate = dt;
        }
        public void SelectEndDatetimePicker(DateTime dt)
        {
            if(dt.ToOADate() < (DatePicker_Start.SelectedDate ?? throw new Exception("Start date is null")).ToOADate())
            {
                //if end date is before the start date -- privilege the start date by setting the end date equal to the start date
                DatePicker_End.SelectedDate = DatePicker_Start.SelectedDate;
            }
            else
            {
                DatePicker_End.SelectedDate = dt;
            }
        }
        #endregion

        private void button_Insert_Click(object sender, EventArgs e)
        {
            try
            {
                Excel.Range selection = MyAddin.MyApp.Selection;
                Double.TryParse(comboBox_IntervalLength.Text, out double intervalLength);
                DateTime startDate = DatePicker_Start.SelectedDate ?? throw new Exception("Start date is null");
                DateTime endDate = DatePicker_End.SelectedDate ?? throw new Exception("End date is null");
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
                if (points > 10000)
                {
                    MessageBox.Show("Cannot place a schedule of over 10,000 dates.");
                    return;
                }

                if (points > cellsLength)
                {
                    DialogResult dlgRst = MessageBox.Show($"Selection was not large enough to place all {points} schedule values. Expand selection to place full schedule?", "Expand Range?", MessageBoxButtons.YesNo);
                    if (dlgRst == DialogResult.Yes)
                    {
                        try
                        {
                            if (isVertical)
                                selection = selection.Resize[points, 1];
                            else
                                selection = selection.Resize[1, points];
                        }
                        catch (System.Runtime.InteropServices.COMException)
                        {
                            MessageBox.Show("Selection cannot be expanded");
                        }
                    }
                    cellValues = new object[selection.Cells.Rows.Count, selection.Cells.Columns.Count];
                }

                //Overwrite cellValues array with the schedule values -- if too many cells are selected, just fill the ones you need
                for (int i = 0; i < Math.Min(selection.Cells.Count, scheduler.GetMidpoints().Count()); i++)
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
            }
            catch(Exception ex) //BOH invalid inputs should throw exceptions for the developer while FOH invalid inputs should notify the user
            {
                switch (ex.Message)
                {
                    case "Cannot use decimal interval lengths for non-daily interval types":
                        MessageBox.Show(ex.Message);
                        break;
                    case "Start date occurs on or after end date":
                        MessageBox.Show(ex.Message);
                        break;
                    default:
                        MessageBox.Show("Unhandled error in SchedulerForm");
                        break;
                }
            }
            
            //Check the array to make sure it's big enough to handle the schedule -- warn user if not all values are printed
            
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox_IntervalType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (specifyDate)
                UpdatePeriods();
            else if (!specifyDate)
                UpdateEndDate();
            else
                throw new Exception("Radio button selection error");

            if ((comboBox_IntervalType.Text == "Months" || comboBox_IntervalType.Text == "Years") && (DatePicker_Start.SelectedDate ?? throw new Exception("Start date is null")).Day > 28)
            {
                DateTime startDate = (DatePicker_Start.SelectedDate ?? throw new Exception("Start date is null"));
                DatePicker_Start.SelectedDate = new DateTime(startDate.Year, startDate.Month, 28);
            }
        }

        private void comboBox_IntervalLength_SelectedValueChanged(object sender, EventArgs e)
        {
            if (specifyDate)
                UpdatePeriods();
            else
                UpdateEndDate();
        }

        private void UpdateEndDate()
        {
            //ASSUME PERIODS IS TRUE
            if (comboBox_IntervalLength.Text == null)
                return;
            if (!int.TryParse(comboBox_IntervalLength.Text, out int intervalLength))
                return;
            if (!int.TryParse(textBox_Periods.Text, out int periods))
                return; //If the periods box doesn't contain an integer, just do nothing
            switch (comboBox_IntervalType.Text)
            {
                case "Days":
                    DatePicker_End.SelectedDate = (DatePicker_Start.SelectedDate ?? throw new Exception("Start date is null")).AddDays(periods * intervalLength);
                    break;
                case "Weeks":
                    DatePicker_End.SelectedDate = (DatePicker_Start.SelectedDate ?? throw new Exception("Start date is null")).AddDays(periods * intervalLength * 7);
                    break;
                case "Months":
                    DatePicker_End.SelectedDate = (DatePicker_Start.SelectedDate ?? throw new Exception("Start date is null")).AddMonths(periods * intervalLength);
                    break;
                case "Years":
                    DatePicker_End.SelectedDate = (DatePicker_Start.SelectedDate ?? throw new Exception("Start date is null")).AddYears(periods * intervalLength);
                    break;
                default:
                    break;
            }
        }

        private void UpdatePeriods()
        {
            if(DatePicker_Start.SelectedDate is null || DatePicker_End.SelectedDate is null)
                return;
            //ASSUME END DATE IS TRUE
            if (comboBox_IntervalLength.Text == null)
                return;
            if (!int.TryParse(comboBox_IntervalLength.Text, out int intervalLength))
                return;
            double totalDays = (DatePicker_End.SelectedDate - DatePicker_Start.SelectedDate).Value.TotalDays;
            //Recalculate the Period combo value
            int period;
            switch (comboBox_IntervalType.Text)
            {
                case "Days":
                    period = Convert.ToInt32(totalDays / intervalLength);
                    if (period >= 1)
                        textBox_Periods.Text = period.ToString();
                    else
                        textBox_Periods.Text = "0";
                    break;
                case "Weeks":
                    period = Convert.ToInt32(totalDays / 7 / intervalLength);
                    if (period >= 1)
                        textBox_Periods.Text = period.ToString();
                    else
                        textBox_Periods.Text = "0";
                    break;
                case "Months":
                    DateTime checkDate;
                    for(int i = 0; i < 12000; i++)
                    {
                        checkDate = (DatePicker_Start.SelectedDate ?? throw new Exception("Start date is null")).AddMonths(i);
                        if (checkDate.ToOADate() == (DatePicker_End.SelectedDate ?? throw new Exception("End date is null")).ToOADate())
                        {
                            if (i >= 1)
                                textBox_Periods.Text = i.ToString();
                            else
                                textBox_Periods.Text = "0";
                            break;
                        }
                        else if (checkDate.ToOADate() > (DatePicker_End.SelectedDate ?? throw new Exception("End date is null")).ToOADate())
                        {
                            if (i >= 2)
                                textBox_Periods.Text = (i - 1).ToString();
                            else if (i >= 1)
                                textBox_Periods.Text = i.ToString();
                            else
                                textBox_Periods.Text = "0";
                            break;
                        }
                    }
                    break;
                case "Years":
                    //These are going to require adding years until a greater oadate is reached
                    DateTime checkDate2;
                    for (int i = 0; i < 12000; i++)
                    {
                        checkDate2 = (DatePicker_Start.SelectedDate ?? throw new Exception("Start date is null")).AddYears(i);
                        if (checkDate2.ToOADate() == (DatePicker_End.SelectedDate ?? throw new Exception("End date is null")).ToOADate())
                        {
                            if(i >= 1)
                                textBox_Periods.Text = i.ToString();
                            else
                                textBox_Periods.Text = "0";
                            break;
                        }
                        else if (checkDate2.ToOADate() > (DatePicker_End.SelectedDate ?? throw new Exception("End date is null")).ToOADate())
                        {
                            if (i >= 2)
                                textBox_Periods.Text = (i - 1).ToString();
                            else
                                textBox_Periods.Text = "0";
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void DatePicker_End_SelectedDateChanged(object sender, EventArgs e)
        {
            if(specifyDate == true)
                UpdatePeriods();
        }

        private void DatePicker_Start_SelectedDateChanged(object sender, EventArgs e)
        {
            UpdateEndDate();
            //Verify that the selection is not past the 28th -- if so, move it to the 28th
            if ((comboBox_IntervalType.Text == "Months" || comboBox_IntervalType.Text == "Years") && (DatePicker_Start.SelectedDate ?? throw new Exception("Start date is null")).Day > 28)
            {
                DateTime startDate = (DatePicker_Start.SelectedDate ?? throw new Exception("Start date is null"));
                DatePicker_Start.SelectedDate = new DateTime(startDate.Year, startDate.Month, 28);
            }
        }

        private void comboBox_Periods_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox_Periods_TextChanged(object sender, EventArgs e)
        {
            if (specifyDate == true) 
                return;
            //Update the end date when the periods are manually changed
            if (!int.TryParse(textBox_Periods.Text, out int periods))
                return;
            DateTime startDate = DatePicker_Start.SelectedDate ?? throw new Exception("Start date is null");
            if (comboBox_IntervalLength.Text == null)
                return;
            if (!int.TryParse(comboBox_IntervalLength.Text, out int intervalLength))
                return;
            double totalDays = (DatePicker_End.SelectedDate - DatePicker_Start.SelectedDate).Value.TotalDays;
            //Recalculate the Period combo value
            switch (comboBox_IntervalType.Text)
            {
                case "Days":
                    DatePicker_End.SelectedDate = startDate.AddDays(periods * intervalLength);
                    break;
                case "Weeks":
                    DatePicker_End.SelectedDate = startDate.AddDays(periods * 7 * intervalLength);
                    break;
                case "Months":
                    DatePicker_End.SelectedDate = startDate.AddMonths(periods * intervalLength);
                    break;
                case "Years":
                    DatePicker_End.SelectedDate = startDate.AddYears(periods * intervalLength);
                    break;
                default:
                    break;
            }
        }

        private void textBox_Periods_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            specifyDate = false;
        }

        private void DatePicker_End_Focus(object sender, EventArgs e)
        {
            specifyDate = true;
        }

        private void SchedulerForm_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            string indexPath = @"C:\Users\grins\Documents\CASE_Help\Index.html";
            System.Diagnostics.Process.Start(indexPath);
        }

    }
}
