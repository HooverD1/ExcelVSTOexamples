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

namespace DNA_Test
{
    public partial class DateSelector : Form
    {
        public DateSelector()
        {
            InitializeComponent();
            if (!AutoPopulate())
            {
                dateTimePicker1.Value = DateTime.Today.AddHours(12);
            }
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MMM d, yyyy HH:mm:ss";

        }

        private bool AutoPopulate()
        {
            Excel.Range selection = DNA_Test.MyAddin.MyApp.Selection;
            if (selection.Cells.Count > 1)
                selection = selection.Cells[1, 1];
            if (selection.Value == null)
                return false;
            //Try to parse the selection as a date
            if (DateTime.TryParse(selection.Value.ToString(), out DateTime dt))
            {
                this.dateTimePicker1.Value = dt;
                return true;
            }
            else
                return false;
        }

        private void button_Insert_Click(object sender, EventArgs e)
        {
            Excel.Range selection = DNA_Test.MyAddin.MyApp.Selection;
            if (selection.Cells.Count > 1)
                throw new Exception("Selection too large");

            selection.Value = this.dateTimePicker1.Value;
            selection.NumberFormat = "MM/dd/yyyy HH:mm:ss";
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
