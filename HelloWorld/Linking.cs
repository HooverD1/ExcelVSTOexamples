using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace HelloWorld
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Linking
    {
        public void FollowLink()
        {
            MessageBox.Show("Run converted code to follow the link");
        }
        
    }
}
