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

namespace HelloWorld
{
    public partial class ExcelReference : Form
    {
        Excel.Worksheet ws;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ws = Globals.ThisAddIn.Application.ActiveSheet;
            ws.SelectionChange += ws_SelectionChange;
        }
        public ExcelReference(Form sender)
        {
            InitializeComponent();
            sender.Close();
            //need an event that fires when the user selects a range
        }
        void ws_SelectionChange(Excel.Range Target)
        {
            this.txtRange.Text = Target.Address;
        }

        private void btnGotRange_Click(object sender, EventArgs e)
        {
            //have the reference. need to xfer it back through...
            var newTestForm = new TestForm();
            
            newTestForm.Text = this.Text;
            newTestForm.Enabled = false;
            newTestForm.Show();
            this.Close();
        }
    }
}
