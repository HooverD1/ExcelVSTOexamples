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
            }
            comboBox_IntervalLength.Text = "";
            comboBox_IntervalType.SelectedIndex = 0;

            textBox_Periods.Enabled = false;
            textBox_Periods.Text = (dateTimePicker_End.Value - dateTimePicker_Start.Value).TotalDays.ToString();
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
                    dateTimePicker_Start.Value = d1;
                    dateTimePicker_End.Value = d2;
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
            if(points > 10000)
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
                    catch(System.Runtime.InteropServices.COMException)
                    {
                        MessageBox.Show("Selection cannot be expanded");
                    }
                }
                cellValues = new object[selection.Cells.Rows.Count, selection.Cells.Columns.Count];
            }
            
            //Overwrite cellValues array with the schedule values -- if too many cells are selected, just fill the ones you need
            for(int i = 0; i < Math.Min(selection.Cells.Count, scheduler.GetMidpoints().Count()); i++)
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
            
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton_SpecifyPeriods_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_SpecifyPeriods.Checked)
            {
                this.dateTimePicker_End.Enabled = false;
                this.textBox_Periods.Enabled = true;

            }
            
        }

        private void radioButton_SpecifyDate_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_SpecifyDate.Checked)
            {
                this.dateTimePicker_End.Enabled = true;
                this.textBox_Periods.Enabled = false;
            }
            
        }

        private void comboBox_IntervalType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (radioButton_SpecifyDate.Checked)
                UpdatePeriods();
            else if (radioButton_SpecifyPeriods.Checked)
                UpdateEndDate();
            else
                throw new Exception("Radio button selection error");
        }

        private void comboBox_IntervalLength_SelectedValueChanged(object sender, EventArgs e)
        {
            if (radioButton_SpecifyDate.Checked)
                UpdatePeriods();
            else if (radioButton_SpecifyPeriods.Checked)
                UpdateEndDate();
            else
                throw new Exception("Radio button selection error");
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
                    dateTimePicker_End.Value = dateTimePicker_Start.Value.AddDays(periods * intervalLength);
                    break;
                case "Weeks":
                    dateTimePicker_End.Value = dateTimePicker_Start.Value.AddDays(periods * intervalLength * 7);
                    break;
                case "Months":
                    dateTimePicker_End.Value = dateTimePicker_Start.Value.AddMonths(periods * intervalLength);
                    break;
                case "Years":
                    dateTimePicker_End.Value = dateTimePicker_Start.Value.AddYears(periods * intervalLength);
                    break;
                default:
                    break;
            }
        }

        private void UpdatePeriods()
        {
            //ASSUME END DATE IS TRUE
            if (comboBox_IntervalLength.Text == null)
                return;
            if (!int.TryParse(comboBox_IntervalLength.Text, out int intervalLength))
                return;
            double totalDays = (dateTimePicker_End.Value - dateTimePicker_Start.Value).TotalDays;
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
                        checkDate = dateTimePicker_Start.Value.AddMonths(i);
                        if (checkDate.ToOADate() == dateTimePicker_End.Value.ToOADate())
                        {
                            if (i >= 1)
                                textBox_Periods.Text = i.ToString();
                            else
                                textBox_Periods.Text = "0";
                            break;
                        }
                        else if (checkDate.ToOADate() > dateTimePicker_End.Value.ToOADate())
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
                        checkDate2 = dateTimePicker_Start.Value.AddYears(i);
                        if (checkDate2.ToOADate() == dateTimePicker_End.Value.ToOADate())
                        {
                            if(i >= 1)
                                textBox_Periods.Text = i.ToString();
                            else
                                textBox_Periods.Text = "0";
                            break;
                        }
                        else if (checkDate2.ToOADate() > dateTimePicker_End.Value.ToOADate())
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

        private void dateTimePicker_End_ValueChanged(object sender, EventArgs e)
        {
            if(dateTimePicker_End.Enabled)
                UpdatePeriods();
        }

        private void dateTimePicker_Start_ValueChanged(object sender, EventArgs e)
        {
            UpdateEndDate();
            //Verify that the selection is not past the 28th -- if so, move it to the 28th
            if (dateTimePicker_Start.Value.Day > 28)
            {
                DateTime startDate = dateTimePicker_Start.Value;
                dateTimePicker_Start.Value = new DateTime(startDate.Year, startDate.Month, 28);
            }
        }

        private void comboBox_Periods_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox_Periods_TextChanged(object sender, EventArgs e)
        {
            if (!textBox_Periods.Enabled)      //Only run this when it's enabled.
                return;
            //Update the end date when the periods are manually changed
            if (!int.TryParse(textBox_Periods.Text, out int periods))
                return;
            DateTime startDate = dateTimePicker_Start.Value;
            if (comboBox_IntervalLength.Text == null)
                return;
            if (!int.TryParse(comboBox_IntervalLength.Text, out int intervalLength))
                return;
            double totalDays = (dateTimePicker_End.Value - dateTimePicker_Start.Value).TotalDays;
            //Recalculate the Period combo value
            switch (comboBox_IntervalType.Text)
            {
                case "Days":
                    dateTimePicker_End.Value = startDate.AddDays(periods * intervalLength);
                    break;
                case "Weeks":
                    dateTimePicker_End.Value = startDate.AddDays(periods * 7 * intervalLength);
                    break;
                case "Months":
                    dateTimePicker_End.Value = startDate.AddMonths(periods * intervalLength);
                    break;
                case "Years":
                    dateTimePicker_End.Value = startDate.AddYears(periods * intervalLength);
                    break;
                default:
                    break;
            }
        }
    }
}
