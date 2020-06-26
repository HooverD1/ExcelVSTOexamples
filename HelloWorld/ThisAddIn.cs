using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public partial class ThisAddIn
    {
        public static Excel.Application MyApp { get; set; }     //handle on the Excel application
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            MyApp = Globals.ThisAddIn.Application;      //Grab Excel at startup.
            Utilities.LoadKeybinds();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            //need to look for any background Excels that I started and terminate them... 
            //maybe keep a global stack of them for reference as they're created?
        }

        private COM_Visibles utilities;
        protected override object RequestComAddInAutomationService()
        {
            if (utilities == null)
                utilities = new COM_Visibles();
            return utilities;
        }
        
        public static dynamic OpenWorkbook(string path, Excel.Application tempApp = null)
        {
            if (tempApp == null)
                return MyApp.Workbooks.Open(path);
            else if (tempApp is Excel.Application)
                return tempApp.Workbooks.Open(path);
            else
                throw new Exception();
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
