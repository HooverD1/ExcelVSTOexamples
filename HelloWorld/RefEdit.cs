using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloWorld
{
    public class RefEdit
    {
        private Form Sender { get; set; }
        public Button RefButton { get; set; }
        public TextBox RefTextbox { get; set; }
        public RefEdit(Form Sender, Button RefButton, TextBox RefTextbox)
        {
            this.Sender = Sender;
            this.RefButton = RefButton;
            this.RefTextbox = RefTextbox;
        }

        public void OnClick()
        {
            ExcelReference GetReferenceForm = new ExcelReference(this.Sender);
            GetReferenceForm.Show();
            this.Sender.Close();    //close the first window
        }

        //close window, open 
    }
}
