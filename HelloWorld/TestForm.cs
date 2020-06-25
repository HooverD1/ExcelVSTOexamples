using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloWorld
{
    public partial class TestForm : Form
    {
        RefEdit refEdit;
        public TestForm()   //constructor
        {
            InitializeComponent();
            refEdit = new RefEdit(this, btnSelectRange, txtRangeSelect);      //create the buttons first, then set up the RefEdit object
        }
        
        public void EditLabel(Label myLabel)
        {
            myLabel.Text = "New Text";
        }

        private void btnSelectRange_Click(object sender, EventArgs e)
        {
            //when clicked, fire a method in refEdit to get the reference
            refEdit.OnClick();
        }


    }
}
